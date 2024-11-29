using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData gameData;

    [Header("Settings")]
    [SerializeField] private int gridSize = 10;
    [SerializeField] private int seed = 0;
    [SerializeField] private int numRevealed = 2;

    public static GameManager instance;

    private void Awake()
    {
        // Singleton Logic
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();

        var shipGrid = GridGenerator.GenerateShipGrid(seed, gridSize);
        var guessGrid = GridGenerator.GenerateHiddenGrid(seed, shipGrid, numRevealed);

        if (shipGrid == null || guessGrid == null)
            yield return null;

        // Create game
        gameData = new GameData(shipGrid, guessGrid);
        GameEvents.instance.TriggerOnGameStart(gameData);
    }

    public void ToggleTile(Vector3Int position)
    {
        // Cache data
        GameData data = gameData;

        // Error check
        if (position.x < 0 || position.x >= data.gridSize || position.y < 0 || position.y >= data.gridSize)
        {
            // print("Selected position out of bounds: " + position);
            return;
        }

        // Get current state
        GuessState currentState = data.guessGrid[position.x, position.y];
        GuessState nextState;

        // Toggle between: hidden, guess-water, guess-ship
        nextState = currentState switch
        {
            GuessState.Unknown => GuessState.Water,
            GuessState.Water => GuessState.Ship,
            GuessState.Ship => GuessState.Unknown,
            _ => currentState,
        };

        // Set next state
        data.guessGrid[position.x, position.y] = nextState;

        // Update visuals
        GameEvents.instance.TriggerOnGameUpdate(data);

        // Check if game is over
        CheckWin(data);
    }

    public void ClearLine(bool isRow, int index)
    {
        // Cache data
        GameData data = gameData;
        int n = data.gridSize;

        if (isRow)
        {
            for (int i = 0; i < n; i++)
                if (data.guessGrid[i, index] == GuessState.Unknown)
                    data.guessGrid[i, index] = GuessState.Water;
        }
        else
        {
            for (int i = 0; i < n; i++)
                if (data.guessGrid[index, i] == GuessState.Unknown)
                    data.guessGrid[index, i] = GuessState.Water;
        }

        // Update visuals
        GameEvents.instance.TriggerOnGameUpdate(data);
    }

    private void CheckWin(GameData data)
    {
        bool hasWon = true;
        int n = data.gridSize;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                TileState actual = data.shipGrid[i, j];
                GuessState guess = data.guessGrid[i, j];

                // If revealed, then skip over
                if (guess == GuessState.Revealed)
                    continue;

                // Make sure ALL states match
                if (actual == TileState.Ship && guess != GuessState.Ship)
                {
                    hasWon = false;
                    break;
                }
                else if (actual == TileState.Water && guess != GuessState.Water)
                {
                    hasWon = false;
                    break;
                }
            }
        }

        if (hasWon)
        {
            print("YOU WIN!");
            GameEvents.instance.TriggerOnGameEnd(data);
        }
    }
}
