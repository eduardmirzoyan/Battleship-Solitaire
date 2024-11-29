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
    [SerializeField] private bool isDragging;
    [SerializeField] private GuessState dragType;

    private GameData gameData;

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
    }

    private void Start()
    {
        GameEvents.instance.OnGameStart += Initialize;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= Initialize;
    }

    private void Update()
    {
        HandleHover();
        HandleDrag();
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            // Error check
            if (previousPosition.x < 0 || previousPosition.x >= gridSize || previousPosition.y < 0 || previousPosition.y >= gridSize)
                return;

            // Only if you are hoving a valid type
            GuessState state = gameData.guessGrid[previousPosition.x, previousPosition.y];
            if (state == dragType)
                GameManager.instance.ToggleTile(previousPosition);
        }
    }

    #region Helpers

    private void Initialize(GameData gameData)
    {
        this.gameData = gameData;
        this.gridSize = gameData.gridSize;
    }

    private void HandleDrag()
    {
        // Start drag
        if (Input.GetMouseButtonDown(0))
        {
            // Error check
            if (previousPosition.x < 0 || previousPosition.x >= gridSize || previousPosition.y < 0 || previousPosition.y >= gridSize)
                return;

            isDragging = true;

            // Store start type
            dragType = gameData.guessGrid[previousPosition.x, previousPosition.y];
        }

        // End drag
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Clear start type
            dragType = GuessState.Revealed;
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
