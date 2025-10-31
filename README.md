# TUI speed reader

Text User Interface (TUI) speed reader is a text based speed reader, which runs in your command prompt. 

## Features

Currently this program supports the following features:
* Reading text **automatically** or **manually** from a **text file** or from the **command line**
* Switching from automatic reading during manual reading and vice versa
* Displaying the status of the program by changing the title of the program
* Setting reading speed
* Setting the read mode to start reading from: manual or automatic
* Setting the speed option for reading: words per second/minute, text per second/minute
* Aligning the position of text to center in horizontal and vertical axis and their combinations
* Exiting the program after speed reading or keeping the program running
* Exiting from the program via the program

## How to use 

* For best chances of success use Windows 11 — it's the only operating system I've tested it on.
 If you don't have it installed, install .NET 9.0 from [this page](https://dotnet.microsoft.com/en-us/download).
* Download the `Speed.Reader.text.user.interface.zip` file from the release you wish to use — all the available releases are listed [here](https://github.com/Rudolf-Snail/TUI-speed-reader/releases).
* Extract its contents into a folder you want; the program is portable and doesn't support or need installation.
* Run the `SpeedReader TUI.exe` file.
* Follow instructions in the program.

### How to use manual reading mode
* Use the **`Right arrow`** key to go to the next word, if there is any, if you're currently on the last word this will go back to the options menu
* Use the **`Left arrow`** key to go to the previous word, if there is any, if you're currently on the first word this will go back to the options menu
* Use the **`Spacebar`** key to change the reading mode to automatic — this will not change the read mode the reading starts from
* Use the **`Escape`** key to go back to the options menu

### How to use automatic reading mode
* Use the **`Spacebar`** key to change the reading mode to manual — this will not change the read mode the reading starts from

## How to build it yourself from source code
* Navigate to `SpeedReader TUI` folder in your command line.
* Run the command `dotnet publish`. More info on the command is available [here](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish).
* Your compiled program is now in the folder `.\SpeedReader TUI\bin\Release\net9.0`.