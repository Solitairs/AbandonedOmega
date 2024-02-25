using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartConstroller : MonoBehaviour
{
    // Update is called once per frame
    private float time = 0;
    private bool ed = false;
    void FixedUpdate()
    {
        if (GetComponent<VideoPlayer>().time>=3)
        {
            time+=Time.deltaTime;
            if (time > 0.6f)
            {
                if (!ed)
                {
                    AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(7);
                    ed = true;
                }
            }
        }

    }
}
