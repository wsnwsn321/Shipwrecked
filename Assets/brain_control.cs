using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brain_control : MonoBehaviour {

    // Use this for initialization
    private Animation an;
    private FieldOfView fov;
    private AIPath ap;
    private float distance;
    private bool collide;
    private Animator Player_ani;
    void Start () {
        an = GetComponent<Animation>();
        fov = GetComponentInParent<FieldOfView>();
        ap = GetComponentInParent<AIPath>();
        collide = false;
        distance = 10f;
    }
	
	// Update is called once per frame
	void Update () {
        if (fov.visibleTargets.Count > 0)
        {
            ap.maxSpeed = 3;
        }
        else
        {
            ap.maxSpeed = 1;
        }

		if (collide)
        {
            ap.maxSpeed = 0;
            an.Play("Attack_2");
            if (!Player_ani.GetCurrentAnimatorStateInfo(0).IsName("Hitted"))
            {
				
                Player_ani.SetTrigger("Hit");
            }
        }
        
		else if(distance>1.65f) {
            an.Play("Walk");
            if (fov.visibleTargets.Count > 0)
            {
                ap.maxSpeed = 3;
            }
            else
            {
                ap.maxSpeed = 1;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Doctor")
        {
			Player_ani = collision.gameObject.GetComponent<Animator> ();
            distance = Vector3.Distance(transform.position, collision.gameObject.transform.position);
            collide = true;
			print (distance);

        }

    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Sarge" || collisionInfo.gameObject.tag == "Mechanic" || collisionInfo.gameObject.tag == "Doctor") 
        {
			collide = false;
            distance = Vector3.Distance(transform.position, collisionInfo.gameObject.transform.position);
            //hitted = false;
            //Player_ani.SetBool("hit", false);
        }
    }
}
