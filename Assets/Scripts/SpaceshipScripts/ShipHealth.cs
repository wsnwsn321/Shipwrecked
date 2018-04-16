using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShipHealth : Photon.MonoBehaviour {

	public float health = 1000;
	public int earthDwellerDmg = 10;

    private float maxHealth;
	private Text healthText;
	private Slider healthBar;
	private bool tookDmg = false;
	private float timeColliding;
	public float timeThreshold = 1f;
	public bool isReparing;
	public AudioClip []clips;
	public static float shipReganRate =0.1025f;
    void Start()
    {
        maxHealth = health;
		isReparing = false;
    }

    // Update is called once per frame
    void Update () {

		if (healthText == null) {
			healthText = GameObject.Find("ShipHealthText").GetComponent<Text>();
			if (healthText == null) {
				Debug.Log ("Error finding Ship Health Text in ShipHealth.cs!!!");
			}
		}
		if (healthBar == null) {
			healthBar = GameObject.Find("ShipHealthBar").GetComponent<Slider>();
			if (healthBar == null) {
				Debug.Log ("Error finding Ship Health Bar in ShipHealth.cs!!!");
			}
		}
		
		if (tookDmg)
		{

			GameObject.Find("MissionText").GetComponent<AudioSource> ().clip = clips[0];

			GameObject.Find("MissionText").GetComponent<AudioSource> ().Play ();


			tookDmg = false;

		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			TakeDamage(200);
		}

		if (isReparing) {
			health += shipReganRate;
		}
		if ((!PhotonNetwork.connected || PhotonNetwork.isMasterClient) && health <= 750f) {
			health += 0.0125f;
		}

		if (health > 1000f) {
			health = 1000f;
		}


		updateHealthText ();
		updateHealthBar ();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (health);
		} else if (stream.isReading) {
			health = (float)stream.ReceiveNext ();
			tookDmg = true;
		}
	}


	public void TakeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			health = 0;
			loseGame ();
		}


		tookDmg = true;
	}


	private void updateHealthText()
	{
		healthText.text = Mathf.Floor (health).ToString() + "/" + maxHealth;
	}

	private void updateHealthBar()
	{
		healthBar.value = Mathf.RoundToInt((health / maxHealth) * 100);
	}

	private void loseGame()
	{
		PhotonNetwork.Destroy (PlayerManager.LocalPlayerInstance);
		SceneManager.LoadScene ("BadEnd");
	}
}
