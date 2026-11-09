$ErrorActionPreference = 'Stop'
# A fresh PowerShell process avoids Add-Type caching a previous test assembly.
Add-Type -Path @(
    (Join-Path $PSScriptRoot '../Logic/MonitorSelector.cs'),
    (Join-Path $PSScriptRoot 'MonitorSelectorTests.cs')
) -CompilerOptions '/langversion:7.3'
[XiboClient.Tests.MonitorSelectorTests]::Run()
