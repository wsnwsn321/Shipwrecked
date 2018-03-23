using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Destructable {

	public override void Die ()
	{
		base.Die ();
		Destroy (gameObject);
		print("We died");

	}

	public override void TakeDamage (float amount)
	{
		base.TakeDamage (amount);
		print ("Remaining: " + HitPointsRemaining);
	}

}
