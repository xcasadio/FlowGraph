@echo off

rem We need to replace CSharpSyntax.dll in the release folder with the merged
rem one.

echo Merging

cd ..
call Merge.bat
cd CSharpSyntax

echo Copying files

del bin\Release\CSharpSyntax.dll
del bin\Release\Antlr3.Runtime.dll
copy ..\CSharpSyntax.dll bin\Release

echo Packing

..\.nuget\nuget.exe pack -prop configuration=release

pause
