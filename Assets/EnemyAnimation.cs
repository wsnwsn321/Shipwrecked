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
    private float distance, spaceshipDistance, turretDistance;
	private float nextAttack = 0f;
	private GameObject Spaceship;
	private GameObject turrets;
	public static int currentSlot = 0;
	public float attackCooldown = 1.5f;
    bool run, attack, collide, hitted, spaceship;
	private float runspeed, walkspeed;
	void Start () {
        attack = false;
        run = false;
        collide = false;
        hitted = false;
		spaceship = false;
        distance = 0f;
        fov = GetComponent<FieldOfView>();
        ap = GetComponent<AIPath>();
        ani = GetComponent<Animator>();
		Spaceship = GameObject.Find("SpaceshipZone");
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

        attack = false ;
            ani.SetBool("findPlayer", run);
		//if (Player_ani != null) {
			if (fov.visibleTargets.Count > 0)
			{
				run = true;
			ap.maxSpeed = runspeed;
			}
			else
			{
				run = false;
			ap.maxSpeed = walkspeed;
			}
		//}
           

		if (ani.GetCurrentAnimatorStateInfo(0).IsName("Die")||ani.GetCurrentAnimatorStateInfo(0).IsName("Stunned")||ani.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
            {
                ap.maxSpeed = 0;
            }

		if (player_hit != null&&player_hit.layer==16) {
			Physics.IgnoreCollision (GetComponent<CapsuleCollider> (), player_hit.GetComponent<BoxCollider> ());
		}
		if (player_hit != null&&player_hit.layer==10) {
			Physics.IgnoreCollision (GetComponent<CapsuleCollider> (), player_hit.GetComponent<BoxCollider> (),false);
		}

		if (collide && fov.visibleTargets.Count > 0) {
			if (!Player_ani.GetCurrentAnimatorStateInfo (0).IsName ("Die")) {
				ap.maxSpeed = 0;
				ani.SetTrigger ("attack");
				setEnemyAttackType ();

				if (!Player_ani.GetCurrentAnimatorStateInfo (0).IsName ("Hitted")) {
					Player_ani.SetTrigger ("Hit");
				}
			} else {
				//remove the player from the fov
				run = false;
				ap.maxSpeed = walkspeed;
			}
               
		} else if (distance > 1.4f) {
			if (fov.visibleTargets.Count > 0) {
				ap.maxSpeed = runspeed;
			} else {
				run = false;
				ap.maxSpeed = walkspeed;
			}
		}

		if (Player_ani!=null&&Player_ani.GetCurrentAnimatorStateInfo (0).IsName ("Die")) {
			run = false;
			ap.maxSpeed = walkspeed;
		}

		spaceshipDistance = Vector3.Distance (transform.position, Spaceship.transform.position);

		//colliding with spaceship
		if (spaceshipDistance<6f) {
			ap.maxSpeed = 0;
			ani.SetTrigger("attack");
			setEnemyAttackTypeForSpaceship ();
		}

		//colliding with turret
		if (turrets != null) {
			
			turretDistance = Vector3.Distance (transform.position, turrets.transform.position);
			if (turretDistance < 2f) {
				ap.maxSpeed = 0;
				ani.SetTrigger("attack");
				print ("turret taking damage3");
				setEnemyAttackTypeTurret ();
			}
		}

        
        
    }

    void OnCollisionEnter(Collision collision)
    {
		if ((collision.gameObject.tag == "Sarge" || collision.gameObject.tag == "Mechanic" || collision.gameObject.tag == "Doctor" || collision.gameObject.tag == "Captain") && !ani.GetCurrentAnimatorStateInfo (0).IsName ("Stunned") && !ani.GetCurrentAnimatorStateInfo (0).IsName ("GetUp")) {
			Player_ani = collision.gameObject.GetComponent<Animator> ();
			player_hit = collision.gameObject;
			collide = true;
			distance = Vector3.Distance (transform.position, collision.gameObject.transform.position);

		} else if (collision.gameObject.tag == "Spaceship") {
			spaceship = true;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            turrets = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        
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
			player_hit = null;
		}
    }

	void setEnemyAttackTypeForSpaceship()
	{
		if (Time.time > nextAttack) {
			nextAttack = attackCooldown + Time.time;
			ShipHealth ph = Spaceship.GetComponent<ShipHealth> ();
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
