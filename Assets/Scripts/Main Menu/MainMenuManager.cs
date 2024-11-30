using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform mainMenuTransform;
    [SerializeField] private Transform levelSelectTransform;
    [SerializeField] private Transform backgroundMapTransform;
    [SerializeField] private TextMeshProUGUI seedText;

    [Header("Settings")]
    [SerializeField] private float transitionPositionX = -1500f;
    [SerializeField] private float transitionTime = 3f;

    public static MainMenuManager instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // Open scene
        TransitionManager.instance.OpenScene();
    }

    public void GoToLevelSelect()
    {
        LeanTween.moveLocalX(mainMenuTransform.gameObject, transitionPositionX, transitionTime).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveLocalX(levelSelectTransform.gameObject, 0, transitionTime).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveX(backgroundMapTransform.gameObject, transitionPositionX / 50f, transitionTime).setEase(LeanTweenType.easeInOutBack);
    }

    public void GoToMainMenu()
    {
        LeanTween.moveLocalX(mainMenuTransform.gameObject, 0, transitionTime).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveLocalX(levelSelectTransform.gameObject, -transitionPositionX, transitionTime).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveX(backgroundMapTransform.gameObject, 0, transitionTime).setEase(LeanTweenType.easeInOutBack);
    }

    public void StartLevel(int index)
    {


        // Read seed data
        string seedStr = seedText.text;
        print("Seed=" + seedText.text + "|");
        int seed = 0; // Convert.ToInt32(seedStr); // int.TryParse("0", out seed) ? seed : 0;
        print("Int Seed=" + seed);

        int defaultHints = 3;

        var levelData = index switch
        {
            1 => new LevelData(seed, 6, new int[] { 3, 2, 2, 1, 1, 1 }, defaultHints), // Easy
            2 => new LevelData(seed, 8, new int[] { 3, 3, 2, 2, 2, 1, 1, 1 }, defaultHints), // Medium
            3 => new LevelData(seed, 10, new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 }, defaultHints), // Hard
            _ => new LevelData(seed, 1, new int[] { 1, 1, 1 }, defaultHints), // Default
        };

        // Save level
        DataManager.instance.Save(levelData);

        // Close the scene
        StartCoroutine(CloseScene());
    }

    private IEnumerator CloseScene()
    {
        // Fade out
        LeanTween.moveLocalX(levelSelectTransform.gameObject, 2 * transitionPositionX, transitionTime).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveX(backgroundMapTransform.gameObject, 2 * (transitionPositionX / 50f), transitionTime).setEase(LeanTweenType.easeInOutBack);

        yield return new WaitForSeconds(transitionTime / 2f);

        // Change scene
        TransitionManager.instance.LoadNextScene();
    }

    public void HowToPlay()
    {
        // Debug
        print("Open Help Screen");
    }

    public void OpenSettings()
    {
        // Debug
        print("Open Settings");

        // Open settings
        // SettingsManager.instance.Open();
    }

    public void QuitGame()
    {
        // Close application
        Application.Quit();
    }
}
