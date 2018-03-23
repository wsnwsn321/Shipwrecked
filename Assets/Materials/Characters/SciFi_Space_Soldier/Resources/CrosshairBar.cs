using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBar : MonoBehaviour {
    public Texture2D crosshairImage;
    public int size;
    public float maxAngle;
    public float minAngle;
    float lookHeight;
	// Use this for initialization
	public void LookHeight(float value)
    {
        lookHeight += value;
        if (lookHeight > maxAngle || lookHeight < minAngle)
        {
            lookHeight -= value;
        }
    }

    void OnGUI()
    {
        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //screenPosition.y = Screen.height - screenPosition.y;
        //GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y-lookHeight, size, size), crosshairImage);
        //float xMin = (Screen.width / 2f) - (crosshairImage.width / 2.1f);
        //float yMin = (Screen.height / 2.1f) - (crosshairImage.height / 2);
        //GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }
}
