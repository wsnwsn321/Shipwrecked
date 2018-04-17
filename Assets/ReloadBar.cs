using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour {

    private Image reloadBarSprite;
    private bool startReload = false;
    private float endTimeStamp;
    private float reloadCooldown;

	// Use this for initialization
	void Start () {
        reloadBarSprite = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (startReload == true)
        {
            reloadBarSprite.fillAmount = (endTimeStamp - Time.time) / reloadCooldown;
            if (reloadBarSprite.fillAmount < 0.01)
            {
                startReload = false;
                setInactive();
            }
        }
	}


    public void startReloadBar(float reloadCooldown, float endTimeStamp)
    {
        this.reloadCooldown = reloadCooldown;
        this.endTimeStamp = endTimeStamp;
        startReload = true;
    }

    public void setInactive()
    {
        gameObject.SetActive(false);
    }

    public void setActive()
    {
        gameObject.SetActive(true);
    }
}
