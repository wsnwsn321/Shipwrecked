using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class NavController : MonoBehaviour {

	public Navigator initialSelected;

	private Navigator currentSelected;

	private int selectedCooldown = 40;
	private int currentCooldown = 0;

	// Use this for initialization
	public void InitializeController () {
		if (initialSelected != null) {
			currentSelected = initialSelected;
		}
	}
	
	// Update is called once per frame
	public void UpdateController () {
		if (currentSelected != null) {
			if (InputManager.MenuNavigateUp ()) {
				currentSelected = currentSelected.moveUp ();
				EventSystem.current.SetSelectedGameObject(currentSelected.gameObject);
			} else if (InputManager.MenuNavigateDown ()) {
				currentSelected = currentSelected.moveDown ();
				EventSystem.current.SetSelectedGameObject(currentSelected.gameObject);
			} else if (InputManager.MenuNavigateLeft ()) {
				currentSelected = currentSelected.moveLeft ();
				EventSystem.current.SetSelectedGameObject(currentSelected.gameObject);
			} else if (InputManager.MenuNavigateRight ()) {
				currentSelected = currentSelected.moveRight ();
				EventSystem.current.SetSelectedGameObject(currentSelected.gameObject);
			} else if (InputManager.MenuSelect ()) {
				currentSelected.Select ();
			}
			
		}

	}
}
