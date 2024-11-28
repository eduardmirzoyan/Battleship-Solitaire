using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState { Water, Ship }
public enum GuessState { Unknown, Revealed, Water, Ship }

[System.Serializable]
public class GameData
{
    public TileState[,] shipGrid;
    public GuessState[,] guessGrid;
    public int gridSize;

    public GameData(int[,] shipGrid, int[,] guessGrid)
    {
        int n = shipGrid.GetLength(0);

        // Init ship grid
        this.shipGrid = new TileState[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                this.shipGrid[i, j] = shipGrid[i, j] == 1 ? TileState.Ship : TileState.Water;

        // Init guess grid
        this.guessGrid = new GuessState[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                this.guessGrid[i, j] = guessGrid[i, j] == 1 ? GuessState.Revealed : GuessState.Unknown;

        gridSize = n;
    }
}
