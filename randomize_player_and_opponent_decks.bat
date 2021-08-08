@echo off

:: Path variables - direct these to the appropriate locations!
set RandomizerLocation=D:\Documents\Programs\C Sharp\YuGiOh Randomizer\YuGiOhRandomizer\bin\Debug\netcoreapp2.1

:: Note that this assumes that your script outputs to ./Output, and that your file name
:: is YGO_2020 (set it in the YuGi_compress.py file: OUTFILES = "Output/YGO_2020")
set ScriptLocation=D:\Documents\Video Games\PC\Yu-Gi-Oh Legacy of the Duelist\Modding

set SteamSaveGameLocation=C:\Program Files (x86)\Steam\userdata\85198859\1150640\remote\savegame.dat

set SteamDataLocation=C:\Program Files (x86)\Steam\steamapps\common\Yu-Gi-Oh! Legacy of the Duelist Link Evolution

set OpponentDeckToReplace=1classic_hard_bandit
set PlayerDeckToReplace=Random

@echo on

:: Run the .net app to randomize the deck files
:: This will edit your save file to give you a random deck in the specified slot
:: It will also put a random deck in the opponent's location to be compiled next
@echo Extracting deck data...
dotnet "%RandomizerLocation%\YuGiOhRandomizer.dll" "%SteamSaveGameLocation%" "%ScriptLocation%" "%OpponentDeckToReplace%" "%PlayerDeckToReplace%"

:: Once that's done, we'll compress the files into the .dat and .toc files
@echo Compressing game data...
python "%ScriptLocation%\YuGi_compress.py"

:: Now we move the output to the steam directory
@echo Copying files to steam location...
copy /Y "%ScriptLocation%\Output\YGO_2020.dat" "%SteamDataLocation%"
copy /Y "%ScriptLocation%\Output\YGO_2020.toc" "%SteamDataLocation%"

@echo All done! Enjoy!
pause