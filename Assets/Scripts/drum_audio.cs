using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drum_audio : MonoBehaviour
{
    private float time = 0;
    private void Awake()
    {
        GetComponent<AudioSource>().time = 0.1f;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2) Destroy(gameObject);
    }
}
