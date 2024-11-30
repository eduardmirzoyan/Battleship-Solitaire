using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintRenderer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshPro text;

    [Header("Settings")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color clearedColor;
    [SerializeField] private Color overflowColor;

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
