function Check-Env ($variableName, $message) {
    if (![System.Environment]::GetEnvironmentVariable($variableName)) {
        Write-Host "$variableName environment variable not set!"
        Write-Host $message
        Exit 1
    }
}

Check-Env 'BUILDS_DIR' 'This should be set to the directory to save builds in.'
Check-Env 'BUILD_METHOD' 'This should be set to the C# method to invoke to create the build.'
Check-Env 'BUILD_TARGET' 'This should be set to the platform to build for.'

# Set project source to the full path to the directory containing this script
$src = $PSScriptRoot
$outdir = $Env:BUILDS_DIR

# Create temporary file for the Unity log. 
# We need this because if multiple editor instances are open we can't rely on the contents of the default Editor.log
# containing output just from this instance.
$logfile = [System.IO.Path]::GetTempFileName()
# Path to Unity - change to match your install location
$unity = 'C:\Program Files\Unity\Hub\Editor\2018.3.7f1\Editor\Unity.exe'

Write-Host "Building to $OUTDIR"
Write-Host "Build started:" (Get-Date).ToString()

$process = $null
try {
    $arguments = "-nographics -quit -batchmode -buildTarget $Env:BUILD_TARGET -projectPath $src -logFile '$logFile' -executeMethod Build.BuildUtility.MakeBuild -buildPath '$outdir'"
    $process = Start-Process $unity -PassThru
    Wait-Process -Id $process.Id

    # Write log output to standard output. TODO: Can we tail the file and output in real time as it's being written to?
    Get-Content $logfile
}
finally {
    # Kill Unity if this script was killed. This makes sure the build actually stops if we cancel the job in Jenkins.
    if ($process -ne $null) {
        Stop-Process $process.Id
    }

    # Remove temporary log file
    Remove-Item $logfile
}

# Exit script with Unity exit code
Exit $process.ExitCode