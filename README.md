# RequestifyTF2
RequesifyTF2 is a simple program for TF2, which supports plugins.
# Installation
1. Go to Download section and Forms and Core. Place them into one folder.
2. Make sure that u have VLC and VAC installed on your pc
3. Place Open
4. Press on Settings button
5. Select path to the game
6. Press Start button and Start your game!
7. When you joined a server open console and type "+voicerecord"
8. If u want to hear what you saying type "voice_loopback 1"
9. Done!
# Dowload
[Download](https://ci.appveyor.com/api/projects/weespin26279/RequestifyTF2/artifacts/scr%2FGUI%2FRequestifyTF2GUI%2Fbin%2FDebug%2FRequestifyTF2Forms.exe) Forms
[Download](https://ci.appveyor.com/api/projects/weespin26279/RequestifyTF2/artifacts/scr%2FPlugins%2FTTSPlugin%2Fbin%2FDebug%2FTTSPlugin.dll) Core
# Development
## Template
Just use a plugin from scr/Plugin folder as tempate.
# Methods
## OnLoad (should be non-static!)
Invokes on Load
## Execute (string nickname, List<string> arguments)
Invokes when someone executed a plugins command
# Events
## OnPlayerConnect => Returns a connected user nickname
## OnPlayerChat => Returns a user nickname and text (invoked if text is not a command!)
## OnPlayerKill => Returns Killer's Nickname, Killed Nickname and Weapon
## OnPlayerSuicide => Returns a user nickname
