using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class turn_to : MonoBehaviour
{
    public int to;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync(to, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
