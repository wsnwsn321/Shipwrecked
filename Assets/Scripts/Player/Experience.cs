using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelThresholds
{
    public static List<int> Thresholds = new List<int> { 20, 40, 80, 100, 150, 200, 250, 300, 500 };
}

[RequireComponent(typeof(Control))]
public class Experience : MonoBehaviour {

    CoreControl core;
    TeammateTypes characterType;
    public int experience;
    public int level;
    int nextLevelThreshold;
    int nextThresholdPosition;
    private Text levelText;
	public static int currentSlot = 0;
    // Use this for initialization
    void Start () {
        core = GetComponent<CoreControl>();
        characterType = GetComponent<EntityType>().teammateType;
        experience = 0;
        level = 1;
        nextLevelThreshold = LevelThresholds.Thresholds[0];
        nextThresholdPosition = 1;
        Text[] textArr = GetComponentsInChildren<Text>();
        foreach (Text text in textArr)
        {
            if (text.name.Equals("LevelText"))
            {
                levelText = text;
            }
        }
    }

    public void IncreaseBy(int amount)
    {
        if (level < LevelThresholds.Thresholds.Count + 1)
        {
            experience += amount;

            while (experience >= nextLevelThreshold && level < LevelThresholds.Thresholds.Count + 1)
            {
                level++;
                levelText.text = "Player Level: " + level.ToString();
                //level up func
                levelUpAnimation();
                if (nextThresholdPosition < LevelThresholds.Thresholds.Count)
                {
                    nextLevelThreshold = LevelThresholds.Thresholds[nextThresholdPosition];
                    nextThresholdPosition++;
                }
                UpdateSkills();
            }
        }
    }

    private void levelUpAnimation()
    {
        core.LevelUp();
    }

    private void UpdateSkills()
    {
        switch(characterType)
        {
            case TeammateTypes.Captain:
                UpdateCaptainSkills();
                break;
            case TeammateTypes.Doctor:
                UpdateDoctorSkills();
                break;
            case TeammateTypes.Engineer:
                UpdateEngineerSkills();
                break;
            case TeammateTypes.Sergeant:
                UpdateSergeantSkills();
                break;
        }

        // Damage increases at levels 3, 5, 7, and 9.
        // Get special ability at level 5.
        switch(level)
        {
            case 3:
                break;
            case 5:
                core.hasSpecialAbility = true;
                break;
            case 7:
                break;
            case 9:
                break;
        }

		switch(currentSlot)
		{
		case 1:
			core.damageModifier = 0.95f;
			break;
		case 2:
			core.hasSpecialAbility = true;
			core.damageModifier = 0.9f;
			break;
		case 3:
			core.damageModifier = 0.85f;
			break;
		case 4:
			core.damageModifier =0.8f;
			break;
		}


    }

    private void UpdateCaptainSkills()
    {
        CaptainControl control = GetComponent<CaptainControl>();

        switch(level)
        {
            case 2:
                control.attackBuff = control.defenseBuff = 2f;
                break;
            case 3:
                break;
            case 4:
                control.attackBuff = control.defenseBuff = 3f;
                break;
            case 5:
                break;
            case 6:
                control.attackBuff = control.defenseBuff = 4f;
                break;
            case 7:
                break;
            case 8:
                control.attackBuff = control.defenseBuff = 5f;
                break;
            case 9:
                break;
            case 10:
                break;
        }
    }

    private void UpdateDoctorSkills()
    {
        DoctorControl control = GetComponent<DoctorControl>();

        switch (level)
        {
            case 2:

                break;
            case 3:
                control.maxPills++;
                break;
            case 4:

                break;
            case 5:
                control.maxPills++;
                break;
            case 6:
                control.healthPerSec = 4f;
                break;
            case 7:
                control.maxPills++;
                break;
            case 8:
                control.healthPerSec = 5f;
                break;
            case 9:
                control.researchBuff = 2.5f;
                control.maxPills++;
                break;
            case 10:
                break;
        }
    }

    private void UpdateEngineerSkills()
    {
        MechanicControl control = GetComponent<MechanicControl>();

        switch (level)
        {
            case 2:
                control.turretDamage = 3f;
                control.turretHealth = 40f;
                break;
            case 3:
                break;
            case 4:
                control.currentTurretBuildLevel++;
                control.turretHealth = 60f;
                control.turretDamage = 4f;
                break;
            case 5:
                break;
            case 6:
                control.currentTurretBuildLevel++;
                break;
            case 7:
                break;
            case 8:
                control.turretHealth = 80f;
                break;
            case 9:
                break;
            case 10:
                control.currentTurretBuildLevel++;
                control.turretHealth = 100f;
                control.turretDamage = 5f;
                break;
        }
    }

    private void UpdateSergeantSkills()
    {
        SergeantControl control = GetComponent<SergeantControl>();

        switch (level)
        {
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
        }
    }
}
