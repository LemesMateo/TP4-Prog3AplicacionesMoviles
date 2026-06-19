@echo off
"C:\Program Files\dotnet\dotnet.exe" build "C:\Users\mateo\source\repos\TP4-ProgMoviles\TP4-ProgMoviles.csproj" -c Debug > "C:\Users\mateo\source\repos\TP4-ProgMoviles\build.log" 2>&1
echo EXITCODE=%ERRORLEVEL%
