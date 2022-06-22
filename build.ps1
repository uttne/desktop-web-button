dotnet publish

if(Test-Path "./buld"){
    Remove-Item -Path "./buld" -Recurse -Force
}

robocopy "./src/DesktopWebButton/bin/Release/net6.0/publish" "./build" /E
