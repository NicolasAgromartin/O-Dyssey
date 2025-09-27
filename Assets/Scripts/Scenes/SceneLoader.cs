using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class SceneLoader : Singleton<SceneLoader>
{
    public event Action OnSceneLoaded;
    public event Action OnLevelLoaded;


    private readonly Dictionary<SceneName, string> sceneByName = new();
    private readonly Dictionary<SceneName, string> levelByName = new();


    public SceneName CurrentScene { get; private set; }
    public SceneName CurrentLevel { get; private set; }

    [SerializeField] private LoadingScreen loadingScreen;
    private Navigation navigation;






    new private void Awake()
    {
        base.Awake();
        
        SetScenesDictionary();

        SuscribeToButtonsEvents();
        OnSceneLoaded += SuscribeToButtonsEvents;
    }
    private void OnDestroy()
    {
        OnSceneLoaded -= SuscribeToButtonsEvents;
        Unsuscribe();
    }



    private void SuscribeToButtonsEvents()
    {
        navigation = FindAnyObjectByType<Navigation>();

        Unsuscribe();

        navigation.OnStartGame_ButtonPressed += GoToGame;
        navigation.OnTitleScreen_ButtonPressed += GoToTitleScreen;
        navigation.OnExitGame_ButtonPressed += ExitGame;
        navigation.OnNextLevel_ButtonPressed += GoToNextLevel;
        navigation.OnReplayLevel_ButtonPressed += ReplayLevel;
    }
    private void Unsuscribe()
    {
        navigation.OnStartGame_ButtonPressed -= GoToGame;
        navigation.OnTitleScreen_ButtonPressed -= GoToTitleScreen;
        navigation.OnExitGame_ButtonPressed -= ExitGame;
        navigation.OnNextLevel_ButtonPressed -= GoToNextLevel;
        navigation.OnReplayLevel_ButtonPressed -= ReplayLevel;
    }



    #region Buttons Listeners
    private void GoToGame()
    {
        StartCoroutine(ChangeActiveScene(SceneName.Game));
    }
    private void GoToTitleScreen()
    {
        StartCoroutine(ChangeActiveScene(SceneName.TitleScreen));
    }
    private void GoToNextLevel()
    {
        Debug.Log("Changing level"); // para testear directamente en la escena GAME
        if(CurrentLevel == SceneName.TitleScreen)
        {
            StartCoroutine(ChangeActiveLevel(SceneName.Level_02));
            return;
        }

        switch (CurrentLevel)
        {
            case SceneName.Level_01:
                StartCoroutine(ChangeActiveLevel(SceneName.Level_02));
                break;
            case SceneName.Level_02:
                StartCoroutine(ChangeActiveLevel(SceneName.Level_03));
                break;
            case SceneName.Level_03:
                Debug.Log("Fin del juego");
                break;
        }
    }
    private void ReplayLevel()
    {
        StartCoroutine(ChangeActiveLevel(CurrentLevel));
    }
    public void GoToGameOverScreen()
    {
        StartCoroutine(ChangeActiveScene(SceneName.GameOverScreen));
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    #endregion



    private void SetScenesDictionary()
    {
        foreach (SceneName scene in Enum.GetValues(typeof(SceneName))) sceneByName.Add(scene, scene.ToString());
        foreach (SceneName scene in Enum.GetValues(typeof(SceneName)))
        {
            if (scene != SceneName.Game || scene != SceneName.TitleScreen || scene != SceneName.GameOverScreen)
            {
                levelByName.Add(scene, scene.ToString());
            }
        }
    }



    // creo que cuando cambia la escena se activa y se desactiva la loading screen 
    // lo que hace que se vea lo que hay detras brevemente
    private IEnumerator ChangeActiveScene(SceneName scene)
    {
        loadingScreen.gameObject.SetActive(true);

        yield return StartCoroutine(loadingScreen.ShowBlackScreen());

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneByName[scene]);
        op.allowSceneActivation = false;


        while (op.progress < .9f)
        {
            loadingScreen.LoadBar(op.progress);
            yield return null;
        }
        op.allowSceneActivation = true;

        if (scene == SceneName.Game)
        {
            yield return StartCoroutine(ChangeActiveLevel(SceneName.Level_01));
        }

        yield return StartCoroutine(loadingScreen.HideBlackScreen());

        CurrentScene = scene;
        OnSceneLoaded?.Invoke();
        loadingScreen.gameObject.SetActive(false);
    }
    private IEnumerator ChangeActiveLevel(SceneName level)
    {
        loadingScreen.gameObject.SetActive(true);

        yield return StartCoroutine(loadingScreen.ShowBlackScreen());


        UnloadLevel(level);

        AsyncOperation op = SceneManager.LoadSceneAsync(levelByName[level], LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (op.progress < .9f)
        {
            loadingScreen.LoadBar(op.progress);
            yield return null;
        }
        op.allowSceneActivation = true;

        yield return StartCoroutine(loadingScreen.HideBlackScreen());

        CurrentLevel = level;
        OnLevelLoaded?.Invoke();
        loadingScreen.gameObject.SetActive(false);

        Debug.Log($"loaded {level}");
    }

    private void UnloadLevel(SceneName level)
    {
        if (level == CurrentLevel && CurrentScene != SceneName.TitleScreen) // reintentar nivel descarga el actual
        {
            SceneManager.UnloadSceneAsync(levelByName[CurrentLevel]);
        }
        else
        {
            if (level == SceneName.Level_02)
            {
                SceneManager.UnloadSceneAsync(levelByName[SceneName.Level_01]);
            }
            if (level == SceneName.Level_03)
            {
                SceneManager.UnloadSceneAsync(levelByName[SceneName.Level_02]);
            }
        }
    }
}
