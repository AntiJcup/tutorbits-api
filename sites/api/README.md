# tutorbits-api

Setup: 
1. Create ec2 or other server
2. Create pem to login
3. Create rdp session to login
4. Login
5. Run this powershell script on ec2 "Install-WindowsFeature -name Web Server -IncludeManagementTools"
6. Download https://github-production-release-asset-2e65be.s3.amazonaws.com/46080325/f0f1a100-0012-11ea-8f97-1c155ff3df27?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIWNJYAX4CSVEH53A%2F20191113%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20191113T163509Z&X-Amz-Expires=300&X-Amz-Signature=da2ea03b377ed4d24a6262a26a989ef3f38213d75ea818203c436d2992eb939d&X-Amz-SignedHeaders=host&actor_id=7553037&response-content-disposition=attachment%3B%20filename%3Dwin-acme.v2.1.0.539.x64.pluggable.zip&response-content-type=application%2Foctet-stream on ec2
7. Extract
8. Open admin cmd in extracted folder
9. Run wacs.exe with manual and follow steps with *.domain
10. Setup SQL_PWD and SQL_UID for AWS SQL Login information in Environmental Variables IIS configuration editor (On the overall system.webServer/aspNetCore)
11. Download https://dotnet.microsoft.com/download/thank-you/dotnet-runtime-2.2.7-windows-hosting-bundle-installer and install
12. Download https://s3.amazonaws.com/aws-cli/AWSCLI64PY3.msi and install
13. Setup aws profile local-test




CloudFront:
1. www needs error page forwarding to index.html (400 and 403)