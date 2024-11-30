using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintRenderer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshPro text;

    [Header("Settings")]
    [SerializeField] private float offset;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color clearedColor;
    [SerializeField] private Color overflowColor;
    [SerializeField] private float endGameTransitionDuration = 1f;

    [Header("Debug")]
    [SerializeField] private int index;
    [SerializeField] private bool isRow;

    public bool IsCleared
    {
        get
        {
            return text.color == clearedColor;
        }
    }

    private bool isSelected;

    public void Initialize(int index, bool isRow)
    {
        this.index = index;
        this.isRow = isRow;

        // Give some space
        Vector3 vectorOffset = isRow ? Vector3.left * offset : Vector3.up * offset;
        transform.position += vectorOffset;
    }

    public void Uninitialize()
    {
        // Fade away
        StopAllCoroutines();
        StartCoroutine(FadeOverTime(endGameTransitionDuration));
    }

    private IEnumerator FadeOverTime(float duration)
    {
        Color startColor = text.color;
        Color endColor = Color.clear;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Fade color
            text.color = Color.Lerp(startColor, endColor, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        text.color = endColor;
        Destroy(gameObject);
    }

    public void SetValue(int value)
    {
        text.text = value.ToString();
    }

    public void SetState(int indicator)
    {
        Color color;

        if (indicator > 0) color = defaultColor;
        else if (indicator < 0) color = overflowColor;
        else color = clearedColor;

        text.color = color;
    }

    private void OnMouseEnter()
    {
        if (IsCleared)
        {
            text.fontStyle = FontStyles.Bold;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.75f);
            isSelected = true;
        }
    }

    private void OnMouseExit()
    {
        text.fontStyle = FontStyles.Normal;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        isSelected = false;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && isSelected)
        {
            GameManager.instance.ClearLine(isRow, index);
        }
    }
}
