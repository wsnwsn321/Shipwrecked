using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAnimation : MonoBehaviour {

    // Use this for initialization
    private Animator ani;
    //public GameObject AI;
    private Animator Player_ani;
	private GameObject player_hit;
    private FieldOfView fov;
    private AIPath ap;
    private float distance;
	private float nextAttack = 0f;

	public float attackCooldown = 2.5f;
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
		//if (Player_ani != null) {
			if (fov.visibleTargets.Count > 0)
			{
				run = true;
				ap.maxSpeed = 3;
			}
			else
			{
				run = false;
				ap.maxSpeed = 1;
			}
		//}
           

		if (ani.GetCurrentAnimatorStateInfo(0).IsName("Die")||ani.GetCurrentAnimatorStateInfo(0).IsName("Stunned")||ani.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
            {
                ap.maxSpeed = 0;
            }

		if (collide&&fov.visibleTargets.Count > 0)
            {
                if (!Player_ani.GetCurrentAnimatorStateInfo(0).IsName("Die"))
                {
                    ap.maxSpeed = 0;
                    ani.SetTrigger("attack");
                    setEnemyAttackType();

                    if (!Player_ani.GetCurrentAnimatorStateInfo(0).IsName("Hitted"))
                    {
                        Player_ani.SetTrigger("Hit");
                    }
                }
            else
            {
				Physics.IgnoreCollision (GetComponent<CapsuleCollider> (), player_hit.GetComponent<BoxCollider> ());
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
		if ((collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Doctor" || collision.gameObject.tag == "Captain") &&!ani.GetCurrentAnimatorStateInfo(0).IsName("Stunned")&&!ani.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
        {
            Player_ani = collision.gameObject.GetComponent<Animator>();
			player_hit = collision.gameObject;
            collide = true;
            distance = Vector3.Distance(transform.position, collision.gameObject.transform.position);

        }
           
    }
    void OnCollisionExit(Collision collisionInfo)
    {
		if ((collisionInfo.gameObject.tag == "Sarge" || collisionInfo.gameObject.tag == "Mechanic" || collisionInfo.gameObject.tag == "Doctor"|| collisionInfo.gameObject.tag == "Captain" ))
        {
            collide = false;
            distance = Vector3.Distance(transform.position, collisionInfo.gameObject.transform.position);
            //hitted = false;
            //Player_ani.SetBool("hit", false);
        }
    }

    void setEnemyAttackType()
    {
		if (player_hit != null && Time.time > nextAttack) {
			nextAttack = attackCooldown + Time.time;
			PlayerHealth ph = player_hit.GetComponent<PlayerHealth> ();
			switch (transform.gameObject.tag) {
			case "CrabAlien":
				// make this access the specific player that got hit instead of all instances of PlayerHealth
			// Otherwise, this will hit every player.
				ph.enemyAttackType = PlayerHealth.EnemyAttackType.CRAB_ALIEN;
				break;
			case "SpiderBrain":
				ph.enemyAttackType = PlayerHealth.EnemyAttackType.SPIDER_BRAIN;
				break;
			default:
				break;
			}
		}
    }
}
