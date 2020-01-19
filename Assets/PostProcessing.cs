using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    public PostProcessVolume volume;

    private Bloom bloom;

    void Start()
    {
        volume.profile.TryGetSettings(out bloom);

        bloom.color.value = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        bloom.color.value = Color.Lerp(Color.green, Color.red, Mathf.PingPong(Time.time, 1));
    }
}
