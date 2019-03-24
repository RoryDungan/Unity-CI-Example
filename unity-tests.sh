#!/bin/bash

# Kill subprocesses if this script is stoped. This makes sure Unity actually 
# stops if you cancel the Jenkins job.
trap 'kill $(jobs -pr)' SIGINT SIGTERM 

# Set project source to the full path to the directory containing this script
SRC=$( cd "$(dirname $0)" ; pwd -P )

# Create a temporary file for the log file.
# We need this because if multiple editor instances are open we can't rely on 
# the contents of the default Editor.log containing output just from this instance.
LOGFILE=$(mktemp)
# Path to Unity - change to match your install location
UNITY=/Applications/Unity/Hub/Editor/2018.3.9f1/Unity.app/Contents/MacOS/Unity 

echo Test run started: `date`

$UNITY -nographics -batchmode -projectPath "$SRC" -logFile "$LOGFILE" -runTests -testPlatform playmode &
unitypid=$!

# Pipe Unity log output to stdout
#
# Unity doesn't output to stdout but it does have a log file. We can pipe this to stdout
# by starting Unity in the background (using &) and using tail to read the log file and 
# write it to stdout. We want this script to return the same value as Unity returned so
# we save the Unity return value in unityreturn.
#
# Note that we also have to start tail in the background so that we can wait for Unity
# to exit and then kill tail. Otherwise, tail will keep running forever. 
tail -f -n +0 "$LOGFILE" &
tailpid=$!
wait $unitypid # Wait for Unity to finish
unityreturn=$?
kill $tailpid # Kill tail now that Unity has finished writing log messages.
wait $tailpid 2>/dev/null # Pipe stderr to /dev/null to suppress message saying process was killed.

# Remove temp file
rm "$LOGFILE"

# Return with the same value as the Unity process
exit $unityreturn
