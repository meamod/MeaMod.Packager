set mypath=%cd%
echo %mypath%

set SEARCH_BASE="MeaMod.Packager\bin\Release"
set FILTER=*.DLL
for /r %SEARCH_BASE% %%i in (%FILTER%) do (
signtool verify /pa /q "%%i" 1>nul 2>nul 
if errorlevel 1 (
signtool sign /n "James Weston" /fd SHA256 /d "MeaMod.Packager" /du "https://www.meamod.com" /tr http://timestamp.digicert.com /td sha256 "%%i"
)
)

rmdir /s /q MeaMod.Packager\publish

dotnet pack -c Release --no-build -p:PublishDir=.\publish

nuget sign "MeaMod.Packager\bin\Release\*.nupkg" -CertificateSubjectName "James Weston" -Timestamper http://timestamp.digicert.com -NonInteractive

nuget push "MeaMod.Packager\bin\Release\*.nupkg" -Source nuget.org -SkipDuplicate -NonInteractive

move "MeaMod.Packager\bin\Release\*.nupkg" nugetpackagearchive

