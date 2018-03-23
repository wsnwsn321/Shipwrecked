using System;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities;
using UnityEngine;

public class MechanicControl : MonoBehaviour, IClassControl {

    public float buildPositionOffset = 3;
    public float buildingPieceTime = 1.2f;
    public GameObject turret;
    public GameObject turretGhost;

    private bool isBuilding;
    private Vector3 buildPosition;
    private float currentBuildingTime = 0;
    private int currentBuildingIteration = 0;
    private List<Transform> currentBuildingChildren;
    private GameObject currentPlaceableObject;
    private GameObject currentTurret;
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
            
            // Stop at the prefab's child named "Gun".
            if (child.name.Equals("Gun"))
            {
                isBuilding = false;
            }
        }

        return children;
    }

    void BuildTurret()
    {
        if (isBuilding)
        {
            ExitBuildingMode();
        }
        else
        {
            if (currentPlaceableObject)
            {
                if (currentPlaceableObject.GetComponent<Placement>().canBeBuilt)
                {
                    BuildNewTurret();
                }
            }
            else
            {
                buildPosition = transform.localPosition + transform.forward * buildPositionOffset;
                Collider[] nearbyTurrets = Physics.OverlapSphere(buildPosition, 0.5f, LayerMask.GetMask("TurretParent"), QueryTriggerInteraction.Collide);
                if (nearbyTurrets.Length > 0)
                {
                    ContinueBuildingNearestTurret(nearbyTurrets);
                }
                else
                {
                    CreateTurretGhost();
                }
            }
        }
    }

    void BuildNewTurret()
    {
        isBuilding = true;
        Destroy(currentPlaceableObject);
        currentPlaceableObject = null;
        currentTurret = Instantiate(turret, buildPosition, Quaternion.identity);
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
        currentPlaceableObject = Instantiate(turretGhost, buildPosition, Quaternion.identity);
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
                isBuilding = false;
            }
        }
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
        if (ability == SpecialAbility.Build)
        {
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

    public void FixedUpdateActions(float deltaTime)
    {
        if (currentPlaceableObject)
        {
            buildPosition = transform.localPosition + transform.forward * buildPositionOffset;
            currentPlaceableObject.transform.position = buildPosition;
        }
    }

    public void StopAction()
    {
        if (isBuilding)
        {
            ExitBuildingMode();
        }
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
