

# RoadShooter
**Haifa CS Projects** 

[slack link](https://haifacsprojects.slack.com)

## Development Credits

This game was developed with the assistance of **Dr. Roei Poranne** , who provided valuable guidance and expertise throughout the development process.


## Game Overview

"Road Shooter" is an exhilarating hyper-casual driving game that puts players in control of a car with no brakes. The objective   is to navigate through challenging levels and reach the final destination without colliding with walls or obstacles. The game offers intense action and quick reflexes as players strive to survive the relentless journey.
![start ](https://github.com/Zoubizaher/RoadShooter/blob/main/screen%20shots/start.png)
![levels](https://github.com/Zoubizaher/RoadShooter/blob/main/screen%20shots/multiple%20levels.png)
![GamePlay](https://github.com/Zoubizaher/RoadShooter/blob/main/screen%20shots/game%20play.png)

## Game Mechanics

-   **Top-Down Perspective**: The game features a top-down camera perspective, providing a clear view of the car's surroundings. The car automatically moves forward at a constant speed throughout the game.

-   **Lane Shifting**: The player can move the car left or right to switch between different road paths, avoiding obstacles and aiming for power-ups.
-   **Shooting Mechanism**: The car is equipped with a powerful gun mounted on the roof. Players can fire bullets to destroy walls and shoot down obstacles that descend from the sky.
-   **Power-Ups**: Power-ups, such as shields, can be collected along the road. These shields provide temporary invincibility to walls and other obstacles, offering players a valuable advantage.

## Game Features

-   **Multiple Levels**: The game offers a variety of levels, each with a unique destination to reach. As players progress, the levels become increasingly challenging, featuring harder walls, traps, and the possibility of cliffs where the player's car can fall from.
-   **Endless Gameplay**: Players strive to survive for as long as possible without crashing into walls or falling down cliffs. The game ends when the player's car collides with a wall, falls down a cliff, or successfully reaches the end of the level.
-   **Engaging Visual Design**: The game's visuals are created using the Unity game engine. The player's car is designed to resemble a classic vehicle, utilizing basic primitives within Unity. A cubic gun is mounted on the car's roof, adding a unique and distinctive look. Walls and obstacles are represented by cubes and spheres, while the road is constructed using the Unity Terrain System.

## Work journal: 
### Update 1 : 
Create the Basic mechanic of the car movement and jump method using the wheel collider, which is built in unity.
create the car physics component - the rigid body component that gives the object a physical movement around the environment that is affected by gravity.
create the camera to follow a script to follow that follows the car in the x, and y-axis offset from the car and stays on the same z-axis.
create a basic terrain and make a layer called "Ground" to make the car know to recognize where is the ground surface to avoid the car to unlimited jump in the air by using Raycast

### Update 2 : 
limit the car rotation to 45 and -45 in y-axis rotation Euler angles to make the car keep moving forward across the road.
auto-rotate the car toward the direction of the road if not pressing anything or rich the boundaries on the sides.
add points lights to the car, points lights, and directional lights to the environment for better visual.
using particle system components to emit particles from the exhaust in relation to the speed of the car.
using the Trail Renderer component to see where the car is crossing.
add boundaries by clamping the z rotation of the car to avoid the player get out from the road.

### Update 3 : 
add obstacles to the road, I created a wall script that detects when the car collides with a wall. This is done by utilizing the OnTriggerEnter event, which is triggered when the car has the tag "Player". When the event is triggered, a method is executed to destroy the wall. This is achieved by using a dynamic Rigidbody component that detects the "MeshRenderer" components within the wall object and adds a Rigidbody to any mesh contained within the object, causing the wall to explode.  
Also, in the last record i sent to you , the car doesn"t have a gun component , now i added the machine gun on the car ( he didn"t work yet ) .  
Additionally, I added post-processing effects to enhance the visual scene. Moreover, a Particle System component was included to visualize the dead zone area.

### Update 4 :
I create a gun shooting mechanic script, using Transform.translate to move the gun in the desired direction.  
The shooting action should be triggered by pressing the "F" key.  
Instantiate a bullet every 0.5 seconds. To visualize the bullet's trajectory, add a Trail Renderer component to it.  
When the car enters the Dead zone or colide with the walls activate the onTriggerEnter event to destroy the car.

### Update 5 :
First, I set the mechanic of the camera to top down perspective .  
Also , I Created a shooter that fires balls from the air by using the Rigidbody component. The shooter detects the player using Physics.OverlapSphere and the layer associated with the player. It aims directly at the player with a random ratio between the minimum and maximum values.  
In Addition , Create a shield box that activates a shield on the car when the car collects the box using the OnTriggerEnter event to detect the player. When the shield is activated, the boolean value inside the car script changes. The shield remains active for a random duration defined within the shieldBox script, and it is deactivated using the Invoke built-in method in Unity. Set the shield within the obstacles script to protect the player from any obstacles in the scene.

### Last update :
Final Touches on the UI: Implement the main menu script to incorporate the methods into the UI buttons. Additionally, create two more levels within the scene, each featuring different roads and crossings. Moreover, enhance the walls by introducing a health system, requiring more than one bullet to destroy them.

## Development Details

-   **Game Engine**: Unity
-   **Programming Language**: C#

Enjoy the thrilling experience of "Road Shooter" as you test your reflexes and navigate through perilous roads. Challenge yourself to survive and complete each level, mastering the art of precision driving and precise shooting. Good luck!







