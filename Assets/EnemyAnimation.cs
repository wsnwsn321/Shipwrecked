using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAnimation : MonoBehaviour {

    // Use this for initialization
    private Animator ani;
    //public GameObject AI;
    private Animator Player_ani;
    private FieldOfView fov;
    private AIPath ap;
    private float distance;
    bool run, attack, collide, hitted;
	void Start () {
        attack = false;
        run = false;
        collide = false;
        hitted = false;
        distance = 0f;
        fov = GetComponent<FieldOfView>();
        ap = GetComponent<AIPath>();
        ani = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        attack = false ;
            ani.SetBool("findPlayer", run);
            if (fov.visibleTargets.Count > 0&&!Player_ani.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                run = true;
                ap.maxSpeed = 3;
            }
            else
            {
                run = false;
                ap.maxSpeed = 1;
            }

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                ap.maxSpeed = 0;
            }

            if (collide)
            {
                if (!Player_ani.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                {
                    ap.maxSpeed = 0;
                    ani.SetTrigger("attack");
                    if (!Player_ani.GetCurrentAnimatorStateInfo(0).IsName("Hitted"))
                    {
                    Player_ani.SetTrigger("Hit");
                    }
                }
            else
            {
                //remove the player from the fov
                run = false;
                ap.maxSpeed = 1;
            }
               
            }
            else if (distance > 1.4f)
            {
                if (fov.visibleTargets.Count > 0)
                {
                    ap.maxSpeed = 3;
                }
                else
                {
                run = false;
                ap.maxSpeed = 1;
                }
            }
        
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Doctor")&& fov.visibleTargets.Count > 0)
        {
            Player_ani = collision.gameObject.GetComponent<Animator>();
            collide = true;
            distance = Vector3.Distance(transform.position, collision.gameObject.transform.position);

        }
           
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if ((collisionInfo.gameObject.tag == "Sarge" || collisionInfo.gameObject.tag == "Mechanic" || collisionInfo.gameObject.tag == "Doctor") && fov.visibleTargets.Count > 0)
        {
            collide = false;
            distance = Vector3.Distance(transform.position, collisionInfo.gameObject.transform.position);
            //hitted = false;
            //Player_ani.SetBool("hit", false);
        }
    }
}
