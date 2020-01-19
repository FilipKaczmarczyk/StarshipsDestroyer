using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    public PostProcessVolume volume;

    public Gradient gradient;

    public float strobeDuration = 2f;

    private Bloom bloom;

    void Start()
    {
        volume.profile.TryGetSettings(out bloom);

        bloom.color.value = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
        bloom.color.value = gradient.Evaluate(t);
        // Color.Lerp(Color.green, Color.red, Mathf.PingPong(Time.time, 1));
    }
}
