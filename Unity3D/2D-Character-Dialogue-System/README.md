# 2D Character Dialogue System

## Video Demos

<b>Dialogue Play Through</b>

<b>Aspect Ratio Fitment</b>

This dialogue experience is tailored to be viewed on mobile devices utilizing <b>aspect ratios 9:21, 9:16, and 3:4</b> but should accomodate other ratios between those listed.

## About

This project is the result of an evaluation as part of a candidate selection process for a Unity Developer role.

<b>** I have replaced all of the original art and dialogue provided to me by the interviewer so as to avoid copyright issues so please excuse my "[programmer art](#programmer-art)". I will update this project to use the original art and dialogue in the event I am granted permission to use their art and dialogue. **</b>

The dialogue system is designed to play through an act. An act is comprised of dialogue steps. In each step actors and their dialogue can be included with property fields to set their UI sprite, position, and size along with dialogue data.<br>
Unity's scriptable objects are utilized to set up an act and all of its dialogue steps in a clean and modular solution allowing for any number of acts or dialogue steps to be assembled. Some dialogue steps can have events tied to them in order to display visual effects or anything else that could be triggered by an event.<br>
The project is completely event driven and responds to mouse clicks on the screen in order to advance through the dialogue.

<b>In the original project step 5 of the act was meant to play a screen flash effect in preparation of a character teleporting in with the next step. Step 5 has been removed from the refactored act as it no longer fit the dialogue, but it is viewable in the "Original Act w Screen Flash" act object.</b>

## Programmer Art

[openart.ai](https://openart.ai/discovery) was utilized to generate a new background image. <br>
All stick figure characters were poorly scribbled and the generated background was lightly polished by me using [paint.net](https://www.getpaint.net/index.html).

## Playing In the Unity Editor

### Scene

Start from the 'Main' scene.

### Flow

Dialogue Manager in the scene is where everything starts. For this scenario it has been provided a direct reference to the Act object that is to be played.<br>
Some of the dialogue steps have events tied to them in order to show the cinematic bars.
<br>
When clicking to advance the dialogue there is a brief cooldown between act steps before more input will cause the next step to play out.

### Restarting the Act:

After the Act is completed by cycling through all the dialogue steps the screen will fade to black, at which point clicking the screen again will cause the Act to restart.
