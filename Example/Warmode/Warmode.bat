@echo off
copy /y bin\Debug\WarmodeSucks.dll ..\WarmodeSucks.dll
Set "MyProcess=Warmode.exe"
echo looking for %MyProcess%

:waitForWarMode
tasklist /NH /FI "imagename eq %MyProcess%" 2>nul |find /i "%MyProcess%" >nul
If not errorlevel 1 (Echo "%MyProcess%" is running, injecting ..) else (goto waitForWarMode)
..\UnityInject_x86.exe