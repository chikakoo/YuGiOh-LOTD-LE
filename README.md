# Summary
This repository consists of three different projects for _Yu-Gi-Oh! Legacy of the Duelist: Link Evolution_. For help on setting up the randomizer or opponent deck inserter, head over to the section titled _Setup Help_.

This is _not_ cross-platform. It is only set up to work with Windows.

# Credits
Code was used/modifed from the following locations to get this to work - full credits to them:
- For code used to write to the savegame.dat and to create the .ydc files: https://github.com/thomasneff/YGOLOTDPatchDraft/blob/master/YGOLOTDPatchDraft/FileUtilities.cs
- For code used to fix the checksum of the save file once modified: https://github.com/Arefu/Wolf/blob/d904ca93a84e49dfc86a7c573778f245a194bcd5/Elroy/GameSaveData.cs
- To MoonlitDeath, for the tutorial on modding in general: https://github.com/MoonlitDeath/Link-Evolution-Editing-Guide/wiki

# Projects
### YuGiOh API Output
This is a somewhat janky project that outputs an HTML page full of relevant card JSON data. This shouldn't really be useful for anyone not developing this project, so no need to go into much detail here.

### YuGiOh Save Deck Extractor
This project requires setup to use - see the _Setup Help_ section for deteails.

This project, along with its cooresponding batch script (titled _replace_opponent_deck_with_prebuilt.bat_) are used to replace an opponent's deck with one of the custom decks that you constructed in-game. This is useful if you want to duel against your own decks.

### YuGiOh Randomizer
This project requires setup to use - see the _Setup Help_ section for deteails.

This project, along with its cooresponding batch script (titled _randomize_player_and_opponent_decks.bat_) are used to generate random decks for yourself and an opponent. That way, you can get into a duel without knowing any of the cards involved. It currently has very basic logic, and will be improved upon in the future, including the ability to configure card distributions.

# Setup Help
This section is to set up the YuGiOh Save Deck Extractor and YuGiOh Randomizer projects.
### Tools/Things Required
- The Steam version of _Yu-Gi-Oh! Legacy of the Duelist: Link Evolution_
- The compiled version of the project you wish to use
   - This will consist of a bunch of a few different files, one of which should be a .dll of the project
   - If you don't want to compile them, you can download the latest release from this repo
- Python 3.8 or later (make sure it's added to PATH! This is a checkbox on the installer, if you need a quick fix.)
- _YuGi_compress.py_ and _YuGi_extract.py_, which are in a .rar file from a link on the following page under the text "download my python script here": https://github.com/MoonlitDeath/Link-Evolution-Editing-Guide/wiki
### Pre-Setup Things
These scripts will **modify your save and game data**. It's important that you **make backups** of them before running the scripts in case something bad happens to them!

Here are the typical locations of these files:
* Save data: _{Steam Install Path}\userdata\\{Steam ID Number}\1150640\remote_
  * The save file is _savegame.dat_ 
* Game data: _{Steam Install Path}\steamapps\common\Yu-Gi-Oh! Legacy of the Duelist Link Evolution_
  * The files you want to backup are _YGO_2020.dat_ and _YGO_2020.toc_
### Setting Up the Directory
If you haven't already, create an empty folder. Do the following with it: 
- Place the two python scripts there (_YuGi_compress.py_ and _YuGi_extract.py_)
- Place the two .bat files in the repo there (_replace_opponent_deck_with_prebuilt.bat_ and _randomizer_player_and_opponent_decks.bat_)
- **Copy** (not move) over the _YGO_2020.dat_ and _YGO_2020.toc_ from your Steam installation (see above)
- Create a new folder called _Output_
   - Both scripts will use this for as temporary location before overwriting your Steam files
### Modifying the scripts
Now that we have the files in place, there's some changes that need to be made for this to work.

First, the scripts expect the compile output to be in a specific place, so we need to change that script to output there.
- Open _YuGi_compress.py_ in your favorite text editor
   - Change the _OUTFILES_ variable so it equals "Output/YGO_2020". That is, the line should say exactly this:
     - OUTFILES = "Output/YGO_2020"

The following is required for the _YuGiOh Save Deck Extractor_ project:
- Open _replace_opponent_deck_with_prebuilt.bat_ and reassign the variables appropriately (note that you should **not** use quotes)
  - **DeckExtractorLocation**: The folder containing _YuGiOh Save Deck Extractor.dll_
  - **ScriptLocation**: The folder you just created, with the python and batch scripts, etc.
  - **SteamSaveLocation**: The path to the _savegame.dat_ file (**including the file name**) (see above)
  - **SteamDataLocation**: The folder containing _YGO.2020.dat_ and _YGO_2020.toc_ in your Steam installation (see above)
  - **DeckToExtract**: This is the name of your pre-built deck to replace the opponent's. Make sure it's a valid deck before running this script!
  - **DeckToReplace**: This is the name of the opponent's deck that will be repalced - it defaults to Bandit Keith's challenge deck
    - After extracting, the names will be in _<Current Folder>/YGO_2020/decks.zib_, should you want to change this

The following is required for the _YuGiOh Randomizer_ project:
- Open _randomize_player_and_opponent_decks.bat_ and reassign the variables appropriately (note that you should **not** use quotes)
  - **RandomizerLocation**: The folder containing YuGiOhRandomizer.dll
  - **ScriptLocation**: The folder you just created, with the python and batch scripts, etc.
  - **SteamSaveLocation**: The path to the _savegame.dat_ file (**including the file name**) (see above)
  - **SteamDataLocation**: The folder containing _YGO.2020.dat_ and _YGO_2020.toc_ in your Steam installation (see above)
  - **OpponentDeckToReplace**: This is the name of the opponent's deck the randomizer will replace - it defaults to Bandit Keith's challenge deck
    - After extracting, the names will be in _<Current Folder>/YGO_2020/decks.zib_, should you want to change this
   - **PlayerDeckToReplace**: This is the name of a custom deck that you created that will be overwritten with random cards. Make sure it's a valid deck before you run this script!

### Extracting the Game Files
Now that we're all set up, go ahead and run YuGi_extract.py. Either double-click it if you've enabled that, or run it via cmd using _python YuGi_extract.py_.

It will create a folder called _YGO_2020_. You can nagivate to YGO_2020/decks.zlib to get a list of all the opponent deck names you can replace with these scripts.

## Running the Scripts
Now we're ready to run the scripts! At this point, all you have to do is run the script you wish to use:
- **YuGiOh Save Deck Extractor**: replace_opponent_deck_with_prebuilt.bat
- **YuGiOh Randomizer**: randomize_player_and_opponent_decks.bat

Happy dueling!
