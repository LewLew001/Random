echo Cleaning Windows event logs...
for /f %%a in ('wevtutil el') do wevtutil cl "%%a"

echo Removing Windows Installer patch cache...
rmdir /q /s "C:\Windows\Installer\$PatchCache$"

echo Cleaning user temp directory...
del /f /q /s "%TEMP%\*.*"

echo Cleaning system temp directory...
del /f /q /s "C:\Windows\Temp\*.*"

echo Clearing prefetch files...
del /f /q /s "C:\Windows\Prefetch\*.*"

echo Flushing DNS cache...
ipconfig /flushdns

echo Cleanup complete.
pause
