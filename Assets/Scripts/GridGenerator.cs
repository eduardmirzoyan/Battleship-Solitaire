using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator
{
    private const int MAX_ATTEMPTS = 1000;

    public static int[,] GenerateShipGrid(int seed, int size, int[] ships)
    {
        // Use random seed if input seed is 0
        System.Random rng = seed == 0 ? new() : new(seed);

        int[,] grid = new int[size, size]; // Innit grid to 0s
        List<Vector2Int> validLocations = new();
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                validLocations.Add(new Vector2Int(i, j));

        // Generate board
        foreach (int ship in ships)
        {
            bool faceHorizontal = Convert.ToBoolean(rng.Next(2));
            bool success = RandomShip(rng, grid, validLocations, ship, faceHorizontal);
            if (!success)
            {
                Debug.Log("Ran out of tries for random ship");
                return null;
            }
        }

        return grid;
    }

    public static int[,] GenerateHiddenGrid(int seed, int[,] shipGrid, int numRevealed)
    {
        // Use random seed if input seed is 0
        System.Random rng = seed == 0 ? new() : new(seed);

        // Set size based on ship grid
        int n = shipGrid.GetLength(0);

        // 0 - hidden, 1 - revealed
        int[,] grid = new int[n, n]; // Init to 0's

        // Find all ship tiles
        List<Vector2Int> shipLocations = new();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                if (shipGrid[i, j] == 1)
                    shipLocations.Add(new Vector2Int(i, j));

        // Reveal random ship tiles
        while (numRevealed > 0 && shipLocations.Count > 0)
        {
            // Get random ship
            int index = rng.Next(shipLocations.Count);
            Vector2Int shipLocation = shipLocations[index];

            // Reveal tile
            grid[shipLocation.x, shipLocation.y] = 1;

            // Remove from possble locations
            shipLocations.Remove(shipLocation);
            numRevealed--;
        }

        return grid;
    }

    private static bool RandomShip(System.Random rng, int[,] grid, List<Vector2Int> validLocations, int shipSize, bool faceHorizontal)
    {
        for (int a = 0; a < MAX_ATTEMPTS; a++)
        {
            // If we are out of valid locations, then stop
            if (validLocations.Count == 0)
                break;

            // Get random valid location
            int index = rng.Next(validLocations.Count);
            var location = validLocations[index];

            // Get ship locations
            var locations = GetShipLocations(location, shipSize, faceHorizontal);

            // Validate location
            bool valid = true;
            foreach (var loc in locations)
            {
                // If not valid location
                if (!validLocations.Contains(loc))
                {
                    valid = false;
                    break;
                }
            }
            if (!valid) // If invalid ship, start over!
                continue;

            // Place ship and remove invalid spots
            foreach (var loc in locations)
            {
                // Set grid
                grid[loc.x, loc.y] = 1;

                // Remove adjacents (if exist)
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        // Add offset
                        Vector2Int newLoc = loc + new Vector2Int(i, j);

                        // Remove location
                        validLocations.Remove(newLoc);
                    }
                }
            }

            // Sucessfully placed a random ship!
            return true;
        }

        Debug.Log("Ran out of tries for random ship");
        return false;
    }

    private static List<Vector2Int> GetShipLocations(Vector2Int location, int shipSize, bool faceHorizontal)
    {
        List<Vector2Int> locations = new();

        Vector2Int current = location;
        for (int i = 0; i < shipSize; i++)
        {
            locations.Add(current);

            if (faceHorizontal)
                current += Vector2Int.right;
            else
                current += Vector2Int.up;
        }

        return locations;
    }
}
