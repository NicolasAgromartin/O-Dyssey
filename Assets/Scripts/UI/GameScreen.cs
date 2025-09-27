using UnityEngine;

public class GameScreen : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject levelCompletedScreen;


    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Navigation navigation;
    [SerializeField] private LevelManager levelManager;


    private void OnEnable()
    {
        levelManager.OnLevelCompleted += ShowLevelCompletedScreen;
        levelManager.OnTimerEnd += ShowDefeatScreen;

        navigation.OnNextLevel_ButtonPressed += HideLevelCompletedScreen;
        navigation.OnReplayLevel_ButtonPressed += HideDefeatScreen;
    }
    private void OnDisable()
    {
        levelManager.OnLevelCompleted -= ShowLevelCompletedScreen;
        levelManager.OnTimerEnd -= ShowDefeatScreen;

        navigation.OnNextLevel_ButtonPressed -= HideLevelCompletedScreen;
        navigation.OnReplayLevel_ButtonPressed -= HideDefeatScreen;

    }



    private void ShowLevelCompletedScreen()
    {
        blackScreen.SetActive(true);
        levelCompletedScreen.SetActive(true);
    }
    private void HideLevelCompletedScreen()
    {
        blackScreen.SetActive(false);
        levelCompletedScreen.SetActive(false);
    }

    private void ShowDefeatScreen()
    {
        blackScreen.SetActive(true);
        defeatScreen.SetActive(true);
    }
    private void HideDefeatScreen()
    {
        blackScreen.SetActive(false);
        defeatScreen.SetActive(false);
    }
}
