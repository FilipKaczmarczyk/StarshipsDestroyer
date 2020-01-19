using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    public AudioSource audio;
    public AudioClip hover;
    public AudioClip click;

    public void HoverSound()
    {
        audio.PlayOneShot(hover);
    }

    public void ClickSound()
    {
        audio.PlayOneShot(click);
    }
}
