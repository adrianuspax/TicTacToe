@echo off
net session >nul 2>&1
if %errorLevel% == 0 (
    echo IF UNITY IS RUNNING, IT WILL BE CLOSED!.
) else (
    echo TRYING Access ADM
    powershell -Command "Start-Process '%~0' -Verb RunAs"
    exit /b
)

cd /d "%~dp0"

echo IS IT OKAY?
pause

echo Step 0/8: Closing Unity processes and auxiliary tools...

:: Closes Unity Hub
tasklist | find /i "Unity Hub.exe" > nul
if %errorlevel% == 0 (
    echo Closing Unity Hub...
    taskkill /F /IM "Unity Hub.exe" > nul
)

:: Closes Unity Editor
tasklist | find /i "Unity.exe" > nul
if %errorlevel% == 0 (
    echo Closing Unity Editor...
    taskkill /F /IM Unity.exe > nul
)

:: Closes ADB (Android Debug Bridge - comum bloquear arquivos)
tasklist | find /i "adb.exe" > nul
if %errorlevel% == 0 (
    echo Closing ADB...
    taskkill /F /IM adb.exe > nul
)

:: Closes Unity Crash Handler (pode travar arquivos)
tasklist | find /i "UnityCrashHandler64.exe" > nul
if %errorlevel% == 0 (
    echo Closing Unity Crash Handler...
    taskkill /F /IM UnityCrashHandler64.exe > nul
)

echo Step 1/8: Deleting Logs Folder...
if exist "Logs" (
    rmdir /s /q "Logs"
    echo Logs Folder Deleted.
) else (
    echo Logs Folder is not found!
)

echo Step 2/8: Deleting obj Folder...
if exist "obj" (
    rmdir /s /q "obj"
    echo obj Folder Deleted.
) else (
    echo obj Folder is not found!
)

echo Step 3/8: Deleting Temp Folder...
if exist "Temp" (
    rmdir /s /q "Temp"
    echo Temp Folder Deleted.
) else (
    echo Temp Folder is not found!
)

echo Step 4/8: Deleting Library Folder...
if exist "Library" (
    rmdir /s /q "Library"
    echo Library Folder Deleted.
) else (
    echo Library Folder is not found!
)

echo Step 5/8: Deleting .sln(x)...
del /q *.sln
del /q *.slnx
echo .sln(x) files deleted.

echo Step 6/8: Deleting .csproj files...
del /q *.csproj
echo .csproj files deleted.

echo Step 7/8: Deleting .mp4 files...
del /q *.mp4
echo .mp4 files deleted.

echo Step 8/8: Deleting Builds Folder...
if exist "Builds" (
    rmdir /s /q "Builds"
    echo Builds Folder Deleted.
) else (
    echo Builds Folder is not found!
)

echo All steps completed.
pause