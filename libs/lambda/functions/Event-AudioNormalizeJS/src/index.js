const s3Util = require('./s3-util'),
	childProcessPromise = require('./child-process-promise'),
	path = require('path'),
	os = require('os'),
	fs = require('fs');

function updateTranscodeStatus(workdir, transcodeDir, status) {
	const transcodeFileKey = path.join(transcodeDir, 'transcode_state.json'),
		tmpTranscodeFilePath = path.join(workdir, 'transcode_state.json');
	return s3Util.downloadFileFromS3(bucket, transcodeFileKey, tmpTranscodeFilePath).then(() => {
		let rawdata = fs.readFileSync(tmpTranscodeFilePath);
		let transcode_json = JSON.parse(rawdata);
		transcode_json["State"] = status;
		let data = JSON.stringify(transcode_json);
		fs.writeFileSync(tmpTranscodeFilePath, data);
		return s3Util.uploadFileToS3(bucket, transcodeFileKey, tmpTranscodeFilePath, 'application/json').then(() => {
			return transcode_json;
		});
	});
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
	return updateTranscodeStatus(workdir, outDir, 2).then((transcode_json) => {
		console.log('converting', inputBucket, key, 'using', inputFile);
		return s3Util.downloadFileFromS3(inputBucket, key, inputFile)
			.then(() => childProcessPromise.spawn(
				'/opt/bin/ffmpeg',
				['-loglevel', 'error', '-y', '-i', inputFile, '-af', 'highpass=200, lowpass=1500, loudnorm=I=-35:TP=-1.5:LRA=20', '-threads', '0', outputFile],
				{ env: process.env, cwd: workdir }
			))
			.then(() => s3Util.uploadFileToS3(transcode_json.TargetBucket, resultKey, outputFile, 'video/mp4'))
			.then(() => updateTranscodeStatus(workdir, outDir, 3))
			.catch((e) => {
				console.error('error', e);
				return updateTranscodeStatus(workdir, outDir, 4);
			});
	});
};