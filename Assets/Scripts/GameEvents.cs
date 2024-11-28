using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

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

    public event Action<GameData> OnGameStart;
    public event Action<GameData> OnGameUpdate;
    public event Action<GameData> OnGameEnd;

    public void TriggerOnGameStart(GameData gameData) => OnGameStart?.Invoke(gameData);
    public void TriggerOnGameUpdate(GameData gameData) => OnGameUpdate?.Invoke(gameData);
    public void TriggerOnGameEnd(GameData gameData) => OnGameEnd?.Invoke(gameData);
}
