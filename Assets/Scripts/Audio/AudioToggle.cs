using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioToggle : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Image slashIcon;

    [Header("Settings")]
    [SerializeField] private string source;

    private float defaultVolume;

    private void Start()
    {
        bool found = audioMixer.GetFloat(source, out float volume);
        if (!found) throw new System.Exception($"Parameter '{source}' does not exit!");

        // Save starting volume as default
        defaultVolume = volume;

        // Hide mute
        slashIcon.enabled = false;
    }

    public void Toggle()
    {
        bool found = audioMixer.GetFloat(source, out float volume);
        if (!found) throw new System.Exception($"Parameter '{source}' does not exit!");

        // Toggle current state
        if (volume > -80f) // Not mute
        {
            audioMixer.SetFloat(source, -80f);
            slashIcon.enabled = true;
        }
        else
        {
            audioMixer.SetFloat(source, defaultVolume);
            slashIcon.enabled = false;
        }
    }
}
