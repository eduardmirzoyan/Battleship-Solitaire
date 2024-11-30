using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridRenderer : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private Tilemap shipTilemap;
    [SerializeField] private Tilemap hiddenTilemap;
    [SerializeField] private RuleTile shipTile;
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile paddingTile;
    [SerializeField] private Tile hiddenTile;
    [SerializeField] private Tile guessWaterTile;
    [SerializeField] private Tile guessShipTile;
    [SerializeField] private GameObject hintPrefab;
    [SerializeField] private Transform hintsTransform;

    [Header("Settings")]
    [SerializeField] private int paddingSize = 5;

    [Header("Debugging")]
    [SerializeField] private List<HintRenderer> columnHints;
    [SerializeField] private List<HintRenderer> rowHints;

    private void Start()
    {
        GameEvents.instance.OnGameStart += RenderGrids;
        GameEvents.instance.OnGameUpdate += UpdateGrid;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= RenderGrids;
        GameEvents.instance.OnGameUpdate -= UpdateGrid;
    }

    private void RenderGrids(GameData gameData)
    {
        RenderShipGrid(gameData.shipGrid);
        RenderGuessGrid(gameData.guessGrid);
        RenderHints(gameData.shipGrid);
        UpdateGrid(gameData);
    }

    private void UpdateGrid(GameData gameData)
    {
        int n = gameData.GridSize;

        // Update the state of each tile
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                GuessState state = gameData.guessGrid[i, j];
                SetGuessTile(new Vector3Int(i, j), state);
            }
        }

        // Update column hints
        for (int i = 0; i < n; i++)
        {
            int colActualShips = 0;
            int colGuessedShips = 0;

            for (int j = 0; j < n; j++)
            {
                if (gameData.shipGrid[i, j] == TileState.Ship)
                    colActualShips++;

                if (gameData.guessGrid[i, j] == GuessState.Ship || gameData.guessGrid[i, j] == GuessState.Revealed)
                    colGuessedShips++;
            }

            int indicator = colActualShips - colGuessedShips;

            columnHints[i].SetValue(colActualShips);
            columnHints[i].SetState(indicator);
        }

        // Update row hints
        for (int j = 0; j < n; j++)
        {
            int rowActualShips = 0;
            int rowGuessedShips = 0;

            for (int i = 0; i < n; i++)
            {
                if (gameData.shipGrid[i, j] == TileState.Ship)
                    rowActualShips++;

                if (gameData.guessGrid[i, j] == GuessState.Ship || gameData.guessGrid[i, j] == GuessState.Revealed)
                    rowGuessedShips++;
            }


            int indicator = rowActualShips - rowGuessedShips;

            rowHints[j].SetValue(rowActualShips);
            rowHints[j].SetState(indicator);
        }
    }

    #region Helpers

    private void RenderShipGrid(TileState[,] grid)
    {
        waterTilemap.ClearAllTiles();
        shipTilemap.ClearAllTiles();

        int padding = paddingSize;
        int n = grid.GetLength(0);
        for (int i = -padding; i < n + padding; i++)
        {
            for (int j = -padding; j < n + padding; j++)
            {
                // Always set water
                waterTilemap.SetTile(new Vector3Int(i, j), waterTile);

                // Check if out of bounds
                if (i < 0 || i >= n || j < 0 || j >= n)
                    continue;

                // If not ship
                if (grid[i, j] != TileState.Ship)
                    continue;

                shipTilemap.SetTile(new Vector3Int(i, j), shipTile);
            }
        }

        // Center camera on grid
        Vector3 position = new(n / 2f, n / 2f);
        CameraManager.instance.MoveTo(position);
    }

    private void RenderHints(TileState[,] grid)
    {
        int n = grid.GetLength(0);

        columnHints = new();
        for (int i = 0; i < n; i++)
        {
            var worldPos = shipTilemap.GetCellCenterWorld(new Vector3Int(i, n));
            var hint = Instantiate(hintPrefab, worldPos, Quaternion.identity, hintsTransform).GetComponent<HintRenderer>();
            hint.Initialize(i, false);
            columnHints.Add(hint);
        }

        rowHints = new();
        for (int j = 0; j < n; j++)
        {
            var worldPos = shipTilemap.GetCellCenterWorld(new Vector3Int(-1, j));
            var hint = Instantiate(hintPrefab, worldPos, Quaternion.identity, hintsTransform).GetComponent<HintRenderer>();
            hint.Initialize(j, true);
            rowHints.Add(hint);
        }
    }

    private void RenderGuessGrid(GuessState[,] grid)
    {
        // Reset tilemap
        hiddenTilemap.ClearAllTiles();

        // Hide tiles if hidden
        int n = grid.GetLength(0);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                if (grid[i, j] == GuessState.Unknown)
                    hiddenTilemap.SetTile(new Vector3Int(i, j), hiddenTile);
    }

    public void SetGuessTile(Vector3Int position, GuessState state)
    {
        // Get tile based on new state
        Tile tile = state switch
        {
            GuessState.Unknown => hiddenTile,
            GuessState.Water => guessWaterTile,
            GuessState.Ship => guessShipTile,
            _ => null,
        };

        hiddenTilemap.SetTile(position, tile);
    }

    #endregion
}
