using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipMenuUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI seedLabel;
    [SerializeField] private TextMeshProUGUI[] shipCountLabels;
    [SerializeField] private GameObject[] shipCountObjects;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f;

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
    }

    public void Open()
    {
        // Fade in UI
        StopAllCoroutines();
        StartCoroutine(Fade(0f, 1f, fadeDuration));
    }

    public void Close()
    {
        // Fade out UI
        StopAllCoroutines();
        StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator Fade(float start, float end, float duration)
    {
        canvasGroup.alpha = start;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}
