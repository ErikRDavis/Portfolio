# Dice Tower

Built in Unity v2021.3.23f1

## About

This is just a small and fun project that prototypes a dice tower.<br>
When input is received five "standard" D6 dice are sent tumbling down through the tower that then reports the total value of all the dice once they come to a stop.<br>
Additionally I personally created the D6 die using Blender as an additional learning experience.

## Playing In Editor

-   Start from scene 'Main'

### <b>Playing</b>

-   To roll the dice just click/tap the game view. <br>
-   Clicking the screen again after the dice have been evaluated will reset them at the top of the tower.

#### <b>UI</b>

There is a very simple UI, in the top left of the screen, provided in order to display the total value of the dice after a roll.

## Controls

-   Just click/tap the game view screen.

## Utilized Elements

-   Physics
-   Triggers
-   Vector3 maths
-   UI
-   delgates/events
-   3D asset creation (D6 die)

## Optimizations

-   The dice and tower materials are GPU instanced
-   Lighting has been baked
-   Completely event driven
