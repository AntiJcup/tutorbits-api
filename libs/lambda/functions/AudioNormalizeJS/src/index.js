const s3Util = require('./s3-util'),
	childProcessPromise = require('./child-process-promise'),
	path = require('path'),
	os = require('os'),
	ffmpeg = require('./normalizer'),
	MIME_TYPE =  "video/mp4";

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
			['-loglevel', 'error', '-y', '-i', localInputFile, '-preset', 'fast', localOutputFile],
			{env: process.env, cwd: workdir}
		)
		.then(() => ffmpeg.normalize({
			input: localOutputFile,
			output: localNormalOutputFile,
			loudness: {
				normalization: 'ebuR128',
				target:
				{
					input_i: -45,
					input_lra: 1.0,
					input_tp: -9.0
				}
			},
			verbose: true
		})
		.then(() => childProcessPromise.spawn(
			'/opt/bin/ffmpeg',
			['-loglevel', 'error', '-y', '-i', localNormalOutputFile, '-af', 'highpass=200, lowpass=1500', localQuietOutputFile],
			{env: process.env, cwd: workdir}
		)
		.then(() => s3Util.uploadFileToS3(inputBucket, outputFile, localQuietOutputFile, MIME_TYPE)))));
};