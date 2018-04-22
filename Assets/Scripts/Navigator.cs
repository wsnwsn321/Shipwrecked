using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour {

	// Selection object to the left
	public GameObject leftObject;

	// Selection object to the right
	public GameObject rightObject;

	// Selection object above current object
	public GameObject upObject;

	// Selection object below current object
	public GameObject downObject;

	// Only necessary when the next navigator is not active
	public GameObject defaultObject;

	public bool isInputField = false;

	// Navigates to the left object
	public Navigator moveLeft() {
		if (leftObject != null) {
			if (leftObject.activeInHierarchy) {
				return leftObject.GetComponent<Navigator> ();
			} else {
				return defaultObject.GetComponent<Navigator> ();
			}
		}
		return this;
	}

	// Navigates to the right object
	public Navigator moveRight() {
		if (rightObject != null) {
			if (rightObject.activeInHierarchy) {
				return rightObject.GetComponent<Navigator> ();
			} else {
				return defaultObject.GetComponent<Navigator> ();
			}		}
		return this;
	}

	// Navigates to the object above 
	public Navigator moveUp() {
		if (upObject != null) {
			if (upObject.activeInHierarchy) {
				if (!upObject.GetComponent<Navigator> ().isInputField) {
					if (upObject.GetComponent<Button> ().interactable) {
						return upObject.GetComponent<Navigator> ();
					} else {
						return this;
					}
				}
				return upObject.GetComponent<Navigator> ();
			} else {
				return defaultObject.GetComponent<Navigator> ();
			}
		}
		return this;
	}

	// Navigates to the object below
	public Navigator moveDown() {
		// Navigational input messes with input fields
		if (!isInputField && downObject != null) {
			// Check if button is active
			if (downObject.activeInHierarchy && downObject.transform.localScale.magnitude > 0f) {
				return downObject.GetComponent<Navigator> ();
			} else {
				return defaultObject.GetComponent<Navigator> ();
			}
		}
		return this;
	}

	public void Select() {
		// I only bothered to tag InputFields since only two are in the game
		if (this.gameObject.tag.Equals ("InputField")) {
			//TODO Add inputfield keyboard input for controllers
			if (downObject.activeInHierarchy && downObject.transform.localScale.magnitude > 0f && downObject.GetComponent<Button>().interactable) {
				downObject.GetComponent<Navigator> ().Select();
			}
		} else {
			// We know it is a button
			this.gameObject.GetComponent<Button> ().onClick.Invoke ();
		}
	}

}
