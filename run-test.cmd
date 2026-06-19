@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "C:\Users\mateo\source\repos\TP4-ProgMoviles\test-api.ps1" > "C:\Users\mateo\source\repos\TP4-ProgMoviles\test-api.out.log" 2>&1
echo DONE_EXIT=%ERRORLEVEL%
