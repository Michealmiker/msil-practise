@echo off

set ILASM_PATH="C:\Windows\Microsoft.NET\Framework64\v2.0.50727"

for /F "tokens=1,2 delims=\\" %%a in ("%1") do (
    set PATH=%%a
    set FILENAME=%%b
)

mkdir built\%PATH%

%ILASM_PATH%\ilasm.exe %1 /exe /output=%~dp0built\%1.exe