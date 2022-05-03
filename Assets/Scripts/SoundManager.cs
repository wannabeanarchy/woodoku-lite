using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource soundClick;

    private void Start()
    {
        GameLogic.onSoundClick += OnSoundClick; 
    }

    private void OnDestroy()
    {
        GameLogic.onSoundClick -= OnSoundClick;
    }

    private void OnSoundClick()
    {
        soundClick.Play();
    }
}
