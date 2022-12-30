set mypath=%cd%
echo %mypath%

rmdir /s /q MeaMod.Packager\bin

dotnet clean
dotnet build -c Release

dotnet pack --include-symbols --no-build -c Release

nuget sign "MeaMod.Packager\bin\Release\*.nupkg" -CertificateSubjectName "James Weston" -Timestamper http://timestamp.digicert.com -NonInteractive

nuget push "MeaMod.Packager\bin\Release\*.nupkg" -Source nuget.org -SkipDuplicate -NonInteractive

move "MeaMod.Packager\bin\Release\*.nupkg" nugetpackagearchive

