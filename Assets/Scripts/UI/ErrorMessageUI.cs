using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessageUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float delayDuration = 1f;
    [SerializeField] private float transitionDuration = 1f;

    private void Start()
    {
        GameEvents.instance.OnGameStart += Show;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnGameStart -= Show;
    }

    private void Show(GameData gameData)
    {
        // If invalid game was made, then display
        if (gameData == null)
        {
            StartCoroutine(DelayedShow(delayDuration));
        }
    }

    private IEnumerator DelayedShow(float delay)
    {
        yield return new WaitForSeconds(delay);

        LeanTween.scale(gameObject, Vector3.one, transitionDuration).setEase(LeanTweenType.easeInQuad);
    }
}
