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
	private GameObject player_hit;
	private float nextAttack = 0f;
	public bool stunned, dead;

	public float attackCooldown = 2.5f;
    void Start () {
        an = GetComponent<Animation>();
        fov = GetComponentInParent<FieldOfView>();
        ap = GetComponentInParent<AIPath>();
        collide = false;
        distance = 10f;
		stunned = false;
		dead = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (fov.visibleTargets.Count > 0)
        {
            ap.maxSpeed = 3;
        }
        else
        {
			an.Play("Walk");
            ap.maxSpeed = 1;
        }
		if (player_hit != null) {
			distance = Vector3.Distance(transform.position, player_hit.transform.position);

		}
		if (distance>1.8f) {
			collide = false;
		}
		if (player_hit != null&&player_hit.layer==16) {
			Physics.IgnoreCollision (GetComponent<BoxCollider>(), player_hit.GetComponent<BoxCollider> ());
		}

		if (player_hit != null && player_hit.layer == 10) {
			Physics.IgnoreCollision (GetComponent<BoxCollider>(), player_hit.GetComponent<BoxCollider> (),false);
		}

		if (stunned) {
			an.Play ("Die_2");
			StartCoroutine (StunTimer ());
			ap.maxSpeed = 0;
		} else if (dead) {
			an.Play ("Die_3");
			ap.maxSpeed = 0;
		} else {

			if (collide && fov.visibleTargets.Count > 0) {
				if (!Player_ani.GetCurrentAnimatorStateInfo (0).IsName ("Die")) {
					ap.maxSpeed = 0;
					an.Play ("Attack_2");
					setEnemyAttackType ();
					if (!Player_ani.GetCurrentAnimatorStateInfo (0).IsName ("Hitted")) {

						Player_ani.SetTrigger ("Hit");
					}
				} else {
					an.Play ("Walk");
					ap.maxSpeed = 1;
				}


			} else {
				an.Play ("Walk");
				if (fov.visibleTargets.Count > 0) {
					ap.maxSpeed = 3;
				} else {
					ap.maxSpeed = 1;
				}
			}
		}


    }

	IEnumerator StunTimer()
	{
		yield return new WaitForSeconds(0.5f);
		an.Stop ();
	}

    void OnCollisionEnter(Collision collision)
    {
		if(collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Doctor"|| collision.gameObject.tag == "Captain" )
        {
			Player_ani = collision.gameObject.GetComponent<Animator> ();
			player_hit = collision.gameObject;
            distance = Vector3.Distance(transform.position, collision.gameObject.transform.position);
            collide = true;

        }

    }
    void OnCollisionExit(Collision collisionInfo)
    {
		if (collisionInfo.gameObject.tag == "Sarge" || collisionInfo.gameObject.tag == "Mechanic" || collisionInfo.gameObject.tag == "Doctor"|| collisionInfo.gameObject.tag == "Captain" ) 
        {
            distance = Vector3.Distance(transform.position, collisionInfo.gameObject.transform.position);
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
