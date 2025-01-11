# Define the Visual Studio version and extension ID
$vsVersion = "2022\Professional"
$extensionId = "Autoharp.1e989351-28fe-46b9-a875-f7cdf035b81c"
$vsixFilePath = "Autoharp\bin\Debug\Autoharp.vsix"

$vsixInstallerPath = "C:\Program Files\Microsoft Visual Studio\$vsVersion\Common7\IDE\VSIXInstaller.exe"

if (-Not (Test-Path $vsixInstallerPath)) {
    Write-Error "VSIXInstaller.exe not found at $vsixInstallerPath"
    exit
}

Write-Host "Uninstalling Visual Studio extension with ID: $extensionId..."
$uninstallCommand = "Start-Process -FilePath '$vsixInstallerPath' -ArgumentList '/uninstall:`"$extensionId`"' -Wait"
Write-Host -ForegroundColor blue $uninstallCommand
Invoke-Expression $uninstallCommand

Write-Host "Installing Visual Studio extension from: $vsixFilePath..."
$installCommand = "Start-Process -FilePath `"$vsixInstallerPath`" -ArgumentList `"$vsixFilePath`" -Wait"
Write-Host -ForegroundColor blue $installCommand
Invoke-Expression $installCommand
