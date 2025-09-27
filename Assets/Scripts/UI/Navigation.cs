using UnityEngine;
using System;




public class Navigation : MonoBehaviour
{
    #region Events
    public event Action OnTitleScreen_ButtonPressed;
    public event Action OnStartGame_ButtonPressed;
    public event Action OnReplayLevel_ButtonPressed;
    public event Action OnExitGame_ButtonPressed;
    public event Action OnNextLevel_ButtonPressed;
    public event Action OnPause_ButtonPressed;
    #endregion

     


    public void GoToTitleScreen()
    {
        OnTitleScreen_ButtonPressed?.Invoke();
    }
    public void StartGame()
    {
        OnStartGame_ButtonPressed?.Invoke();
    }
    public void ReplayLevel()
    {
        OnReplayLevel_ButtonPressed?.Invoke();
    }
    public void ExitGame()
    {
        OnExitGame_ButtonPressed?.Invoke();
    }
    public void NextLevel()
    {
        OnNextLevel_ButtonPressed?.Invoke();
    }
    public void TogglePause()
    {
        OnPause_ButtonPressed?.Invoke();
    }
}
