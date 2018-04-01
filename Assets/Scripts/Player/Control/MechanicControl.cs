using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class MechanicControl : MonoBehaviour, IClassControl {

    public float buildPositionOffset = 3;
    public float buildingPieceTime = 1.2f;
    public Material highlightMaterial;
    public GameObject[] turrets;
    public GameObject[] turretGhosts;
    public Material[] turretMaterials;

    [HideInInspector]
    public int currentTurretBuildLevel = 0;
    [HideInInspector]
    public float turretHealth = 20f;
    [HideInInspector]
    public float turretDamage = 2f;

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
        isBuilding = false;
        currentBuildingChildren = new List<Transform>();
        unfinishedBuildings = new Dictionary<GameObject, UnfinishedBuilding>();
        ani = GetComponent<Animator>();
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
                isBuilding = false;
            }

            if (child.name.Equals("Turret"))
            {
                currentTurret.GetComponent<TurretBehaviors>().enabled = true;
            }
        }

        return children;
    }

    void BuildTurret()
    {
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
                ContinueBuildingNearestTurret(nearbyTurrets);
            }
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
        if (PhotonNetwork.connected)
        {
            currentPlaceableObject = PhotonNetwork.Instantiate(turretGhosts[currentTurretBuildLevel].name, buildPosition, Quaternion.identity, 0);
        }
        else
        {
            currentPlaceableObject = Instantiate(turretGhosts[currentTurretBuildLevel], buildPosition, Quaternion.identity);
        }

        currentPlaceableObject.GetComponent<Placement>().builderCoreControl = gameObject.GetComponent<CoreControl>();
    }

    void ExitBuildingMode()
    {
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

    #region Inherited Methods

    public void Activate(SpecialAbility ability)
    {
		if (ability == SpecialAbility.MakeGhostTurret)
		{
			CreateTurretGhost();
		}

        if (ability == SpecialAbility.Build)
        {
			Debug.Log ("What'sup");
            BuildTurret();
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
		if (currentPlaceableObject || isBuilding) {
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
            Collider[] nearbyTurrets = Physics.OverlapSphere(buildPosition, 0.5f, LayerMask.GetMask("TurretParent"), QueryTriggerInteraction.Collide);
            if (nearbyTurrets.Length > 0)
            {
                if (currentPlaceableObject.activeSelf)
                {
                    currentPlaceableObject.SetActive(false);
                }

                Collider nearestTurret = nearbyTurrets[0];
                float nearestDistance = Vector3.Distance(buildPosition, nearestTurret.transform.position);
                for (int i = 1; i < nearbyTurrets.Length; i++)
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
