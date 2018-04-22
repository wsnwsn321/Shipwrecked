using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSViewer: MonoBehaviour {
	public Text fpsText;
	public float deltaTime;

	void Update() {
        float fps = 0;
        if (GameplayManager.State == GameState.Paused)
        {
            fps = 120;
        }
        else
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            fps = 1.0f / deltaTime;
        }

		
		fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();

	}

}


