# Unity Ferris Wheel Project

This is a Unity project featuring a ferris wheel interaction. The project has been optimized and cleaned up for better readability and maintainability.

## Project Overview

The main feature of this project is a ferris wheel that can be interacted with. The ferris wheel has a number of cabins, and the user can interact with them to solve a sequence puzzle.

The project is structured as follows:

- `Assets/Scenes`: Contains the main scenes of the project.
- `Assets/Scripts`: Contains the C# scripts that control the game logic.
- `Assets/Prefab`: Contains the prefabs used in the project.
- `Assets/Sprites`: Contains the sprites used for the background, UI, and game elements.

## Getting Started

To run this project, you will need Unity 2020.3.48f1 or later.

1. Clone this repository to your local machine.
2. Open the project in Unity.
3. Open the main scene located in `Assets/Scenes`.
4. Press the "Play" button in the editor to run the project.

## Main Components

### GameManager

The `GameManager` (`Assets/Scripts/GameManager/GameManager.cs`) is a singleton that manages the main game logic, including camera movement and game state.

### FerrisWheelManager

The `FerrisWheelManager` (`Assets/Scripts/speciali/ruotaPanoramica/FerrisWheelManager.cs`) controls the behavior of the ferris wheel. It handles:

- **Cabin Instantiation**: Creates the cabins and places them on the ferris wheel.
- **Sequence Checking**: Checks if the user has solved the sequence puzzle.
- **Rotation**: Rotates the ferris wheel using DOTween for a smooth animation.
- **Resetting**: Resets the puzzle with a new sequence.

The `FerrisWheelManager` has been refactored to use serialized fields for asset references, so you will need to assign the required prefabs and sprites in the Unity Inspector.

### How to Configure the Ferris Wheel

To configure the ferris wheel, select the `FerrisWheelManager` object in the scene and look at the Inspector. You will find the following properties:

- **Cabine Prefab**: The prefab for the cabins.
- **Ferris Wheel Radius**: The radius of the ferris wheel.
- **Sequences Prefabs**: The prefabs for the different sequences.
- **Sprite Array**: The array of sprites for the cabins.

Make sure to assign all the required assets in the Inspector for the ferris wheel to work correctly.
