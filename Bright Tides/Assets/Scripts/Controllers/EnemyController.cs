using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController {

    public List<Entity> enemies; // The list of all enemies in the region
    private readonly Tile[,] tileMap;

    private bool isTakingTurn = false; // If the enemies are performing their turn

    // Class for the A* pathfinding
    private class PathDistanceValue : IComparable<PathDistanceValue>{
        public float pathCost; // The total distance from the starting node to this node
        public float specificDistance; // The total distance to get from the starting node to the goal by passing this node

        public PathDistanceValue() {
            // Initially cost is infinite
            pathCost = float.MaxValue;
            specificDistance = float.MaxValue;
        }

        // Compares two entries by the specific distance
        public int CompareTo(PathDistanceValue other) {
            return specificDistance.CompareTo(other.specificDistance);
        }
    }

    public EnemyController(Tile[,] tileMap) {
        this.tileMap = tileMap;
        enemies = new List<Entity>();
    }

    // Add the enemy to the list of enemies in the region
    public void RegisterEnemyEntity(Entity enemy) {
        enemies.Add(enemy);
    }

    // The public method to call when the enemies should take their turn
    public void PerformEnemyTurn() {
        Entity player = GameManager.instance.playerInstance.GetComponent<Entity>();

        if (player == null) {
            Debug.LogError("No player entity found! Enemies cannot take a turn!");
            isTakingTurn = false;
        }

        if (!isTakingTurn) {
            isTakingTurn = true; // Start the turn
            foreach (Entity enemy in enemies) {
                Tile enemyTile = enemy.transform.GetComponentInParent<Tile>();
                Tile playerTile = player.transform.GetComponentInParent<Tile>();
                if (enemyTile && playerTile) {
                    float distanceFromPlayer = Tile.CalculateChessboardDistance(enemyTile, playerTile);
                    if (enemy.attributes.baseAttackRange >= Math.Floor(Vector3.Distance(enemy.transform.position, player.transform.position))) { // Enemy is within firing range of the player
                        Debug.Log(enemy.attributes.captainName + " is firing upon " + player.attributes.captainName);
                    } else {
                        List<Tile> moves = AStarPathfinding(playerTile, enemyTile); // Find the best path to the player. Calculating the path from the player to the enemy prevents loops in movement
                        if (moves != null) { // If the algorithm found a path
                            enemy.MoveToTileBlocking(moves[1], enemy.attributes.movementSpeed); // 0 is the enemy's tile, and 1 is the next step
                        }
                    }
                }
            }
            isTakingTurn = false;
        }
    }

    // A* pathfinding algorithm adapted from Dr. Heywood's lectures and from the wikipedia entry for A*
    private List<Tile> AStarPathfinding(Tile origin, Tile target) {
        // Initialize data structures
        List<Tile> visitedTiles = new List<Tile>();
        List<Tile> unvisitedTiles = new List<Tile>();
        Dictionary<Tile, Tile> connectedTiles = new Dictionary<Tile, Tile>(); // A collection of tiles and their connected most efficient neighbor on the path

        Dictionary<Tile, PathDistanceValue> nodes = new Dictionary<Tile, PathDistanceValue>();
        foreach (Tile tile in tileMap) {
            nodes.Add(tile, new PathDistanceValue()); // Create an entry for every tile in the map
        }

        PathDistanceValue currentNodePDV = new PathDistanceValue {
            pathCost = 0, // The cost from start to start is 0
            specificDistance = HeuristicCostEstimate(origin, target) // The cost from the starting tile to the goal is just the heuristic
        };
        nodes[origin] = currentNodePDV; // Assign the updated entry in the dictionary
        unvisitedTiles.Add(origin); // Add the starting tile to the list

        while (unvisitedTiles.Count > 0) {
            // Using LINQ aggregation method
            Tile current = unvisitedTiles.Aggregate((min, curr) => nodes[min].CompareTo(nodes[curr]) < 0 ? min : curr); // Compare all entries for each Tile in the open set and return the Tile with the lowest specific distance
            if (current == target) {
                return TraceBestPath(connectedTiles, current); // Goal reached, retrace the path
            }

            unvisitedTiles.Remove(current); // We are exploring the tile now, so remove it
            visitedTiles.Add(current); // Add the tile to the visited tiles

            foreach (Tile neighbour in current.neighbours) { // Get the neighbours of the tile
                if (visitedTiles.Contains(neighbour) || !neighbour.TileProperties.IsPathableByEnemy) {
                    continue; // If the neighbour has already been visited or it cannot be pathed through, ignore it
                }

                // Path cost is the distance between the current node and its neighbour (should be 1, assume diagonal is the same movement cost)
                float pendingPathCost = nodes[current].pathCost + 1f;

                if (!unvisitedTiles.Contains(neighbour)) {
                    unvisitedTiles.Add(neighbour); // Since the tile has not yet been visited, add it to the list
                } else if (pendingPathCost >= nodes[current].pathCost) {
                    continue; // The cost of the new tile is not an improvement, so skip it
                }

                connectedTiles[neighbour] = current; // The best path (so far) to this neighbour from the previous tile has been found
                PathDistanceValue neighbourPDV = new PathDistanceValue {
                    pathCost = pendingPathCost, // The path cost for this tile
                    specificDistance = pendingPathCost + HeuristicCostEstimate(neighbour, target) // Total cost with heuristic estimate
                };
                nodes[neighbour] = currentNodePDV; // Assign the updated entry in the dictionary
            }
        }

        return null; // No path could be found
    }

    private List<Tile> TraceBestPath(Dictionary<Tile, Tile> connectedTiles, Tile current) {
        List<Tile> bestPath = new List<Tile> {
            current // Add the goal to the path
        };

        while (connectedTiles.Keys.Contains(current)) {
            current = connectedTiles[current]; // Get the best neighbour of current
            bestPath.Add(current);
        }
        return bestPath;
    }

    // Distance heuristic using chessboard distance
    private float HeuristicCostEstimate(Tile origin, Tile destination) {
        return Tile.CalculateChessboardDistance(origin, destination);
    }
}


