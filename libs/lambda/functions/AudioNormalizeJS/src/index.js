const s3Util = require('./s3-util'),
	childProcessPromise = require('./child-process-promise'),
	path = require('path'),
	os = require('os'),
	ffmpeg = require('./normalizer'),
	MIME_TYPE = "video/mp4";

exports.handler = function (eventObject, context) {
	const inputBucket = eventObject.BucketName,
		inputFile = eventObject.WebmPath,
		workdir = os.tmpdir(),
		outputFile = eventObject.Mp4Path,
		localInputFile = path.join(workdir, "local.webm"),
		localOutputFile = path.join(workdir, "local.mp4");

	console.log('converting', inputBucket, 'using', inputFile);
	return s3Util.downloadFileFromS3(inputBucket, inputFile, localInputFile)
		.then(() => childProcessPromise.spawn(
			'/opt/bin/ffmpeg',
			['-loglevel', 'error', '-y', '-i', localInputFile, '-vcodec', 'libx264', '-filter:v', 'scale=-1:720', '-preset', 'veryfast', '-codec:a', 'aac', '-strict', 'experimental', '-af', 'highpass=200, lowpass=1500, loudnorm=I=-35:TP=-1.5:LRA=20', '-threads', '0', localOutputFile],
			{ env: process.env, cwd: workdir }
		).then(() => s3Util.uploadFileToS3(inputBucket, outputFile, localOutputFile, MIME_TYPE)));
};