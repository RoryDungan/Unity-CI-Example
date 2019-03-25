# Set project source to the full path to the directory containing this script
$src = $PSScriptRoot

# Create temporary file for the Unity log. 
# We need this because if multiple editor instances are open we can't rely on the contents of the default Editor.log
# containing output just from this instance.
$logfile = [System.IO.Path]::GetTempFileName()
# Path to Unity - change to match your install location
$unity = 'C:\Program Files\Unity\Hub\Editor\2018.3.7f1\Editor\Unity.exe'

Write-Host "Test run started:" (Get-Date).ToString()

$process = $null
try {
    $unityArgs = "-nographics -batchmode -projectPath `"$src`" -logFile `"$logFile`" -runTests -testPlatform playmode"
    $process = Start-Process -FilePath "$unity" -ArgumentList $unityArgs -PassThru
    Wait-Process -Id $process.Id

    # Write log output to standard output. TODO: Can we tail the file and output in real time as it's being written to?
    Get-Content $logfile
}
finally {
    # Kill Unity if this script was killed. This makes sure the build actually stops if we cancel the job in Jenkins.
    if ($process -ne $null -And -Not $process.HasExited) {
        Stop-Process $process.Id
    }

    # Remove temporary log file
    Remove-Item $logfile
}

# Exit script with Unity exit code
Exit $process.ExitCode