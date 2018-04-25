using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Paused,
    Running,
    MainMenu
}

public class GameplayManager : MonoBehaviour
{
    public static GameState State;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        State = GameState.Running;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        // Unlock the cursor.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        State = GameState.Paused;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        // Lock the cursor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        State = GameState.Running;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        // Lock the cursor.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        State = GameState.MainMenu;
    }
}
