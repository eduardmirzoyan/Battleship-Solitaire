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
}
