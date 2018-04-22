using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Controls

    #region Gameplay

    public static bool Ability1()
    {
        return Input.GetKeyDown(KeyCode.Q) || LeftButton();
    }

    public static bool Ability2()
    {
        return Input.GetKeyDown(KeyCode.E) || RightButton();
    }

    public static bool Aim()
    {
        return Input.GetMouseButton(1) || LeftTrigger() > 0f;
    }

    public static bool Emote1()
    {
        return Input.GetKeyDown(KeyCode.Alpha1) || DPadHorizontal() > 0f;
    }

    public static bool Emote2()
    {
        return Input.GetKeyDown(KeyCode.Alpha2) || DPadHorizontal() < 0f;
    }

    public static bool Emote3()
    {
        return Input.GetKeyDown(KeyCode.Alpha3) || DPadVertical() > 0f;
    }

    public static bool Emote4()
    {
        return Input.GetKeyDown(KeyCode.Alpha4) || DPadVertical() < 0f;
    }

    public static bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space) || AButton();
    }

	public static bool NavigateUp() {
		if (Input.GetKeyUp (KeyCode.UpArrow) || LeftJoystickVertical () > 0.2f) {
			return true;
		}
		return false;
	}

	public static bool NavigateDown() {
		if (Input.GetKeyUp (KeyCode.DownArrow) || LeftJoystickVertical () < -0.2f) {
			return true;
		}
		return false;
	}

	public static bool NavigateLeft() {
		if (Input.GetKeyUp (KeyCode.LeftArrow) || LeftJoystickHorizontal () < -0.2f) {
			return true;
		}
		return false;
	}

	public static bool NavigateRight() {
		if (Input.GetKeyUp (KeyCode.RightArrow) || LeftJoystickHorizontal () > 0.2f) {
			return true;
		}
		return false;
	}

	public static bool ConfirmSelection() {
		return (Input.GetKeyUp (KeyCode.Return) || AButton ());
	}

    public static float LookVertical()
    {
        float result = Input.GetAxis("Mouse Y") + RightJoystickVertical();
        return Mathf.Clamp(result, -1f, 1f);
    }

    public static float LookHorizontal()
    {
        float result = Input.GetAxis("Mouse X") + RightJoystickHorizontal();
        return Mathf.Clamp(result, -1f, 1f);
    }

    public static float MovementForward()
    {
        float result = Input.GetAxis("Vertical") + LeftJoystickVertical();
        return Mathf.Clamp(result, -1f, 1f);
    }

    public static float MovementLateral()
    {
        float result = Input.GetAxis("Horizontal") + LeftJoystickHorizontal();
        return Mathf.Clamp(result, -1f, 1f);
    }

    public static bool Pause()
    {
        return Input.GetKeyDown(KeyCode.Escape) || StartButton();
    }

    public static bool Reload()
    {
        return Input.GetKeyDown(KeyCode.R) || XButton();
    }

    public static bool Roll()
    {
        return Input.GetKeyDown(KeyCode.C) || RightThumbstick();
    }

    public static bool ReviveAlly()
    {
        return Input.GetKeyDown(KeyCode.G) || YButton();
    }

    public static bool ShootDown()
    {
        bool shootDown = Input.GetMouseButtonDown(0) || (RightTrigger() > 0.2f && !rightTriggerInUse );

        rightTriggerInUse = RightTrigger() > 0.2f;

        return shootDown;
    }

    public static bool ShootHeld()
    {
        return Input.GetMouseButton(0) || RightTrigger() > 0.2f;
    }

    public static bool Sprint()
    {
        return Input.GetKey(KeyCode.LeftShift) || LeftThumbstick();
    }

    public static bool StopAbility()
    {
        return Input.GetKeyDown(KeyCode.BackQuote) || BButton();
    }

    #endregion Gameplay

    #region Menu

    public static bool MenuNavigateDown()
    {
        bool navDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || DPadVertical() < -0.2f || LeftJoystickVertical() < -0.2f;

        return navDown;
    }

    public static bool MenuNavigateUp()
    {
        bool navUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || DPadVertical() > 0.2f || LeftJoystickVertical() > 0.2f;

        return navUp;
    }

    public static bool MenuSelect()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || AButton() || StartButton();
    }

    #endregion Menu

    // Reset the in-use states when there is no input.
    public static void NoInput()
    {
        leftJoysticHorizontalInUse = false;
        leftJoysticVerticalInUse = false;
        rightJoysticHorizontalInUse = false;
        rightJoysticVerticalInUse = false;
        dPadHorizontalInUse = false;
        dPadVerticalInUse = false;
        leftTriggerInUse = false;
        rightTriggerInUse = false;
    }

    #endregion Controls

    #region Inputs

    #region Controller

    private static bool leftJoysticHorizontalInUse = false;
    private static float LeftJoystickHorizontal()
    {
        float result = Input.GetAxis("Left Joystick Horizontal");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool leftJoysticVerticalInUse = false;
    private static float LeftJoystickVertical()
    {
        float result = Input.GetAxis("Left Joystick Vertical");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool rightJoysticHorizontalInUse = false;
    private static float RightJoystickHorizontal()
    {
        float result = Input.GetAxis("Right Joystick Horizontal");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool rightJoysticVerticalInUse = false;
    private static float RightJoystickVertical()
    {
        float result = Input.GetAxis("Right Joystick Vertical");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool dPadHorizontalInUse = false;
    private static float DPadHorizontal()
    {
        float result = Input.GetAxis("DPad Horizontal");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool dPadVerticalInUse = false;
    private static float DPadVertical()
    {
        float result = Input.GetAxis("DPad Vertical");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool leftTriggerInUse = false;
    private static float LeftTrigger()
    {
        float result = Input.GetAxis("Left Trigger");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool rightTriggerInUse = false;
    private static float RightTrigger()
    {
        float result = Input.GetAxisRaw("Right Trigger");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static bool AButton()
    {
        return Input.GetButtonDown("A Button");
    }

    private static bool BButton()
    {
        return Input.GetButtonDown("B Button");
    }

    private static bool XButton()
    {
        return Input.GetButtonDown("X Button");
    }

    private static bool YButton()
    {
        return Input.GetButtonDown("Y Button");
    }

    private static bool LeftButton()
    {
        return Input.GetButtonDown("Left Button");
    }

    private static bool RightButton()
    {
        return Input.GetButtonDown("Right Button");
    }

    private static bool BackButton()
    {
        return Input.GetButtonDown("Back Button");
    }

    private static bool StartButton()
    {
        return Input.GetButtonDown("Start Button");
    }

    private static bool LeftThumbstick()
    {
        return Input.GetButton("Left Thumbstick");
    }

    private static bool RightThumbstick()
    {
        return Input.GetButton("Right Thumbstick");
    }

    #endregion Controller

    #endregion Inputs

    public static void UpdateLeftTriggerInUse(bool state)
    {
        leftTriggerInUse = state;
    }

    public static void UpdateRightTriggerInUse(bool state)
    {
        rightTriggerInUse = state;
    }
}
