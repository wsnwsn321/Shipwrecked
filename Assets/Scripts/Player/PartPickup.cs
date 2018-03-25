using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PartPickup : NewPickupItem {

    public Sprite shipPartImage;
    private static int slot = 0;

	public override void OnPickup (Transform item)
	{
        switch (slot)
        {
            case 0:
                Image slotImage = GameObject.Find("Slot0").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 1:
                slotImage = GameObject.Find("Slot1").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 2:
                slotImage = GameObject.Find("Slot2").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 3:
                slotImage = GameObject.Find("Slot3").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
            case 4:
                slotImage = GameObject.Find("Slot4").GetComponent<Image>();
                slotImage.sprite = shipPartImage;
                break;
        }
        slot++;
        if (slot >= 5)
        {
			SceneManager.LoadScene("RegularCredits");
        }
	}
}
