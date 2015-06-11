# adaptingGravityUnity3D
A small library for gravity that adapts to the ground normal the player is standing on

## Introduction
Think of games like super mario galaxy. Mario can jump from one planat to another and run on those small spheres without problems. Also in racing games the tracks are often not just on the ground. With this repository I want offer a small library to create exactly such gravtity adaption possibilites.

## Technical overview
To get the desired effect all objects that should adapt to gravity check for ground objects via raycasts. If they find a ground object near them, they save the normal map of the surface and take the negative surface normal as the new gravity direction.
![A screenshot of the unity editor in which a cube is on a larger sphere. From the cube a green line is drawn in the current gravity direction](https://i.imgur.com/DpHS4cG.jpg)