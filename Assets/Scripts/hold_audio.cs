using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hold_audio : MonoBehaviour
{
    public AudioSource audioSource;
    private float time=2;

    // Update is called once per frame
    void Update()
    {
        time-=Time.deltaTime;
        if (time <= 0)
        {
            if (audioSource.time >= 3.8f)
            {
                audioSource.pitch = -1;
            }
            else if(audioSource.time == 0)
            {
                audioSource.pitch = 1;
                audioSource.Play();
            }
        }
    }
}
