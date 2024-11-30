using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public int seed;
    public int gridSize;
    public int[] ships;
    public int numHints;

    public LevelData(int seed, int gridSize, int[] ships, int numHints)
    {
        this.seed = seed;
        this.gridSize = gridSize;
        this.ships = ships;
        this.numHints = numHints;
    }
}

public class DataManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private LevelData levelData;

    public static DataManager instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);

        // Create basic data
        CreateDefaultData();
    }

    private void CreateDefaultData()
    {
        levelData = new LevelData(0, 6, new int[] { 3, 2, 2, 1, 1, 1 }, 3); // Easy
    }

    public void Save(LevelData levelData) => this.levelData = levelData;

    public LevelData Load() => levelData;
}
