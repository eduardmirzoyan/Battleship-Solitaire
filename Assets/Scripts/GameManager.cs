using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GridRenderer gridRenderer;

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
        var shipGrid = GridGenerator.GenerateShipGrid(seed, gridSize);
        var hiddenGrid = GridGenerator.GenerateHiddenGrid(seed, shipGrid, numRevealed);

        if (shipGrid == null)
            return;

        gridRenderer.RenderShipGrid(shipGrid);
        gridRenderer.RenderHints(shipGrid);
        gridRenderer.RenderHiddenGrid(hiddenGrid);
        InputHandler.instance.Initialize(gridSize);
    }

    public void ToggleTile()
    {
        // Toggle between: hidden, guess-water, guess-ship
    }
}
