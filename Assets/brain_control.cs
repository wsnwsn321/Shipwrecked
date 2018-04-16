using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brain_control : MonoBehaviour {

    // Use this for initialization
    private Animation an;
    private FieldOfView fov;
    private AIPath ap;
    private float distance, turretDistance, spaceDistance;
    private bool collide;
    private Animator Player_ani;
	private GameObject player_hit;
	private GameObject turrets;
	private GameObject spaceship;
	private float nextAttack = 0f;
	public bool stunned, dead, collideSpace;
	public static int currentSlot;
	private float runspeed, walkspeed;
	public float attackCooldown = 2.5f;
    void Start () {
        an = GetComponent<Animation>();
        fov = GetComponentInParent<FieldOfView>();
        ap = GetComponentInParent<AIPath>();
		spaceship = GameObject.Find("SpaceshipZone");
        collide = false;
        distance = 10f;
		stunned = false;
		dead = false;
		collideSpace = false;

    }
	
	// Update is called once per frame
	void Update () {

		switch(currentSlot)
		{
		case 0:
			runspeed = 3f;
			walkspeed = 1f;
			break;
		case 1:
			runspeed = 3.3f;
			walkspeed = 1.5f;
			break;
		case 2:
			runspeed = 3.6f;
			walkspeed = 1.8f;
			break;
		case 3:
			runspeed = 4f;
			walkspeed = 2f;
			break;
		case 4:
			runspeed = 4.5f;
			walkspeed = 2.5f;
			break;
		}
        if (fov.visibleTargets.Count > 0)
        {
			ap.maxSpeed = runspeed;
        }
        else
        {
			if (!collideSpace) {
				an.Play("Walk");
				ap.maxSpeed = walkspeed;
			}
				


        }
		if (player_hit != null) {
			distance = Vector3.Distance(transform.position, player_hit.transform.position);

		}
		if (turrets != null) {
			turretDistance = Vector3.Distance(transform.position, turrets.transform.position);

		}
		if (distance>1.8f) {
			collide = false;
		}
		spaceDistance = Vector3.Distance(transform.position, spaceship.transform.position);
		//colliding with spaceship
		if (spaceDistance > 6.1f) {
			collideSpace = false;
		} else {
			collideSpace = true;
		}
		if (player_hit != null&&player_hit.layer==16) {
			Physics.IgnoreCollision (GetComponent<BoxCollider>(), player_hit.GetComponent<BoxCollider> ());
		}

		if (player_hit != null && player_hit.layer == 10) {
			Physics.IgnoreCollision (GetComponent<BoxCollider>(), player_hit.GetComponent<BoxCollider> (),false);
		}


		if (stunned) {
			an.Play ("Die_2");
			if (dead) {
				stunned = false;
			}
			ap.maxSpeed = 0;
		} else if (dead) {
			an.Play ("Die_3");
			ap.maxSpeed = 0;
		}
		else{
			if (collideSpace&& fov.visibleTargets.Count ==0) {
				ap.maxSpeed = 0;
				an.Play ("Attack_1");
				setEnemyAttackTypeForSpaceship ();
			} 
			else if(turrets!=null){
				ap.maxSpeed = 0;
				an.Play ("Attack_1");
				setEnemyAttackTypeTurret ();
			}
			else {
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
						ap.maxSpeed = walkspeed;
					}


				} else {
					an.Play ("Walk");
					if (fov.visibleTargets.Count > 0) {
						ap.maxSpeed = runspeed;
					} else {
						ap.maxSpeed = walkspeed;
					}
				}
			}

		}

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

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Turret"))
		{
			turrets = other.gameObject;
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

	void setEnemyAttackTypeForSpaceship()
	{
		
		if (Time.time > nextAttack) {
			nextAttack = attackCooldown + Time.time;
			ShipHealth ph = spaceship.GetComponent<ShipHealth> ();
			switch (transform.gameObject.tag) {
			case "CrabAlien":
				// make this access the specific player that got hit instead of all instances of PlayerHealth
				// Otherwise, this will hit every player.
				ph.TakeDamage((int)PlayerHealth.EnemyAttackType.CRAB_ALIEN);
				break;
			case "SpiderBrain":
				ph.TakeDamage((int)PlayerHealth.EnemyAttackType.SPIDER_BRAIN);
				break;
			default:
				break;
			}
		}
	}

	void setEnemyAttackTypeTurret()
	{
		if (Time.time > nextAttack) {
			print ("turret taking damage2");
			nextAttack = attackCooldown + Time.time;
			TurretBehaviors tb = turrets.GetComponent<TurretBehaviors> ();
			switch (transform.gameObject.tag) {
			case "CrabAlien":
				// make this access the specific player that got hit instead of all instances of PlayerHealth
				// Otherwise, this will hit every player.
				tb.TakeDamage((int)PlayerHealth.EnemyAttackType.CRAB_ALIEN);
				break;
			case "SpiderBrain":
				tb.TakeDamage((int)PlayerHealth.EnemyAttackType.SPIDER_BRAIN);
				break;
			default:
				break;
			}
		}
	}
}
