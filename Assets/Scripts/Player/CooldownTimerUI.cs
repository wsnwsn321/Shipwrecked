using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTimerUI {

    private Image skillImage1;
    private Image skillImage2;
    public GameObject skillCooldownGO1;
    public GameObject skillCooldownGO2;
    private Image skillCooldownImage1;
    private Image skillCooldownImage2;
    private bool startCooldown1;
    private bool startCooldown2;

    public CooldownTimerUI(Image skillImage1, Image skillImage2)
    {
        this.skillImage1 = skillImage1;
        this.skillImage2 = skillImage2;
    }

    /**
     * Starts the cooldown animation for the UI. 
     * 
     * @param skillNumber
     *                      Specifies which skill is the skill requiring a cooldown animation
     **/
    public void startCooldownTimerUI(int skillNumber)
    {
        switch (skillNumber)
        {
            case 1:              
                skillCooldownGO1.SetActive(true);
                skillCooldownImage1 = skillCooldownGO1.GetComponent<Image>();
                skillCooldownImage1.sprite = GameObject.FindGameObjectWithTag("Skill1").GetComponent<Image>().sprite;
                skillCooldownImage1.fillAmount = 1;
                startCooldown1 = true;
                break;
            case 2:               
                skillCooldownGO2.SetActive(true);
                skillCooldownImage2 = skillCooldownGO2.GetComponent<Image>();
                skillCooldownImage2.sprite = GameObject.FindGameObjectWithTag("Skill2").GetComponent<Image>().sprite;
                skillCooldownImage2.fillAmount = 1;
                startCooldown2 = true;
                break;
            default:
                Debug.Log("Error: No skill number is assigned for this value yet.");
                break;
        }
    }

    public void CooldownStart()
    {
        skillCooldownGO1 = GameObject.Find("Skill1 Cooldown");
        skillCooldownGO2 = GameObject.Find("Skill2 Cooldown");
        skillCooldownGO1.SetActive(false);
        skillCooldownGO2.SetActive(false);

        startCooldown1 = false;
        startCooldown2 = false;
    }

    public void CooldownUpdate(float cooldown1, float cooldown2, float timeStamp1, float timeStamp2)
    {
        if (startCooldown1)
        {
            if (cooldown1 == 0)
            {
                skillCooldownImage1.fillAmount = 0;
            }
            else
            {
                float percentage = (timeStamp1 - Time.time) / cooldown1;
                skillCooldownImage1.fillAmount = percentage;
                if (skillCooldownImage1.fillAmount <= 0.01)
                {
                    startCooldown1 = false;
                }
            }
        }   
        if (startCooldown2)
        {
            if (cooldown2 == 0)
            {
                skillCooldownImage1.fillAmount = 0;
            }
            else
            {
                float percentage = (timeStamp2 - Time.time) / cooldown2;
                skillCooldownImage2.fillAmount = percentage;
                if (skillCooldownImage2.fillAmount <= 0.01)
                {
                    startCooldown2 = false;
                }
            }
        }
    }
}
