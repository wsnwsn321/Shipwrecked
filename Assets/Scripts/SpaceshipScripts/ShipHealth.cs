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
    void Start()
    {
        maxHealth = health;
		isReparing = false;
    }

    // Update is called once per frame
    void Update () {
		if (healthText == null) {
			healthText = GameObject.FindGameObjectWithTag("ShipHealthText").GetComponent<Text>();
			healthBar = GameObject.FindGameObjectWithTag("ShipHealthBar").GetComponent<Slider>();
		}
		if (tookDmg)
		{
			updateHealthText();
			updateHealthBar();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			TakeDamage(200);
		}

		if (isReparing) {
			health += 0.1025f;
			updateHealthText ();
			updateHealthBar ();
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (health);
		} else if (stream.isReading) {
			health = (int)stream.ReceiveNext ();
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
		SceneManager.LoadScene ("QuitCredits");
	}
}
