﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    #region Controls

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

    public static bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space) || AButton();
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

    public static bool Reload()
    {
        return Input.GetKeyDown(KeyCode.R) || XButton();
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
        return Input.GetKeyDown(KeyCode.Escape) || BButton();
    }

    #endregion Controls

    #region Inputs

    #region Controller

    private static float LeftJoystickHorizontal()
    {
        float result = Input.GetAxis("Left Joystick Horizontal");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static float LeftJoystickVertical()
    {
        float result = Input.GetAxis("Left Joystick Vertical");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static float RightJoystickHorizontal()
    {
        float result = Input.GetAxis("Right Joystick Horizontal");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static float RightJoystickVertical()
    {
        float result = Input.GetAxis("Right Joystick Vertical");
        return Mathf.Clamp(result, -1f, 1f);
    }

    private static float DPadHorizontal()
    {
        float result = Input.GetAxis("DPad Vertical");
        return Mathf.Clamp(result, -1f, 1f);
    }

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
