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
		localOutputFile = path.join(workdir, "local.mp4"),
		localNormalOutputFile = path.join(workdir, "localN.mp4"),
		localQuietOutputFile = path.join(workdir, "localQ.mp4");

	console.log('converting', inputBucket, 'using', inputFile);
	return s3Util.downloadFileFromS3(inputBucket, inputFile, localInputFile)
		.then(() => childProcessPromise.spawn(
			'/opt/bin/ffmpeg',
			['-loglevel', 'error', '-y', '-i', localInputFile, '-vcodec', 'libx264', '-vprofile', 'high', '-preset', 'ultrafast', '-b:v', '500k', '-maxrate', '500k', '-bufsize', '3000m', '-vf', 'scale=-1:480', '-codec:a', 'aac', '-strict', 'experimental', '-b:a', '128k', '-af', 'highpass=200, lowpass=1500, loudnorm=I=-35:TP=-1.5:LRA=20', localOutputFile],
			{ env: process.env, cwd: workdir }
		).then(() => s3Util.uploadFileToS3(inputBucket, outputFile, localOutputFile, MIME_TYPE)));
};