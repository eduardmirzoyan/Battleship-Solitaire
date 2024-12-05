using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform mapTransform;

    [Header("Data")]
    [SerializeField] private GameData gameData;

    [Header("Settings")]
    [SerializeField] private float transitionTime = 3f;
    [SerializeField] private float mapCenterXOffset = 0f;

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
        // Start game waiting a frame
        StartCoroutine(SetupGame());
    }

    private IEnumerator SetupGame()
    {
        // Open scene
        TransitionManager.instance.OpenScene();

        yield return new WaitForEndOfFrame();

        // Get data from manager
        LevelData levelData = DataManager.instance.Load();

        var shipGrid = GridGenerator.GenerateShipGrid(levelData.seed, levelData.gridSize, levelData.ships);
        var guessGrid = GridGenerator.GenerateHiddenGrid(levelData.seed, shipGrid, levelData.numHints);

        if (shipGrid == null || guessGrid == null)
            yield return null;

        // Create game
        gameData = new GameData(levelData, shipGrid, guessGrid);
        GameEvents.instance.TriggerOnGameStart(gameData);

        // Pan screen to board
        LeanTween.moveX(mapTransform.gameObject, mapCenterXOffset, transitionTime).setEase(LeanTweenType.easeInOutBack);
    }

    public void ToggleTile(Vector3Int position)
    {
        // Cache data
        GameData data = gameData;

        // Error check
        if (position.x < 0 || position.x >= data.GridSize || position.y < 0 || position.y >= data.GridSize)
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

        // Play sfx
        AudioManager.instance.PlaySFX("Toggle");

        // Update visuals
        GameEvents.instance.TriggerOnGameUpdate(data);

        // Check if game is over
        CheckWin(data);
    }

    public void ClearLine(bool isRow, int index)
    {
        // Cache data
        GameData data = gameData;
        int n = data.GridSize;

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

        // Play sfx
        AudioManager.instance.PlaySFX("Clear Line");

        // Update visuals
        GameEvents.instance.TriggerOnGameUpdate(data);

        // Check for wins
        CheckWin(data);
    }

    #region Helpers

    private void CheckWin(GameData data)
    {
        bool hasWon = true;
        int n = data.GridSize;

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
            // Play sfx
            AudioManager.instance.PlaySFX("Game Win");

            // Update visuals
            GameEvents.instance.TriggerOnGameEnd(data);
        }

    }

    #endregion
}
