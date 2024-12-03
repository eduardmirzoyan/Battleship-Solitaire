using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipMenuUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform gameWinTranform;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI seedLabel;
    [SerializeField] private TextMeshProUGUI[] shipCountLabels;
    [SerializeField] private GameObject[] shipCountObjects;

    [Header("Settings")]
    [SerializeField] private float homeXPosition = 660f;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private float winTransitionDuration = 1f;

    private void Start()
    {
        GameEvents.instance.OnGameStart += SetupShips;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= SetupShips;
    }

    private void SetupShips(GameData gameData)
    {
        int maxShipSize = shipCountLabels.Length;
        var ships = gameData.levelData.ships;
        int[] counts = new int[maxShipSize];

        foreach (var ship in ships)
            counts[ship - 1] += 1;

        for (int i = 0; i < maxShipSize; i++)
        {
            if (counts[i] == 0) shipCountObjects[i].SetActive(false);
            else shipCountLabels[i].text = $"x{counts[i]}";
        }

        seedLabel.text = $"Seed: {gameData.levelData.seed}";

        LeanTween.moveLocalX(rectTransform.gameObject, homeXPosition, transitionDuration).setEase(LeanTweenType.easeInOutBack);
    }
}
