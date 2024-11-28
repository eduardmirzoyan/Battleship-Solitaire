using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Tilemap selectTilemap;
    [SerializeField] private Tile selectTile;

    [Header("Debug")]
    [SerializeField] private Vector3Int previousPosition;
    [SerializeField] private int gridSize;

    public static InputHandler instance;

    private void Awake()
    {
        // Singleton Logic
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        GameEvents.instance.OnGameStart += Initialize;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= Initialize;
    }

    private void Update()
    {
        HandleHover();
        HandleClick();
    }

    #region Helpers

    private void Initialize(GameData gameData)
    {
        this.gridSize = gameData.gridSize;
    }

    private void HandleClick()
    {
        // On left click
        if (Input.GetMouseButtonDown(0))
        {
            // Toggle state of tile
            GameManager.instance.ToggleTile(previousPosition);
        }
    }

    private void HandleHover()
    {
        // Get current mouse position in world-space
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int currentPosition = selectTilemap.WorldToCell(mousePosition);

        // If same position don't do anything
        if (currentPosition == previousPosition)
            return;

        // Un-select previous position
        selectTilemap.SetTile(previousPosition, null);

        // Check bounds
        if (currentPosition.x < 0 || currentPosition.x >= gridSize || currentPosition.y < 0 || currentPosition.y >= gridSize)
        {
            // Do nothing
        }
        else
        {
            // Select current position
            selectTilemap.SetTile(currentPosition, selectTile);
        }

        // Update previous
        previousPosition = currentPosition;
    }

    #endregion
}
