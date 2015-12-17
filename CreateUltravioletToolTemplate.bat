rmdir /s /q UltravioletTool_Template 2>nul
mkdir UltravioletTool_Template
xcopy UltravioletTool UltravioletTool_Template /s /e /y

cd UltravioletTool_Template
powershell -Command "(Get-Content Program.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Program.cs"
powershell -Command "(Get-Content Assets\GlobalFontID.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Assets\GlobalFontID.cs"
powershell -Command "(Get-Content UltravioletToolForm.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content UltravioletToolForm.cs"
powershell -Command "(Get-Content UltravioletToolForm.Designer.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content UltravioletToolForm.Designer.cs"
powershell -Command "(Get-Content Properties\Resources.Designer.cs) | ForEach-Object { $_ -creplace 'SAFE_PROJECT_NAME', '$safeprojectname$' -creplace 'PROJECT_NAME', '$projectname$' } | Set-Content Properties\Resources.Designer.cs"

del "Ultraviolet Tool (Windows Forms).zip" 2>nul
7z a "Ultraviolet Tool (Windows Forms).zip" * -xr!obj -xr!bin
