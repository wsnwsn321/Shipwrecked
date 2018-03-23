using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour {

    public Material goodPlacementMaterial;
    public Material badPlacementMaterial;
    public LayerMask disallowedLayers;

    [HideInInspector]
    public bool canBeBuilt = false;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        Collider[] overlap = Physics.OverlapBox(transform.position, col.bounds.extents, transform.rotation, disallowedLayers);

        if (overlap.Length > 0)
        {
            SetMaterial(transform, badPlacementMaterial);
            canBeBuilt = false;
        } else
        {
            SetMaterial(transform, goodPlacementMaterial);
            canBeBuilt = true;
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

    void SetMaterial(Transform t, Material material)
    {
        foreach (Transform child in t)
        {
            child.GetComponent<MeshRenderer>().material = material;
            SetMaterial(child.transform, material);
        }
    }
}
