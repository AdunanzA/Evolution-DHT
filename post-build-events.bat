@ECHO off

SET VsTargetDir=%~1
SET VsSolutionName=%~2
SET VsSolutionDir=%~3

rem Questo file e' da modificare anche nel .crproj di confuser :(
SET VsTargetFile=Evolution.Dht.dll

REM Obfuscation

ECHO VsSolutionDir= %VsSolutionDir%
ECHO VsSolutionName= %VsSolutionName%
ECHO VsTargetDir= %VsTargetDir%
ECHO VsTargetFile= %VsTargetFile%
echo on
cd %VsSolutionDir%
"%VsSolutionDir%Confuser\Confuser.Console.exe" "%VsSolutionDir%Confuser\%VsSolutionName%.crproj"

REM Replacing non obfuscated binaries
xcopy  "%VsTargetDir%Confused\%vsTargetFile%" "%VsTargetDir%" /Y /F /R

REM @call "%VS110COMNTOOLS%VsDevCmd.bat"
REM @call "%VS110COMNTOOLS%VCVarsQueryRegistry.bat"
REM @call "%VS110COMNTOOLS%vsvars32.bat"
