# this script uses [gsudo](https://github.com/gerardog/gsudo)

Push-Location
Set-Location $PSScriptRoot

sudo powershell {
	Start-Job { Stop-Process -Name PowerToys* } | Wait-Job > $null

	# change this to your PowerToys installation path
	$ptPath = 'C:\Program Files\PowerToys'
	$projectName = 'PowerToys Run Plugin Michael Brand (dl)'
	$safeProjectName = 'PowerToys_Run_Plugin_Michael_Brand_(dl)'
	#$debug = '.\bin\x64\Debug\net9.0-windows'
	$debug = "C:\Source\GitHub\PowerToys-Run-Plugin\PowerToys Run Plugin MB\bin\x64\Debug\net9.0-windows\*"
	$dest = "C:\Program Files\PowerToys\RunPlugins\$projectName"
	$files = @(
		"Community.PowerToys.Run.Plugin.$safeProjectName.deps.json",
		"Community.PowerToys.Run.Plugin.$safeProjectName.dll",
		'plugin.json',
		'Images'
	)
	

	Set-Location $debug
	mkdir $dest -Force -ErrorAction Ignore | Out-Null
	Copy-Item $debug $dest -Force -Recurse

	& "$ptPath\PowerToys.exe"
}

Pop-Location
