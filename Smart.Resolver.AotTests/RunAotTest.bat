@echo off
setlocal enabledelayedexpansion

set "CONFIG=%~1"
if "%CONFIG%"=="" set "CONFIG=Release"

set "RID=%~2"
if "%RID%"=="" set "RID=win-x64"

set "PROJECT_DIR=%~dp0"
set "PROJECT=%PROJECT_DIR%Smart.Resolver.AotTests.csproj"
set "PUBLISH_DIR=%PROJECT_DIR%bin\%CONFIG%\net11.0\%RID%\publish"
set "EXE=%PUBLISH_DIR%\Smart.Resolver.AotTests.exe"

where vswhere.exe >nul 2>&1
if errorlevel 1 (
    set "VSWHERE_DIR=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer"
    if exist "!VSWHERE_DIR!\vswhere.exe" (
        set "PATH=!VSWHERE_DIR!;%PATH%"
    ) else (
        echo [WARN] vswhere.exe not found. Native link may fail.
        echo        Run this from a "Developer Command Prompt for VS" if it does.
    )
)

echo === Native AOT publish ^(%CONFIG% / %RID%^) ===
dotnet publish "%PROJECT%" -c %CONFIG% -r %RID%
if errorlevel 1 (
    echo.
    echo [FAIL] AOT publish failed.
    exit /b 1
)

if not exist "%EXE%" (
    echo.
    echo [FAIL] Native executable not found: "%EXE%"
    exit /b 1
)

echo.
echo === Running native AOT executable ===
"%EXE%"
if errorlevel 1 (
    echo.
    echo [FAIL] AOT smoke tests failed.
    exit /b 1
)

echo.
echo [PASS] AOT smoke tests passed.
exit /b 0
