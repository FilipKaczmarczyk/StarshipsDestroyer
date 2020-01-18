using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    public AudioSource audio;
    public AudioClip hover;

    public void HoverSound()
    {
        audio.PlayOneShot(hover);
    }
}
