@echo off

set ILASM_PATH="C:\Windows\Microsoft.NET\Framework64\v2.0.50727"

%ILASM_PATH%\ilasm.exe %1 /exe /output=%~dp0built\%1.exe