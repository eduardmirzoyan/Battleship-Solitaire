using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image toggleMenuIcon;
    [SerializeField] private Image toggleInstructionsIcon;
    [SerializeField] private Transform instructionsTransform;
    [SerializeField] private TextMeshProUGUI instructionsLabel;

    [Header("Settings")]
    [SerializeField] private float openPositionX;
    [SerializeField] private float closePositionX;
    [SerializeField] private float transitionDuration;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private Color activeColor;

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
            LeanTween.moveLocalX(gameObject, closePositionX, transitionDuration).setEase(LeanTweenType.easeInQuad);
            toggleMenuIcon.color = Color.white;
        }
        else
        {
            LeanTween.moveLocalX(gameObject, openPositionX, transitionDuration).setEase(LeanTweenType.easeInQuad);
            toggleMenuIcon.color = activeColor;
        }

        isOpen = !isOpen;
    }

    public void ToggleInstructions()
    {
        if (showInstructions)
        {
            LeanTween.scale(instructionsTransform.gameObject, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInQuad);
            instructionsLabel.text = "Show Rules";
            toggleInstructionsIcon.color = Color.white;
        }
        else
        {
            LeanTween.scale(instructionsTransform.gameObject, Vector3.one, transitionDuration).setEase(LeanTweenType.easeOutQuad);
            instructionsLabel.text = "Hide Rules";
            toggleInstructionsIcon.color = activeColor;
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
