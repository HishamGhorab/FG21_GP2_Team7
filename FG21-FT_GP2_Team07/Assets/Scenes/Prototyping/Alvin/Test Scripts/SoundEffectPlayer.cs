using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource soundsPlayer;

    public void OnClick()
    {
        soundsPlayer.Play();
    }
}
