using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    private void Awake()
    {
        GameObject[] backgroundMusicObjects = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if (backgroundMusicObjects.Length > 1)
        {
            Destroy(backgroundMusicObjects[1]);
        }
        DontDestroyOnLoad(backgroundMusicObjects[0]);
    }
}
