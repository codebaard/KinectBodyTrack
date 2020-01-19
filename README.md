# KinectBodyTrack

This application is required for the Synthesia Music Sequencer to work properly. Its essential function is to track persons within range and provide their coordinates to the server side application.

## Requirements

- The latest Kinect v2 SDK: https://www.microsoft.com/en-us/download/details.aspx?id=44561
- .NET 4.5 
- a physical Kinect v2 camera attached to the USB port of your device.

Since this application is written in and for Windows I can't provide a description how to make it work on MAC.

## Build

Use Visual Studio to make a release build. Works best with x64 configuration.
Make sure you build the **DummyListener** within the Project, if you want to run the application without the server side application.

## Run

- **Local:** call the `tracker.exe` application from within .\bin\x64\release\ with no cli arguments provided. The local listener will be started automatically.
- **Remote:** call the `tracker.exe` with IP Adress and port provided. Assumed the remotehost is at *192.168.0.100* with port *3000* it would look like this: `tracker.exe 192.168.0.100 3000`

If you did everything right, it should work smoothly.
