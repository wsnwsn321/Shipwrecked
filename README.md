#Small Room Productions
##Shipwrecked
###CSE 5912, The Ohio State University
###Team members: Drake Addis, Ben Clarke, Chris Crain, Ryan Liu, Greg Sop, Adam Sturgeon, Songnan Wu
###Instructor: Roger Crawfis

---

## Game Description

It's the year 5912. The Earth has long gone dark. Rumored to be an apocalyptic wasteland, teeming with vile creatures and radioactive gases, no one has returned from a journey to Earth since the year 5542. However, a heavily funded expedition has been launched to discover what exactly happened on Earth, and whether it ever has a chance to be habitable again. A team of the most elite scientists, soldiers and doctors was formed, and a frigate of the highest quality was used to transport them to Earth and be used as a home base during research. However, after unseen complications thanks to space hysteria, the team is now shipwrecked on the shoreline right next to an abandoned city, and it seems that all the rumors were true. The team must hold off all the monsters with whatever resources and abilities they have, and use cooperation, strategy and teamwork to destroy the monster nests, and hold out until repairs can be completed. From what turned into a research expedition, it's up to you to just get the team out alive.

---

## How To Start
Once you have the game built and running, select your graphics and input settings (if applicable) and select "Play!". Once passing the loading screen, and after the main menu appears, you have two options for playing the game. Select "Single Player" if you are by yourself, and "Multiplayer" if you are playing with others. In single player, select "New Game" if you are starting from scratch, or "Continue" if you have previous progress in the game. In Multiplayer, select "New Game", and then enter the nickname that will be used in your game. From there, if you are the group leader, create a lobby. This involves entering a lobby name of at least four characters. From there, wait for other players to join the lobby and then click "Start Game". If you are joining a group, first click "Refresh" to show the list of available lobbies. From there, select the lobby you want to join then click the button "Join Game". In both single player and multi-player, once the game starts, there will be an initial cutscene before gameplay. Press the spacebar if you would like to skip the cutscene.

---

##Default Controls
W/Up Arrow: Move up

A/Left Arrow: Move left

S/Down Arrow: Move down

D/Right Arrow: Move right

Space Bar: Jump (skip intro scene when applicable)

Escape: Return to main menu

Left Click: Shoot

Right Click: Aim Mode

F: Pick Up Items

Q: Special Ability

E: Special Skill

Shift: Sprint while moving

C: Front Flip

R: Reload

H: Dodge

---

##Menus
###Main Menu
New Game: Starts new game

Continue: 

Settings: 

Controls: Displays basic controls

Credits: Displays team member's names + pictures

###Pause Menu
Yet to be implemented
--- 

##Unity Package Folders
AI: Holds all script files dealing with artificial intelligence and spawning in game

Animations: Holds all animator files 

Audio: Holds all mp3 files used during gameplay

Fonts: Holds all text styles

Images: Holds all images used in credits, loading screen, etc.

Materials: Holds all materials used to decorate scenes and characters

Networking: Contains all assets needed for multiplayer functionality

OutlineEffect: Asset used to make ball more defined/brighter, give it more clear outline

Prefabs: Holds all copyable objects

Resources: Contains all animators for all characters

RockVR: Folder that holds Video Capture asset

Scenes: Holds all scenes. 

	1. AI_TestScene: Holds scene for developers to test and implement new artificial intelligence updates/strategies

	2: CharacterSelect: Holds scene for selecting between four characters in rotation format

	3: GameLevel: Main scene. Holds scene for where player spends most of game. Holds many models from Wasteland Asset, Crab Alien Asset, and other assets used for the main game

	4: LobbyScene: Holds lobby scene and logic for UI starting multiplayer games. 
	
	5: MainMenuMultiplayer: Holds UI menu for playing multiplayer games

	6: MainMenuSelect: Holds UI menu for selecting between single player, multiplayer, and quitting the game

	7: MainMenuSinglePlayer: Holds UI menu for playing singleplayer

	8: QuitCredits: Holds UI scene for when player quits game

	9: RegularCredits: Holds UI scene for when player selects "Credits" on main menu screen

	10: IntroScene: Holds video that starts when beginning new game

	11: LoadingScene: Scene that shows screens for when game is initially loading

Sci Fi Assets: Holds all assets downloaded for sci-fi assets of game (gun effects, etc.)

Scripts: Holds all scripts that determine movement, AI, collision detection/response, all logic-based actions and algorithms, etc.

Textures: Holds all textures used to decorate scenes

TypeOut: Holds all assets and scripts involving font in Intro Scene, including the random letter typeout that happens before actually selecting letters to type out. 
---

##Team Member Roles
Ben Clarke: Scrummaster, documentation, presentation slides, level tester

Ryan Liu: UI, main menu page, game level creation

Adam Sturgeon: Intro scene, character select screen, design document

Drake Addis: Shooting Mechanics, sound effects

Chris Crain: Networking, bug fixing

Greg Sop: AI, code refactoring

Oliver Wu: Character abilities, character animations

---

###Enjoy the game! For more detailed gameplay information, refer to the design document.

--- 

##Resources
Course website: https://web.cse.ohio-state.edu/~crawfis.3/cse786/index.html

Assets used: https://www.assetstore.unity3d.com/en/#!/content/25468

Team website: https://sites.google.com/site/smallroomproductionsllc/

Team repository: https://bitbucket.org/asturg/finalgame
