using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.UI;

public class MechanicControl : Photon.MonoBehaviour, IClassControl {

    public float buildPositionOffset = 3;
    public float buildingPieceTime = 1.2f;
    public Material highlightMaterial;
    public GameObject[] turrets;
    public GameObject[] turretGhosts;
    public Material[] turretMaterials;
    public Sprite mechanicSkillOne;
    public Sprite mechanicSkillTwo;
    public Sprite genericSkillSprite;

    [HideInInspector]
    public int currentTurretBuildLevel = 0;
    [HideInInspector]
    public float turretHealth = 20f;
    [HideInInspector]
    public float turretDamage = 2f;
	public float hpPerSec;
	public float totalHp;
	public bool isReparing;

	private bool isBuilding;
    private Vector3 buildPosition;
    private float currentBuildingTime = 0;
    private int currentBuildingIteration = 0;
    private List<Transform> currentBuildingChildren;
    private GameObject currentPlaceableObject;
    private GameObject currentTurret;
    private GameObject highlightedTurret;
    private Material previousTurretMaterial;
    private Dictionary<GameObject, UnfinishedBuilding> unfinishedBuildings;
    private Animator ani;
	private GameObject spaceship;
	private ShipHealth shp;
	private float distanceWithSpace;

    private List<GameObject> builtTurrets;
    [HideInInspector]
    public int maxTurrets;

    [HideInInspector]
    public bool canBuild = true;
    private bool canUseAbility = true;
	private bool canRepair;
    public float buildTurretCooldown = 20f;
	public float repairSpaceShipCooldown = 10f;
    private CooldownTimerUI timer;
    public float skillTimeStamp1;
    public float skillTimeStamp2;

    class UnfinishedBuilding
    {
        int currentIteration;
        List<Transform> currentChildren;

        public UnfinishedBuilding(int currentIteration, List<Transform> currentChildren)
        {
            this.currentIteration = currentIteration;
            this.currentChildren = currentChildren;
        }

        public int CurrentIteration
        {
            get
            {
                return currentIteration;
            }
        }

        public List<Transform> CurrentChildren
        {
            get
            {
                return currentChildren;
            }
        }
    }

    void Start () {

		hpPerSec = 0.1025f;
		totalHp = 50;
        maxTurrets = 1;
        isBuilding = false;
		isReparing = false;
		canRepair = true;
        currentBuildingChildren = new List<Transform>();
        unfinishedBuildings = new Dictionary<GameObject, UnfinishedBuilding>();
        builtTurrets = new List<GameObject>();
        ani = GetComponent<Animator>();
		spaceship = GameObject.Find("SpaceshipZone");
		shp = spaceship.GetComponent<ShipHealth> ();
    }

    void Update()
    {
			if (timer == null) {
				timer = new CooldownTimerUI (GameObject.FindGameObjectWithTag ("Skill1").GetComponent<Image> (), GameObject.FindGameObjectWithTag ("Skill2").GetComponent<Image> ());
				timer.CooldownStart ();
			}
			if (canBuild == false && Time.time >= skillTimeStamp1) {
				canBuild = true;
			}
			timer.CooldownUpdate (buildTurretCooldown, repairSpaceShipCooldown, skillTimeStamp1, skillTimeStamp2);
			if (spaceship != null) {
				distanceWithSpace = Vector3.Distance (transform.position, spaceship.transform.position);
			} else {
				spaceship = GameObject.Find ("SpaceshipZone");
			}
    }
    
