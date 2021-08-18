@echo off

:: Path variables - direct these to the appropriate locations!
set DeckExtractorLocation=D:\Documents\Programs\C Sharp\YuGiOh Save Deck Extractor\YuGiOh Save Deck Extractor\bin\Debug\netcoreapp2.1

:: Note that this assumes that your script outputs to ./Output, and that your file name
:: is YGO_2020 (set it in the YuGi_compress.py file: OUTFILES = "Output/YGO_2020")
set ScriptLocation=D:\Documents\Video Games\PC\Yu-Gi-Oh Legacy of the Duelist\Modding

set SteamSaveGameLocation=C:\Program Files (x86)\Steam\userdata\85198859\1150640\remote\savegame.dat

set SteamDataLocation=C:\Program Files (x86)\Steam\steamapps\common\Yu-Gi-Oh! Legacy of the Duelist Link Evolution

set DeckToExtract=Opponent
set DeckToReplace=1classic_hard_bandit

@echo on

:: Run the .net app to grab your save file and replace the one in the extractor directory
@echo Extracting deck data...
dotnet "%DeckExtractorLocation%\YuGiOh Save Deck Extractor.dll" "%SteamSaveGameLocation%" "%ScriptLocation%" "%DeckToExtract%" "%DeckToReplace%"

if %ERRORLEVEL% NEQ 0 (
	@echo Deck insertion failed! See the output for details...
	pause
	exit
)

:: Once that's done, we'll compress the files into the .dat and .toc files
@echo Compressing game data...
python "%ScriptLocation%\YuGi_compress.py"

:: Now we move the output to the steam directory
@echo Copying files to steam location...
copy /Y "%ScriptLocation%\Output\YGO_2020.dat" "%SteamDataLocation%"
copy /Y "%ScriptLocation%\Output\YGO_2020.toc" "%SteamDataLocation%"

@echo All done! Enjoy!
pause