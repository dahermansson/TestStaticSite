@echo off
echo Deploying files...
xcopy %DEPLOYMENT_SOURCE% d:\home\site\static\ /Y