using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintRenderer : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    public void Initialize(int value)
    {
        text.text = value.ToString();
    }
}
