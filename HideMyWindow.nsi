!define MUI_PRODUCT "Hide My Window"
!define MUI_FILE "HideMyWindow_Install.exe"
!define MUI_VERSION "1.0b"
!define MUI_BRANDINGTEXT "Hide My Window Installer"

!include "C:\Program Files (x86)\NSIS\Contrib\zip2exe\modern.nsh"

Name "Hide My Window 1.0b"

OutFile "HideMyWindow_Install.exe"

InstallDir "$PROGRAMFILES\theDiary Software\HideMyWindow"

RequestExecutionLevel admin

