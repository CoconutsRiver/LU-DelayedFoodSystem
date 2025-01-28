# LU-DelayedFoodSystem
Delayed food system input mod for Littleington University game 

## Overview

`DelayedFoodSystem` is a BepInEx plugin for Littleington University that modifies the game's food and drink system to apply changes over time instead of instantly. The durations for these changes are configurable via a JSON file.

## Requirements

- [BepInEx 5.x](https://github.com/BepInEx/BepInEx)
- Last release of Littleington University
- .NET Framework 4.6 or higher.

## Installation

1. **Install BepInEx**:
   - Download and install BepInEx for your game.
   - Extract the BepInEx files into your game directory.


2. **Download the Plugin**:
   - [Download the latest release] (https://github.com/CoconutsRiver/LU-DelayedFoodSystem/releases/tag/Release) of `DelayedFoodSystem.dll`.

3. **Place the Plugin**:
   - Copy `DelayedFoodSystem.dll` to the `BepInEx/plugins` folder in your game directory.

4. **Create Configuration File**:
   - Create or download the release file `DelayedFoodSystemConfig.json` file in the `BepInEx/plugins` folder.
   - Use the following structure for the file:

     ```json
     {
         "foodDuration": 15.0,
         "drinkDuration": 10.0
     }
     ```

   - If the file does not exist, it will be created with default values the first time the game runs with the plugin.
  
  
4. **Launch the Game**:
   - The plugin will automatically load when the game starts.
  - The food and drink changes will now be applied over the specified durations instead of instantly.
  - **foodDuration**: Duration in seconds for food changes to apply.
  - **drinkDuration**: Duration in seconds for drink changes to apply.

   You can modify these values in the `DelayedFoodSystemConfig.json` file to suit your preferences.
   
---

## Development Setup

Don't hesitate to make a pull request or open an issue if you want to contribute.



