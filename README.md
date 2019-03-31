# Unity Continuous Integration Example
Example game for my [Unity CI talk](https://docs.google.com/presentation/d/1fTgXJW1QZ4K_EeidvbB4OKnkhwUcp4co5Sa3zpFqZuk/edit?usp=sharing) at the [Brisbane Unity Meetup](https://www.meetup.com/Brisbane-Unity-Developers/events/259891477/)

The project was set up with Unity 2018.3, and is based on the [code from my previous talk](https://github.com/RoryDungan/MVVM-Cookie-Clicker) on the MVVM pattern and Unity Weld.

The aim of this example is to show how you can set up a Unity project to build from the command-line in order to set up an build pipeline using a continuous integration tool like [Jenkins](https://jenkins.io/). The content of the example isn't really important and could be substituted for any game - the important parts are the script in the `Editor` folder that lets us run trigger builds from the command line, and the Powershell and Bash scripts in the project root that help make running Unity from the command-line easier.

# Project structure

## BuildUtility.cs

This class provides static methods for invoking a Unity build. These can be triggerd [from the CLI](https://docs.unity3d.com/Manual/CommandLineArguments.html) and have different versions for building for Windows, Mac and Linux.

## build-config.json

Since we will not be using Unity's default build dialog to select which scenes we want to build, these will need to be specified some other way. I have chosen to store the list of scenes in a simple JSON file which gets read by `BuildUtility`, but you could alternatively store this in some other kind of config file, environment variable, or command-line arguments.

## unity-build.sh & unity-build.ps1

These scripts manage running Unity from the command-line, setting all the options we need in order to make a build, and piping output to the standard output. Since Unity doesn't write any output to the standard output stream, in order to get information like detailed information about build errors inside our Jenkins logs, we need to read the log file and pipe its output to the standard output. The script also creates a custom temporary log file so that multiple instances of Unity running on the same machine don't compete to try and write to the same default log file. Finally, this script also makes sure to kill the Unity process if it is killed so that we actually stop Unity if a build in Jenkins is cancelled.

The Bash script (`unity-build.sh`) in intended for Mac but *should* also work using the Linux preview release of Unity or on Windows using Git Bash, MSYS2 or Cygwin. The Powershell script (`unity-build.ps1`) is intended for Windows. Note that you may need to modify your execution policy settings to enable Powershell scripts.

## unity-tests.sh & unity-tests.ps1

These scripts are similar to `unity-build.sh` and `unity-build.ps1` except that instead of making a build they run the Unit tests in the project using the Unity Test framework.

# Running Unity from the command-line

To make a build for Mac using the tools in `BuildUtility` directly, navigate to the directory containing the project and run the following command:

    /Applications/Unity/Hub/Editor/2018.3.9f1/Unity.app/Contents/MacOS/Unity -nographics -batchmode -quit -projectPath . -executeMethod Build.BuildUtility.MakeBuildMacCLI -buildPath /path/to/build/output

Where

- `/Applications/Unity/Hub/Editor/2018.3.9f1/Unity.app/Contents/MacOS/Unity` is the path to the Unity Editor executable. This will depend on where you chose to install Unity and your platform. On Windows it will probably be something like `C:\Program Files\Unity\Hub\Editor\2018.3.9f1\Editor\Unity.exe`
- `-nographics` tells Unity not to create a graphical window. Useful for us because we don't need to interact with the window and we may want to run this on a build server with no graphics card. On a system with no graphics card, *not* specifying `-nographics` will crash Unity because it will be unable to initialise the 3d view.
- `-batchmode` tells Unity we are running batch commands from the CLI (as opposed to the default interactive mode)
- `-quit` ensures Unity quits once we're done making the build.
- `-projectPath .` tells Unity to open the project in the current working directory. Alternatively, you can specify the full path to the directory containing your project, which can be useful if you don't want your working directory to be the same as the project.
- `-executeMethod Build.BuildUtility.MakeBuildMacCLI` specifies the static editor method to run. In this case it's our custom editor method that creates a build. Substitute this for `MakeBuildLinuxCLI` or `MakeBuildWindowsCLI` to build for those platforms.
- `-buildPath /path/to/build/output` is the custom additional argument that `BuildUtility` parses to work out where to save your build. Substitute with the path to where you actually want to save your build.

## Using the helper scripts (unity-build.sh and unity-build.ps1)

Running Unity directly from the command line requires remembering a very long command, and you don't get log output in the console. The `unity-build.sh` and `unity-build.ps1` scripts are intended to solve these issues by abstracting the direct call to the Unity executable.

To make a build with either, just set some environment variables and then run the script:

```
# For the bash scripts
export BUILDS_DIR=/path/to/build/output
export BUILD_METHOD=Build.BuildUtility.MakeBuildMacCLI
export BUILD_TARGET=OSXUniversal
./unity-build.sh
```

```
# For the Powershell scripts
$env:BUILDS_DIR = "C:\path\to\build\output"
$env:BUILD_METHOD=Build.BuildUtility.MakeBuildWindowsCLI
$env:BUILD_TARGET=Win64
.\unity-build.ps1
```

The exit code of the script will match the exit code of the Unity process, with succes being 0 and build failure non-zero. The path to the Unity executable is stored as the variable `UNITY` within the script, so you'll probably need to modify that to match your installation and Unity version.

Note that the Bash script pipes Unity log messages to the console in real time build the Powershell script waits till Unity exits and then sends them all at once. This is because I couldn't work out a way to monitor the file and pipe outputs to the standard output in real time in Powershell, but I'll admit I am much more well-vested in Bash than Powershell so if you have an idea of how to fix this, feel free to create an issue or pull request!

The `unity-tests.sh` and `unity-tests.ps1` scripts work in much the same way as the scripts for making builds except that they don't require any additional environment variables to be set to specify settings. They will just run all the tests in the project in play mode and afterwards, output a report summary XML file in the project root. This report uses the NUnit format.
