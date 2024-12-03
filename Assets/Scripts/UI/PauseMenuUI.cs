using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform labelTransform;
    [SerializeField] private Transform instructionsTransform;
    [SerializeField] private TextMeshProUGUI instructionsLabel;

    [Header("Settings")]
    [SerializeField] private float openPositionY;
    [SerializeField] private float closePositionY;
    [SerializeField] private float transitionDuration;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    [Header("Debug")]
    [SerializeField] private bool isOpen;
    [SerializeField] private bool showInstructions;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleVisibility();
        }
    }

    public void ToggleVisibility()
    {
        if (isOpen)
        {
            LeanTween.moveLocalY(gameObject, closePositionY, transitionDuration).setEase(LeanTweenType.easeInQuad);
            labelTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            LeanTween.moveLocalY(gameObject, openPositionY, transitionDuration).setEase(LeanTweenType.easeInQuad);
            labelTransform.localScale = new Vector3(1, -1, 1);
        }

        isOpen = !isOpen;
    }

    public void ToggleInstructions()
    {
        if (showInstructions)
        {
            LeanTween.scale(instructionsTransform.gameObject, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInQuad);
            instructionsLabel.text = "Show Rules";
        }
        else
        {
            LeanTween.scale(instructionsTransform.gameObject, Vector3.one, transitionDuration).setEase(LeanTweenType.easeOutQuad);
            instructionsLabel.text = "Hide Rules";
        }

        showInstructions = !showInstructions;
    }

    public void Restart()
    {
        // Reload the current scene
        TransitionManager.instance.ReloadScene();
    }

    public void NewGame()
    {
        // Get current level
        LevelData levelData = DataManager.instance.Load();

        // Generate a new random seed
        int newSeed = Random.Range(1, 1000000); // Between 1 - 1M
        LevelData newLevel = new(newSeed, levelData.gridSize, levelData.ships, levelData.numHints);

        // Save new level
        DataManager.instance.Save(newLevel);

        // Restart level
        TransitionManager.instance.ReloadScene();
    }

    public void GoToMainMenu()
    {
        // Go back to main menu scene
        TransitionManager.instance.LoadMainMenuScene();
    }

}
