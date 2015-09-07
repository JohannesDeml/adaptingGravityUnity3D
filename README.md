# Adapting gravity for Unity3D

## Introduction
Think of games like super mario galaxy. Mario can jump from one planet to another and run on those small spheres without problems. Also in racing games the tracks are often not just on the ground. With this repository I want offer a small library to create exactly such gravity adaption possibilities.

## Technical overview
To get the desired effect all objects that should adapt to gravity check for ground objects via ray-casts. If they find a ground object near them, they save the normal map of the surface and take the negative surface normal as the new gravity direction.
![A screenshot of the unity editor in which a cube is on a larger sphere. From the cube a green line is drawn in the current gravity direction](https://i.imgur.com/YNOR389.jpg)

## Usage
To use the library you have to make changes for every object that should be attracting as well as for every object that should be influenced by gravity. The introduction of adapting gravity can be accomplished by those few steps:

1. Add the AdaptingGravity script to all objects that should attracted by adapting gravity. You can also start by only adding it to your player. You find the script in Deml/Physics/Gravity. If you are starting your project from scratch you can add the prefab player from Deml/Player/Prefabs/Player.
2. Once the script is on the player you can tweak specific settings for it.
  1. The attracting object tags will define which tags should be considered to be attracting to the player.
  2. The gravity strength defines the strength the player will be attracted to objects. The acceleration is set in m/s². (Side note: The gravity strength of the earth would be 9.81 m/s², but in games you would normally use a greater strength)
  3. The gravity check distance defines the distance of the ray that will check of objects that are able to attract the object
  4. The ground check distance defines the distance for which an object will be seen as touching the ground. This could be relevant if you want to only be able to move with the player if it is touching the ground.
![A screenshot of unity3D that shows the parameters that can be changed through the editor](https://i.imgur.com/xwYfJ52.jpg)
3. If you want the player to perfectly rotate to the current gravity direction also add the script LockFallingOver to the player object. It can be found in AdaptingGravity/Scripts/Physics/Manipulation
4. Add one of the tags you defined are valid for attracting objects (You defined them in step 2.1, if you didn't the standard value is only the tag "Ground") to all objects you want to be attracting to other objects.
5. You can where the evaluation of ground objects should happen by adding handles to the object. You will find a button for that in the GravityHandler script.
 ![Add handles by pressing the button. Afterwards you can rotate and transform them the way you are used to](https://i.imgur.com/qOtjMx5.png)
6. With the weigted average toogle you can define if the gravity should be defined by the direction of the nearest object that is hit by a ray or by the average of all objects that are hit.
7. You're set and ready to tweak the settings to your needs.

##Licence
AdaptingGravity is released under the [MIT Licence](https://github.com/JohannesDeml/adaptingGravityUnity3D/blob/master/LICENSE).
