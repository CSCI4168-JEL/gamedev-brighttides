# gamedev-brighttides

### Project Description
Final project for Game Development class at Dalhousie University worked on by Lee Lunn, Elliot Darbyshire, and Jonathan Ignacio. Bright Tides is a strategical, turn-based, roguelike game developed using the Unity engine.

### Gameplay Overview
The player controls a ship placed on a 3D tile grid and must navigate to the end of the region which is marked by a red flag. As the player progresses through the region, the levels will become increasingly challenging due to the presence of more powerful enemies and the scarcity of resources. The player can perform different actions per turn indicated by the limit on the UI.

#### Resources
- **Gold** - Important resource for a pirate. Dropped by defeated enemies and contained in treasure chests. Can be used to purchase ammo/ship repairs and upgrades to ship stats.
- **Ammo** - Limited resource for attacking enemies. Contained in treasure chests. Required to make a ranged attack against enemy ships.
- **Health** - Ship's durability resource. Enemies will attack the player, reducing the player's health. If health reaches 0, the ship is destroyed and the game ends.

#### Player Controls
- **Attack** - Uses one action. consume ammo to perform a ranged attack against an enemy. Deals damage to the target based on the player's attack damage stat.
- **Move** - Uses one action. Move the player from their current tile to any adjacent open tile.
- **End Turn** - Allow the enemies to take their turn, then refresh player actions.
