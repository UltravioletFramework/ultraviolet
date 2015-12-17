rmdir /s /q UltravioletGame_Template 2>nul
mkdir UltravioletGame_Template
xcopy UltravioletGame UltravioletGame_Template /s /e /y

cd UltravioletGame_Template
powershell -Command "(Get-Content Game.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Game.cs"
powershell -Command "(Get-Content Assets\GlobalFontID.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Assets\GlobalFontID.cs"
powershell -Command "(Get-Content Input\GameInputActions.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Input\GameInputActions.cs"
powershell -Command "(Get-Content Input\IUltravioletInputExtensions.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Input\IUltravioletInputExtensions.cs"

del "Ultraviolet Game.zip" 2>nul
7z a "Ultraviolet Game.zip" * -xr!obj -xr!bin