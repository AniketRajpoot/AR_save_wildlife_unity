# AR_save_wildlife_unity
An unity project to spread awareness about diminishing wildlife through an interactive augmented reality game!
APK file can be found [here](https://drive.google.com/file/d/1YoB6u2dzLUM0f3ctNi-6xdLA5fkzA_8o/view?usp=sharing). 

## Overview
This was a project for Hack Your Reality hackathon on hackerearth. It's build on top of [GoogleARcore](https://developers.google.com/ar) library using [unity](https://unity.com/) engine. 

## Objective
To find a way to make people aware of the current grievous situation of the wildlife animals and help them know various ways through which they can contribute on an individual level and try to improve the current situation with the help of an interactive AR(Augmented Reality) game for various wildlife protection organizations like [WWF](https://www.worldwildlife.org/).

## Motivation
1) Make an interactive AR Game, in which people can make their own wildlfie environment. 
2) Through this game people can be made aware about the problem and also educate them on how they can individually help on any level to eradicate this problem. 
3) There will be tasks which will be given to the users that they will have to complete every week to get extra points in the game. (This will be regularly updated through database, which can be done easily.) 
4) Donations can be done to the various wildlife organisations through the app.

## Implementation
GoogleARcore provides a prefab that manages ARcore session and holds the First person camera. Planes were detected using Plane Discovery prefab and then raycast (from the phone camera) was use to manipulate placing and manipulating gameobjects. The game is itself a gameobject on top of which other objects are placed. 
We also designed a basic user-interface through which all the actions on animals i.e. gameobjects were performed. 

## Source Code

 - [ScenceController.cs](https://github.com/AniketRajpoot/AR_save_wildlife_unity/blob/master/AR_Save_Wildlife_Base/Assets/Scripts/ScenceController.cs "ScenceController.cs") : module for the main ARcore session, contains all the functions for interactions of user and GameObjects. 
 - [Hunger.cs](https://github.com/AniketRajpoot/AR_save_wildlife_unity/blob/master/AR_Save_Wildlife_Base/Assets/Scripts/Hunger.cs "Hunger.cs") : submodule for database reference and properties of animals, how they change or update with user interaction. 

## Acknowledgements

This project is built on top of [GoogleARcore](https://developers.google.com/ar) using [Unity](https://unity.com/). 
And was  part of [Hack Your Reality](https://hackyourreality.hackerearth.com/) hackathon on hackerearth.
Thanks to [Arjun](https://github.com/arjunsinghrathore) for his collabration. 
