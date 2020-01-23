const s3Util = require('./s3-util'),
	childProcessPromise = require('./child-process-promise'),
	path = require('path'),
	os = require('os'),
	fs = require('fs');
const { promisify } = require('util');

const openFileAsync = promisify(fs.open);
const readFileAsync = promisify(fs.read);
const fstatAsync = promisify(fs.fstat);

function updateTranscode(workdir, transcodeDir, status, duration) {
	const transcodeFileKey = path.join(transcodeDir, 'transcode_state.json'),
		tmpTranscodeFilePath = path.join(workdir, 'transcode_state.json');
	return s3Util.downloadFileFromS3(bucket, transcodeFileKey, tmpTranscodeFilePath).then(() => {
		let rawdata = fs.readFileSync(tmpTranscodeFilePath);
		let transcode_json = JSON.parse(rawdata);
		if (status) {
			transcode_json["State"] = status;
		}

		if (duration) {
			transcode_json["DurationMS"] = duration;
		}
		let data = JSON.stringify(transcode_json);
		fs.writeFileSync(tmpTranscodeFilePath, data);
		return s3Util.uploadFileToS3(bucket, transcodeFileKey, tmpTranscodeFilePath, 'application/json').then(() => {
			return transcode_json;
		});
	});
}

async function getVideoLength(file) {
	const fd = await openFileAsync(file, 'r');
	const stat = await fstatAsync(fd);
	var buff = new Buffer(stat["size"]);
	const readFileRes = await readFileAsync(fd, buff, 0, stats["size"], 0);
	fs.closeSync(fd);

	var start = readFileRes.buffer.indexOf(new Buffer('mvhd')) + 17;
	var timeScale = readFileRes.buffer.readUInt32BE(start, 4);
	var duration = readFileRes.buffer.readUInt32BE(start + 4, 4);
	var movieLength = Math.floor(duration / timeScale);

	return movieLength;
}

exports.handler = function (eventObject, context) {
	const eventRecord = eventObject.Records && eventObject.Records[0],
		inputBucket = eventRecord.s3.bucket.name,
		key = eventRecord.s3.object.key,
		id = context.awsRequestId,
		resultKey = key.replace(/\.[^.]+$/, '.mp4'),
		workdir = os.tmpdir(),
		inputFile = path.join(workdir, id + path.extname(key)),
		outputFile = path.join(workdir, id + 'out.mp4');


	const elements = key.split('/');
	elements.splice(elements.length - 1, 1);
	const outDir = elements.join('/');
	return updateTranscode(workdir, outDir, 2).then(async (transcode_json) => {
		console.log('converting', inputBucket, key, 'using', inputFile);
		try {
			await s3Util.downloadFileFromS3(inputBucket, key, inputFile)
			await childProcessPromise.spawn(
				'/opt/bin/ffmpeg',
				['-loglevel', 'error', '-y', '-i', inputFile, '-af', 'highpass=200, lowpass=1500, loudnorm=I=-35:TP=-1.5:LRA=20', '-threads', '0', outputFile],
				{ env: process.env, cwd: workdir }
			);
			const videoLength = await getVideoLength(outputFile);
			await s3Util.uploadFileToS3(transcode_json.TargetBucket, resultKey, outputFile, 'video/mp4');
			await updateTranscode(workdir, outDir, 3, videoLength);
		} catch (e) {
			console.error('error', e);
			await updateTranscode(workdir, outDir, 4);
		}
	});
};