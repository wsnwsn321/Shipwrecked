using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillImages : MonoBehaviour {

    public Sprite sargeSkillOne;
    public Sprite sargeSkillTwo;
    public Sprite doctorSkillOne;
    public Sprite doctorSkillTwo;
    public Sprite mechanicSkillOne;
    public Sprite mechanicSkillTwo;
    public Sprite captainSkillOne;
    public Sprite captainSkillTwo;
    public Sprite genericSkillSprite;

    // Use this for initialization
    void Start () {
        Image skillImage = transform.GetComponent<Image>();
        if (transform.gameObject.tag == "Skill1")
        {
            switch (transform.root.tag)
            {
                case "Sarge":
                    skillImage.sprite = sargeSkillOne;
                    break;
                case "Doctor":
                    skillImage.sprite = doctorSkillOne;
                    break;
                case "Mechanic":
                    skillImage.sprite = mechanicSkillOne;
                    break;
                case "Captain":
                    skillImage.sprite = captainSkillOne;
                    break;
            }
        } else if (transform.gameObject.tag == "Skill2")
        {
            switch (transform.root.tag)
            {
                case "Sarge":
                    skillImage.sprite = sargeSkillTwo;
                    break;
                case "Doctor":
                    skillImage.sprite = doctorSkillTwo;
                    break;
                case "Mechanic":
                    skillImage.sprite = mechanicSkillTwo;
                    break;
                case "Captain":
                    skillImage.sprite = captainSkillTwo;
                    break;
            }
        }
    }  

    // Update is called once per frame
    void Update () {
		
	}
}
