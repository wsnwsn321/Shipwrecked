using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour {

	public GameObject leftObject;
	public GameObject rightObject;
	public GameObject upObject;
	public GameObject downObject;

	// Only necessary when the next navigator is not active
	public GameObject defaultObject;


	public Image highlightable;
	public bool isInputField = false;

	void Start() {
		// I only bothered to tag InputFields since only two are in the game
		if (this.gameObject.tag.Equals ("InputField")) {
			//TODO Add inputfield keyboard input for controllers
			//highlightable = gameObject.GetComponent<Image>();
		} else {
			// We know it is a button
			highlightable = this.gameObject.GetComponent<Button>().image;

		}
		if (highlightable == null) {
			Debug.Log ("Error grabbing highlightable");
		}
	}

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
				return upObject.GetComponent<Navigator> ();
			} else {
				return defaultObject.GetComponent<Navigator> ();
			}
		}
		return this;
	}

	// Navigates to the object below
	public Navigator moveDown() {
		if (downObject != null) {
			if (downObject.activeInHierarchy) {
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

		} else {
			// We know it is a button
			this.gameObject.GetComponent<Button>().onClick.Invoke();
		}
	}

}
