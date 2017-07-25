@echo off

SET AssemblyPath=Albion\Release
SET AssemblyName=Merlin

SET Target=Albion-Online.exe

SET LoadingAssembly=%AssemblyPath%\%AssemblyName%.dll
SET UnloadAssembly=%AssemblyPath%\unload\%AssemblyName%.dll

echo Unloading
if exist %UnloadAssembly% mono-assembly-injector -dll %UnloadAssembly% -target %Target% -namespace %AssemblyName% -class Core -method Unload
timeout 1

echo Loading
if exist %UnloadAssembly% del %UnloadAssembly%

timeout 1

copy /y %LoadingAssembly% %UnloadAssembly%

timeout 1

mono-assembly-injector -dll %LoadingAssembly% -target %Target% -namespace %AssemblyName% -class Core -method Load

