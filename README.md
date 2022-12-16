Primary repository on [GitHub](https://github.com/CyberBotX/TShock_AutoTeam).

# TShock_AutoTeam

![License](https://img.shields.io/github/license/CyberBotX/TShock_AutoTeam?style=plastic)
![Forks](https://img.shields.io/github/forks/CyberBotX/TShock_AutoTeam?style=plastic)
![Stars](https://img.shields.io/github/stars/CyberBotX/TShock_AutoTeam?style=plastic)
![Watchers](https://img.shields.io/github/watchers/CyebrBotX/TShock_AutoTeam?style=plastic)

TShock_AutoTeam is a plugin for [TShock](https://github.com/Pryaxis/TShock) that will automatically join a player to their previous team (if any) on connect.

The information about the player's team is stored in TShock's database and is based on their character's name and the UUID assigned to them. As long as those match and the player was on a team previously, they will be joined to that team again when they connect.

## Prerequisites

* TShock v5.1.3 (There is no guarantee that this plugin will work with any other version of TShock)
* [.NET 6 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (needed by TShock itself as well)

## Installation

Simply place TShock_AutoTeam.dll into TShock's ServerPlugins directory. It should load when you start the server and use TShock's database, so no configuration is needed.

## Building

(You should only need to do this if you have forked the repository and are making changes.)

TShock_AutoTeam uses .NET 6, so you will need a minimum of [Visual Studio 2022](https://visualstudio.microsoft.com/) to build the plugin.

## Contact

If you have any questions, comments or concerns about TShock_AutoTeam:

* Contact me by email: cyberbotx@cyberbotx.com (please include TShock_AutoTeam in the subject line)
* Contact me on Discord: CyberBotX#8477
* Submit an issue via [GitHub's issue tracker](https://github.com/CyberBotX/TShock_AutoTeam/issues)
* Start a discussion via [GitHub's discussions](https://github.com/CyberBotX/TShock_AutoTeam/discussions)

# License

TShock_AutoTeam is licensed as follows:

```
The MIT License (MIT)

Copyright (c) 2022 Naram Qashat

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
