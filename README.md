# SSX Mod Manager

A Windows desktop application to manage mods for the SSX series. Provides a GUI to install, uninstall, enable/disable, and manage mod packages for supported SSX titles.

This repository contains:
- `SSXModManagerWinForm/` — WinForms UI application (main app)
- `SSX-Library/` — shared library with core mod-handling logic
- `SSXModManagerWinForm.slnx` — solution file

Key features
- Browse and organize installed mods
- Install/uninstall mod packages
- Enable/disable mods and manage load order
- Backup and restore mod configurations
- Game-specific logic separated in the SSX-Library

Requirements
- .NET 10 SDK / runtime
- Windows 10 or later
- Visual Studio 2022/2026 or dotnet CLI
- The target SSX game installed (the manager does not include game data)

Getting started (developer)
1. Clone the repository:
   git clone https://github.com/GlitcherOG/SSXModManager.git
2. If the library is a submodule, initialize submodules:
   git submodule update --init --recursive
3. Open the solution in Visual Studio or build with dotnet CLI:
   dotnet build SSXModManagerWinForm.slnx
4. Run the WinForms app from Visual Studio (F5) or:
   dotnet run --project SSXModManagerWinForm/SSXModManagerWinForm.csproj

Basic usage (end user)
- Launch the app.
- Point the app to your SSX game installation path (Settings / Game Setup).
- Import mod packages (zip/folder), enable/disable mods, and apply or revert changes.
- Use the backup feature before making large changes.

Configuration and game support
- Game-specific behavior is implemented in `SSX-Library`. See the library for how game paths, file patterns, and patch logic are handled.
- If a game is not auto-detected, set the installation path manually in the app settings.

Troubleshooting
- Ensure the .NET 10 runtime is installed.
- Run the app with administrator privileges if file operations fail.
- Verify the game folder is correct and the game is not running while applying mods.

Contributing
- Fork the repo and create a feature branch.
- Keep changes focused and add/update tests when applicable.
- To add support for another SSX title, extend `SSX-Library` and add UI integration in the WinForms app.

License
See the LICENSE file in the repository for license terms.

Acknowledgements
- Project maintained by GlitcherOG (origin: https://github.com/GlitcherOG/SSXModManager)
- Core library: https://github.com/GlitcherOG/SSX-Library
