Zip the code up
aws lambda update-function-code --function-name AudioNormalizeJS --zip-file *file*

dotnet lambda invoke-function AudioNormalizeJS --payload '{"BucketName":"tutorbitties", "Endpoint": "us-west-2", "Mp4Path": "projects/ceb04435-ef48-4027-9608-08d779a836f4/video/vid.mp4"}'


dotnet lambda invoke-function AudioNormalizeJS --payload "{\"BucketName\":\"tutorbitties\", \"Endpoint\": \"us-west-2\", \"WebmPath\":\"projects/ceb04435-ef48-4027-9608-08d779a836f4/video/vid.webm\", \"Mp4Path\": \"projects/ceb04435-ef48-4027-9608-08d779a836f4/video/vid.js.mp4\"}"