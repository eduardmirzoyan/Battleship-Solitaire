using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridRenderer : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] private Tilemap shipTilemap;
    [SerializeField] private Tilemap hiddenTilemap;
    [SerializeField] private Tile shipTile;
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile paddingTile;
    [SerializeField] private Tile hiddenTile;
    [SerializeField] private GameObject hintPrefab;

    [Header("Settings")]
    [SerializeField] private int paddingSize = 5;

    public void RenderShipGrid(int[,] grid)
    {
        shipTilemap.ClearAllTiles();

        int padding = paddingSize;
        int n = grid.GetLength(0);
        for (int i = -padding; i < n + padding; i++)
        {
            for (int j = -padding; j < n + padding; j++)
            {
                // Check if out of bounds
                if (i < 0 || i >= n || j < 0 || j >= n)
                {
                    shipTilemap.SetTile(new Vector3Int(i, j), paddingTile);
                }
                else
                {
                    if (grid[i, j] == 1)
                        shipTilemap.SetTile(new Vector3Int(i, j), shipTile);
                    else
                        shipTilemap.SetTile(new Vector3Int(i, j), waterTile);
                }
            }
        }

        // Center camera on grid
        Vector3 position = new(n / 2f, n / 2f);
        CameraManager.instance.MoveTo(position);
    }

    public void RenderHints(int[,] grid)
    {
        int n = grid.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            int colSum = 0;
            for (int j = 0; j < n; j++)
                if (grid[i, j] == 1)
                    colSum++;

            var worldPos = shipTilemap.GetCellCenterWorld(new Vector3Int(i, n));
            var hint = Instantiate(hintPrefab, worldPos, Quaternion.identity, transform).GetComponent<HintRenderer>();
            hint.Initialize(colSum);
        }

        for (int j = 0; j < n; j++)
        {
            int rowSum = 0;
            for (int i = 0; i < n; i++)
                if (grid[i, j] == 1)
                    rowSum++;

            var worldPos = shipTilemap.GetCellCenterWorld(new Vector3Int(-1, j));
            var hint = Instantiate(hintPrefab, worldPos, Quaternion.identity, transform).GetComponent<HintRenderer>();
            hint.Initialize(rowSum);
        }
    }

    public void RenderHiddenGrid(int[,] grid)
    {
        // Reset tilemap
        hiddenTilemap.ClearAllTiles();

        // Hide tiles if hidden
        int n = grid.GetLength(0);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                if (grid[i, j] == 0)
                    hiddenTilemap.SetTile(new Vector3Int(i, j), hiddenTile);
    }
}
