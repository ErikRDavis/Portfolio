# Minesweeper

Compatible with Unity v2018.3.8f1 and greater

## About

The game is mostly event driven with the exception of a single Update loop in the UIController that watches for the 'esc' menu toggle input.\
Canvas and UI elements are implemented in a way so the game will fit on screen with any resolution and aspect ratio.

## [Play The Demo](https://erikrdavis.github.io/Portfolio/Unity3D/Demos/Minesweeper/)

## Playing In Editor

-   Start from scene 'Main'
-   The game plays similarly to the original Minesweeper game.

## Controls

-   Left-click to explore a grid space
-   Right-click to mark a grid space with a flag (F)
-   Press the 'esc' key to toggle the menu and expose an exit game button

## Game Config

There is a scriptable object game config with two properties to set the grid size and the number of mines.\
Multiple game config objects could be created for varying difficulty of gameplay.\
The in-scene Game Manager has a reference to the game config object.
