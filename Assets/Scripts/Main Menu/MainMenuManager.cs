using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform mainMenuTransform;
    [SerializeField] private Transform levelSelectTransform;
    [SerializeField] private Transform backgroundMapTransform;
    [SerializeField] private Transform instructionsTransform;
    [SerializeField] private TextMeshProUGUI seedText;

    [Header("Settings")]
    [SerializeField] private float transitionPositionX = -1500f;
    [SerializeField] private float transitionTime = 3f;
    [SerializeField] private float hoverStrength = 10f;
    [SerializeField] private float hoverSpeed = 2;

    [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeOutQuad;

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

        // Start hovering the various windwos in the scene
        StartWindowHovers();
    }

    private void StartWindowHovers()
    {
        // Start bobbing motion
        LeanTween.moveLocalY(mainMenuTransform.gameObject, hoverStrength, hoverSpeed).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
        LeanTween.moveLocalY(instructionsTransform.gameObject, hoverStrength, hoverSpeed).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
        LeanTween.moveLocalY(levelSelectTransform.gameObject, hoverStrength, hoverSpeed).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }

    public void GoToLevelSelect()
    {
        // Move rightward
        LeanTween.moveLocalX(mainMenuTransform.gameObject, transitionPositionX, transitionTime).setEase(tweenType);
        LeanTween.moveLocalX(levelSelectTransform.gameObject, 0, transitionTime).setEase(tweenType);
        LeanTween.moveX(backgroundMapTransform.gameObject, transitionPositionX / 50f, transitionTime).setEase(tweenType);
    }

    public void GoToMainMenu()
    {
        // Move leftward
        LeanTween.moveLocalX(mainMenuTransform.gameObject, 0, transitionTime).setEase(tweenType);
        LeanTween.moveLocalX(levelSelectTransform.gameObject, -transitionPositionX, transitionTime).setEase(tweenType);
        LeanTween.moveX(backgroundMapTransform.gameObject, 0, transitionTime).setEase(tweenType);
    }

    public void StartLevel(int index)
    {
        // Ditch last terminator character
        string str = seedText.text[..^1];

        bool success = int.TryParse(str, out int seed);
        if (!success)
            seed = Random.Range(1, 1000000); // Random seed between 1 - 1M

        LevelData levelData = index switch
        {
            1 => new(seed, 6, new int[] { 3, 2, 2, 1, 1, 1 }, 3), // Easy
            2 => new(seed, 8, new int[] { 3, 3, 2, 2, 2, 1, 1, 1 }, 5), // Medium
            3 => new(seed, 10, new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 }, 7), // Hard
            _ => new(seed, 1, new int[] { 1, 1, 1 }, 1), // Default
        };

        // Save level
        DataManager.instance.Save(levelData);

        // Close the scene
        StartCoroutine(CloseScene());
    }

    private IEnumerator CloseScene()
    {
        // Fade rightward
        LeanTween.moveLocalX(levelSelectTransform.gameObject, 2 * transitionPositionX, 3 * transitionTime).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveX(backgroundMapTransform.gameObject, 2 * (transitionPositionX / 50f), 3 * transitionTime).setEase(LeanTweenType.easeInOutBack);

        yield return new WaitForSeconds(3 * transitionTime / 2f);

        // Change scene
        TransitionManager.instance.LoadNextScene();
    }

    public void OpenIntructions()
    {
        // Move leftward
        LeanTween.moveLocalX(mainMenuTransform.gameObject, -transitionPositionX, transitionTime).setEase(tweenType);
        LeanTween.moveLocalX(instructionsTransform.gameObject, 0, transitionTime).setEase(tweenType);
        LeanTween.moveX(backgroundMapTransform.gameObject, -transitionPositionX / 50f, transitionTime).setEase(tweenType);
    }

    public void CloseIntructions()
    {
        // Move rightward
        LeanTween.moveLocalX(mainMenuTransform.gameObject, 0, transitionTime).setEase(tweenType);
        LeanTween.moveLocalX(instructionsTransform.gameObject, transitionPositionX, transitionTime).setEase(tweenType);
        LeanTween.moveX(backgroundMapTransform.gameObject, 0, transitionTime).setEase(tweenType);
    }

    public void QuitGame()
    {
        // Close application
        Application.Quit();
    }
}
