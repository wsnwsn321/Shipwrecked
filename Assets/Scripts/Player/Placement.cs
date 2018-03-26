using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour {

    public Material goodPlacementMaterial;
    public Material badPlacementMaterial;
    public LayerMask allowedLayers;

    [HideInInspector]
    public bool canBeBuilt = false;
    [HideInInspector]
    public CoreControl builderCoreControl;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        Collider[] overlap = Physics.OverlapBox(transform.position, col.bounds.extents, transform.rotation, allowedLayers);

        if (overlap.Length > 0 && !builderCoreControl.IsJumping())
        {
            SetMaterial(transform, goodPlacementMaterial);
            canBeBuilt = true;
            
        } else
        {
            SetMaterial(transform, badPlacementMaterial);
            canBeBuilt = false;
        }

		overlap = Physics.OverlapBox(transform.position, col.bounds.extents, transform.rotation, LayerMask.NameToLayer("TurretParent"));
    
		if (canBeBuilt && overlap.Length > 0)
		{
			SetMaterial(transform, badPlacementMaterial);
			canBeBuilt = false;

		}
	}

    void OnTriggerEnter(Collider collider)
    {
        LayerMask layer = collider.gameObject.layer;
        if (!layer.Equals(LayerMask.GetMask("Ground")))
        {
            SetMaterial(transform, badPlacementMaterial);
            canBeBuilt = false;
        } else
        {
            SetMaterial(transform, goodPlacementMaterial);
            canBeBuilt = true;
        }
    }

    public static void SetMaterial(Transform t, Material material)
    {
        foreach (Transform child in t)
        {
            child.GetComponent<MeshRenderer>().material = material;
            SetMaterial(child.transform, material);
        }
    }
}
