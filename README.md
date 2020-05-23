# CopyWords
This is a helper tool for adding new Danish words into [Anki](http://ankisrs.net/).

I have customized Anki's card template to contain additional information about a word, such as transcription, sound file, etc. CopyWord tool makes it easier to fill a new card by automatically finding a word in online Danish dictionary and parsing different parts. Then you can click on various "Copy text" buttons to copy text into clipboard and paste it into Anki editor.

First type a word or its part in the "Search" texbox and click FInd. If this word exists in the [Den Danske Ordbog](http://ordnet.dk/ddo/) online dictionary, it will be parsed and presented in the tool.
Use "copy" buttons to copy relevant text into clipboard and paste it into the Anki card editor.

![Copy word parts into Anki](https://raw.githubusercontent.com/evgenygunko/CopyWords/master/img/Copy_word_parts.png)

#### Card templates
A card template can be created in Anki but unfortunately cannot be exported. It is saved with a "deck", which is a set of cards that a user wants to learn.
A user adds new cards with new words into the collection and then will learn and remember them.

My card templates:
- Forward card template (when a word in foreign language is shown and you need to guess a translation) ![Forward card](https://raw.githubusercontent.com/evgenygunko/CopyWords/master/img/Card_template_forward.png)
- Reverse card template (when translation is shown and you need to type a word in foreign language) ![Reverse card](https://raw.githubusercontent.com/evgenygunko/CopyWords/master/img/Card_template_reverse.png)

# Danish-Russian offline dictionary
This is a set of image files with a photo copies of hard copy dictionary. CopyWords tries to display an image which contains a selected word so that it is possible to check the translation.
The window also has "Move forward" and "Move back" buttons for easier navigation.
The "Danish-Russian offline dictionary" open by clicking on "DanRus" button and requires that the dictionary is present on a hard drive.

![Danish-Russian dictionary](https://raw.githubusercontent.com/evgenygunko/CopyWords/master/img/Danish_Russian_dictionary.png)

## Sound
"Play sound" button will download mp3 file (if it exists) and play it.
"Save sound" button will normalize the sound file (change its volume) and save into Anki's media collection folder.

## Settings
Settings dialog can be opened from the context menu.
- "Path to Anki sounds folder" specifies a path to the Anki's media collection for current user (it contains media files which are displayed or played on the cards)
- "Path to mp3gain" specifies a path to [mp3gain](http://mp3gain.sourceforge.net/) utility which is used for normalizing sound volume in mp3 files

## How to build solution
The solution has referenced a nuget package https://github.com/evgenygunko/CopyWords.Parsers which is not added to any public nuget feed. You need to save the nuget package to local drive and configure VS or nuget.config to use this folder as a local nuget feed.