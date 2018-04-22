using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavController : MonoBehaviour {

	public Navigator initialSelected;

	private Navigator currentSelected;

	private bool wasSelected = false;
	private int selectedCooldown = 40;
	private int currentCooldown = 0;

	// Use this for initialization
	public void InitializeController () {
		if (initialSelected != null) {
			currentSelected = initialSelected;
			NavigationManager.showSelect.gameObject.transform.position = currentSelected.gameObject.transform.position;
		} else {
			Debug.Log ("Navigation controls not implemented for this view!");
		}
	}
	
	// Update is called once per frame
	public void UpdateController () {
		if (currentSelected != null && !wasSelected) {
			if (InputManager.NavigateUp ()) {
				currentSelected = currentSelected.moveUp ();
				Debug.Log (currentSelected.name);
				NavigationManager.showSelect.gameObject.transform.position = currentSelected.gameObject.transform.position;
			} else if (InputManager.NavigateDown ()) {
				currentSelected = currentSelected.moveDown ();
				Debug.Log (currentSelected.name);
				NavigationManager.showSelect.gameObject.transform.position = currentSelected.gameObject.transform.position;

			} else if (InputManager.NavigateLeft ()) {
				currentSelected = currentSelected.moveLeft ();
				Debug.Log (currentSelected.name);
				NavigationManager.showSelect.gameObject.transform.position = currentSelected.gameObject.transform.position;

			} else if (InputManager.NavigateRight ()) {
				currentSelected = currentSelected.moveRight ();
				Debug.Log (currentSelected.name);
				NavigationManager.showSelect.gameObject.transform.position = currentSelected.gameObject.transform.position;

			} else if (InputManager.ConfirmSelection ()) {
				currentSelected.Select ();
				currentSelected.highlightable.color = Color.green;
				wasSelected = true;
			}
			
		} else {
			currentCooldown++;
			if (currentCooldown >= selectedCooldown) {
				currentCooldown = 0;
				wasSelected = false;
				currentSelected.highlightable.color = Color.blue;
			}
		}

	}
}