    List<Transform> ActivateChildren(Transform t)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in t)
        {
            child.gameObject.SetActive(true);

            children.Add(child);

            // Stop at the prefab's "Gun" level.
            if (child.name.Equals("Gun") || child.name.Equals("RotatingGuns"))
            {
                child.gameObject.SetActive(true);
                isBuilding = false;
                currentTurret.GetComponent<TurretBehaviors>().isFinished = true;
            }

            if (child.name.Equals("Turret"))
            {
                currentTurret.GetComponent<TurretBehaviors>().enabled = true;
            }
        }

        return children;
    }

    void ActivateDescendents(Transform t)
    {
        foreach (Transform child in t)
        {
            child.gameObject.SetActive(true);
            ActivateDescendents(child);
        }
    }

    public void AddTurretToBuildList(GameObject turret)
    {
        if (!builtTurrets.Contains(turret))
        {
            builtTurrets.Add(turret);
        }
    }

    public void RemoveTurretFromBuiltList(GameObject turret)
    {
        if (builtTurrets.Contains(turret))
        {
            builtTurrets.Remove(turret);
        }
    }

    public bool IsAtMaxTurrets()
    {
        return builtTurrets.Count == maxTurrets;
    }

    void BuildTurret()
    {
        GameObject.Find("Skill1").GetComponent<Image>().sprite = mechanicSkillOne;
        GameObject.Find("Skill2").GetComponent<Image>().sprite = mechanicSkillTwo;

        if (currentPlaceableObject && currentPlaceableObject.activeSelf)
        {
            if (currentPlaceableObject.GetComponent<Placement>().canBeBuilt)
            {
                BuildNewTurret();
            }
        }
		else if (highlightedTurret)
        {
            buildPosition = transform.localPosition + transform.forward * buildPositionOffset;
            Collider[] nearbyTurrets = Physics.OverlapSphere(buildPosition, 0.5f, LayerMask.GetMask("TurretParent"), QueryTriggerInteraction.Collide);
            if (nearbyTurrets.Length > 0)
            {
                if (nearbyTurrets[0].GetComponent<TurretBehaviors>().isFinished)
                {
                    UpgradeTurret(nearbyTurrets[0].transform);
                }
                else
                {
                    ContinueBuildingNearestTurret(nearbyTurrets);
                }

                Placement.SetMaterial(highlightedTurret.transform, previousTurretMaterial);
                highlightedTurret = null;
                previousTurretMaterial = null;
            }
        }
    }

    void UpgradeTurret(Transform turret)
    {
        isBuilding = true;
        currentBuildingIteration = 1;

        Destroy(currentPlaceableObject);
        currentPlaceableObject = null;

        TurretBehaviors oldStats = turret.GetComponent<TurretBehaviors>();
        if (oldStats.turretLevel < currentTurretBuildLevel)
        {
            GameObject newTurret;
            if (PhotonNetwork.connected)
            {
                newTurret = PhotonNetwork.Instantiate(turrets[currentTurretBuildLevel].name, turret.position, Quaternion.identity, 0);
            }
            else
            {
                newTurret = Instantiate(turrets[currentTurretBuildLevel], turret.position, Quaternion.identity);
            }

            ActivateDescendents(newTurret.transform);

            TurretBehaviors newStats = newTurret.GetComponent<TurretBehaviors>();
            newStats.enabled = true;
            newStats.isFinished = true;
            newStats.engineer = gameObject;
            newStats.damage = turretDamage;
            newStats.health = turretHealth;
            newStats.turretLevel = currentTurretBuildLevel;
            newStats.SetRotations(oldStats.GetRotations());

            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Destroy(turret.GetComponent<PhotonView>());
            }
            else
            {
                Destroy(turret.gameObject);
            }
        }
        else
        {
            oldStats.damage = turretDamage;
            oldStats.health = turretHealth;
        }
    }

    void BuildNewTurret()
    {
        isBuilding = true;
        Destroy(currentPlaceableObject);
        currentPlaceableObject = null;

        if (PhotonNetwork.connected)
        {
            currentTurret = PhotonNetwork.Instantiate(turrets[currentTurretBuildLevel].name, buildPosition, Quaternion.identity, 0);
        }
        else
        {
            currentTurret = Instantiate(turrets[currentTurretBuildLevel], buildPosition, Quaternion.identity);
        }

        AddTurretToBuildList(currentTurret);

        TurretBehaviors turretStats = currentTurret.GetComponent<TurretBehaviors>();
        turretStats.engineer = gameObject;
        turretStats.damage = turretDamage;
        turretStats.health = turretHealth;
        turretStats.turretLevel = currentTurretBuildLevel;
    }

    void ContinueBuildingNearestTurret(Collider[] nearbyTurrets)
    {
        GameObject nearestTurret = nearbyTurrets[0].gameObject;
        float nearestDistance = Vector3.Distance(transform.position, nearbyTurrets[0].transform.position);
        foreach (Collider turret in nearbyTurrets)
        {
            float distance = Vector3.Distance(transform.position, turret.transform.position);
            if (distance < nearestDistance)
            {
                nearestTurret = turret.gameObject;
            }
        }

        if (unfinishedBuildings.ContainsKey(nearestTurret))
        {
            isBuilding = true;
            currentTurret = nearestTurret;
            currentBuildingIteration = unfinishedBuildings[currentTurret].CurrentIteration;
            currentBuildingChildren = unfinishedBuildings[currentTurret].CurrentChildren;
            unfinishedBuildings.Remove(currentTurret);
        }
    }

    void CreateTurretGhost()
    {
        currentPlaceableObject = Instantiate(turretGhosts[currentTurretBuildLevel], buildPosition, Quaternion.identity);

        currentPlaceableObject.GetComponent<Placement>().builderCoreControl = gameObject.GetComponent<CoreControl>();
    }

    void ExitBuildingMode()
    {
        if (currentPlaceableObject || isBuilding)
        {
            GameObject.Find("Skill1").GetComponent<Image>().sprite = mechanicSkillOne;
            GameObject.Find("Skill2").GetComponent<Image>().sprite = mechanicSkillTwo;
        }

        if (currentPlaceableObject)
        {
            Destroy(currentPlaceableObject);
            currentPlaceableObject = null;
        }

        if (isBuilding)
        {
            // Make sure that at least something is built before stopping, otherwise there can be
            // invisible, unfinished turrets.
            if (currentBuildingIteration > 0)
            {
                unfinishedBuildings.Add(currentTurret, new UnfinishedBuilding(currentBuildingIteration, currentBuildingChildren));
                ResetBuildingVariables();
            }
        }

        if (highlightedTurret)
        {
            Placement.SetMaterial(highlightedTurret.transform, previousTurretMaterial);
            highlightedTurret = null;
            previousTurretMaterial = null;
        }
		isBuilding = false;
    }

    void ResetBuildingVariables()
    {
        isBuilding = false;
        currentBuildingIteration = 0;
        currentBuildingChildren = new List<Transform>();
        currentBuildingTime = 0;
    }

    void UpdatePlaceableLocation()
    {
        buildPosition = transform.localPosition + transform.forward * buildPositionOffset;
        currentPlaceableObject.transform.position = buildPosition;
    }

	void RepairShip(){
		if (ani && !ani.GetCurrentAnimatorStateInfo (0).IsName ("Die")&&canRepair) {
			if (!ani.GetCurrentAnimatorStateInfo (0).IsName ("AB2")&&distanceWithSpace < 9.5f) {
				canRepair = false;
				ani.SetTrigger ("Repairing");
				shp.isReparing = true;
                // Start cooldown animation for UI skill image
              
                StartCoroutine (RepairTimer ());

			}
		}

	}

	IEnumerator RepairTimer( )
	{
		yield return new WaitForSeconds(5f);
		shp.isReparing = false;
		ani.SetTrigger ("FinishRepair");
		StartCoroutine (WaitAbilityUse ());
		timer.startCooldownTimerUI(2);
		skillTimeStamp2 = Time.time + repairSpaceShipCooldown;

	}

	IEnumerator WaitAbilityUse()
	{
		yield return new WaitForSeconds(repairSpaceShipCooldown);
		canRepair = true;
	}

    IEnumerator DelayButtonPress()
    {
        canUseAbility = false;
        yield return new WaitForSeconds(.1f);
        canUseAbility = true;
    }

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
		if (ability == SpecialAbility.MakeGhostTurret)
		{
            if (!currentPlaceableObject && canUseAbility)
            {
                CreateTurretGhost();
                StartCoroutine(DelayButtonPress());
                GameObject.Find("Skill1").GetComponent<Image>().sprite = genericSkillSprite;
                GameObject.Find("Skill2").GetComponent<Image>().sprite = mechanicSkillOne;
            }

            if (currentPlaceableObject && canUseAbility)
            {
                ExitBuildingMode();
                StartCoroutine(DelayButtonPress());
            }
		}

        if (ability == SpecialAbility.Build)
        {
            if (canBuild && !IsAtMaxTurrets())
            {
                if (!highlightedTurret)
                {
                    // Start cooldown animation for UI skill image
                    timer.startCooldownTimerUI(1);
                    skillTimeStamp1 = Time.time + buildTurretCooldown;
                    canBuild = false;
                }
                BuildTurret();
            }

            if (!isBuilding && highlightedTurret)
            {
                BuildTurret();
            }
        }
		if (ability == SpecialAbility.RepairShip) {

			RepairShip ();
		}
    }

    public bool CanAim()
    {
        return !isBuilding;
    }

    public bool CanIdle()
    {
        return !isBuilding;
    }

    public bool CanJump()
    {
        return !isBuilding;
    }

    public bool CanMove()
    {
        return !isBuilding;
    }

    public bool CanPickUpObject()
    {
        return !isBuilding;
    }

    public bool CanReload()
    {
        return !isBuilding;
    }

    public bool CanRoll()
    {
        return !isBuilding;
    }

    public bool CanShoot()
    {
        return !isBuilding;
    }

    public bool CanSprint()
    {
        return !isBuilding;
    }

	public bool CanUseAbility1()
	{
		if ((currentPlaceableObject && !canUseAbility) || isBuilding) {
			return false;
		}

		return true;
	}

	public bool CanUseAbility2()
	{
		if (currentPlaceableObject)
		{
			return false;
		}

		return true;
	}

    public bool OverrideAbility2()
    {
        if (currentPlaceableObject)
        {
            return true;
        }

        return false;
    }

    public void FixedUpdateActions(float deltaTime)
    {
        if (currentPlaceableObject)
        {
            buildPosition = transform.localPosition + transform.forward * buildPositionOffset;
            List<Collider> nearbyTurrets = new List<Collider>(Physics.OverlapSphere(buildPosition, 0.5f, LayerMask.GetMask("TurretParent"), QueryTriggerInteraction.Collide));
            foreach (Collider turret in nearbyTurrets)
            {
                if (!turret.GetComponent<TurretBehaviors>().engineer.Equals(gameObject))
                {
                    nearbyTurrets.Remove(turret);
                }
            }
            if (nearbyTurrets.Count > 0)
            {
                if (currentPlaceableObject.activeSelf)
                {
                    currentPlaceableObject.SetActive(false);
                }

                Collider nearestTurret = nearbyTurrets[0];
                float nearestDistance = Vector3.Distance(buildPosition, nearestTurret.transform.position);
                for (int i = 1; i < nearbyTurrets.Count; i++)
                {
                    float distance = Vector3.Distance(buildPosition, nearbyTurrets[i].transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestTurret = nearbyTurrets[i];
                        nearestDistance = distance;
                    }
                }

                if (highlightedTurret && !nearestTurret.gameObject.Equals(highlightedTurret))
                {
                    Placement.SetMaterial(highlightedTurret.transform, previousTurretMaterial);
                }

                if (!highlightedTurret || !nearestTurret.gameObject.Equals(highlightedTurret))
                {
                    highlightedTurret = nearestTurret.gameObject;
                    previousTurretMaterial = turretMaterials[highlightedTurret.GetComponent<TurretBehaviors>().turretLevel];
                    Placement.SetMaterial(highlightedTurret.transform, highlightMaterial);
                }
            }
            else
            {
                if (highlightedTurret)
                {
                    Placement.SetMaterial(highlightedTurret.transform, previousTurretMaterial);
                    highlightedTurret = null;
                    previousTurretMaterial = null;
                }

                if (!currentPlaceableObject.activeSelf)
                {
                    currentPlaceableObject.SetActive(true);
                }
                
                currentPlaceableObject.transform.position = buildPosition;
            }
        }
    }

    public void StopAction()
    {
        ExitBuildingMode();
    }

    public void UpdateActions(float deltaTime)
    {
        if (isBuilding)
        {
            if (currentBuildingTime >= buildingPieceTime)
            {
                currentBuildingTime -= buildingPieceTime;
                if (currentBuildingChildren.Count == 0)
                {
                    if (currentBuildingIteration > 0)
                    {
                        ResetBuildingVariables();
                    }
                    else
                    {
                        currentBuildingChildren = ActivateChildren(currentTurret.transform);
                        currentBuildingIteration++;
                    }
                }
                else
                {
                    List<Transform> temp = new List<Transform>();
                    foreach (Transform child in currentBuildingChildren)
                    {
                        temp.AddRange(ActivateChildren(child.transform));
                    }
                    currentBuildingChildren = temp;

                    if (!isBuilding)
                    {
                        ResetBuildingVariables();
                    }
                }
            }
            currentBuildingTime += deltaTime;
        }
    }

    public void UpdateAnimationStates(Animator animator)
    {
        if (animator)
        {
            animator.SetBool("Building", isBuilding);
        }
    }

    #endregion
}
