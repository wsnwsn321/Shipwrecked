using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class characterSelectArrows : MonoBehaviour {

	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

	}

	void TaskOnClick()
	{
		if (gameObject.name.Equals ("LButton")) {
			GameObject.Find ("Character Cylinder").GetComponent<rotate> ().butr = true;
		} else {
			GameObject.Find ("Character Cylinder").GetComponent<rotate> ().butl = true;
		}
	}
}

