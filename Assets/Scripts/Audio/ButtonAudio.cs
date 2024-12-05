using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button button;

    private const string BUTTON_CLICK = "Click";

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickAudio);
    }

    private void PlayClickAudio()
    {
        AudioManager.instance.PlaySFX(BUTTON_CLICK);
    }
}
