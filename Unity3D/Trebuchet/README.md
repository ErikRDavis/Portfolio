# Trebuchet

Built in Unity v2021.3.23f1

## About

This project is physics-based.\
The trebuchet is powered by gravity pulling on the counter weight as one would be in reality, accelerating its munition with centripetal force. Rotating bodies of the trebuchet are connected with configurable joints and the munition is connected to the sling/rope with a fixed joint and is released when the direction of its velocity, relative to the forward direction of the trebuchet, is at an angle approximate to the desired release angle. To ensure the munition actually leaves the trebuchet at the desired angle there is a one time velocity correction performed.

## Playing In Editor

-   Start from scene 'Main'

### <b>Playing</b>

The trebuchet will be automatically armed with a munition before and after each shot.\
A few simple target structures have been provided as targets for you to demolish with the trebuchet.\
There is no goal or win condition beyond just playing with the trebuchet and demolishing the structures.

#### <b>Aiming & Shot Range</b>

-   The trebuchet can be rotated left/right to adjust the aim
-   The munition can be released at an angle you choose from -45 to 90 degrees to adjust the flight range

#### <b>Changing the Camera View</b>

-   The camera can be rotated around whatever it is currently focusing on
-   You can change what the camera focuses on between the trebuchet, the target area, and any active munition

#### <b>UI</b>

UI is present to provide feedback as to what the current munition release angle is set to.\
Additionally there is a menu that provides the player with the control maps and a button to exit the game.

## Controls

### Supported Inputs

-   Gamepad
-   Keyboard & Mouse

Control inputs can be viewed in game by opening the menu with the 'esc' key on a keyboard or the 'start' button on a gamepad.

## Utilized Elements

-   Physics
-   UI
-   Unity's new Input System
-   Scriptable objects

## Optimizations

-   All 3D meshes use the same "universal" material
    -   The material is marked as instanced to reduce batching
-   Object pooling
    -   Munition projectiles

## Lessons Learned

-   While mesh combining may be ideal for some or most scenarios, using a trebuchet constructed of combined meshes increased the rendering batch count compared to the prototype trebuchet constructed of a number of simple cubes.
    -   The reason being the cubes making up the prototype trebuchet could all be GPU instanced with all the other blocks (cubes) in the scene while the combined meshes would require separate passes.
    -   Where a benefit may be seen to use the combined mesh trebuchet would be if there were many trebuchets; all of the combined meshes of the trebuchets could then be GPU instanced together.
