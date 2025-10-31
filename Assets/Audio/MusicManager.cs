using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : Singleton<MusicManager>
{
    [Header("Snapshots")]
    [SerializeField] private AudioMixerSnapshot titleScreen_Snapshot;
    [SerializeField] private AudioMixerSnapshot game_Snapshot;
    [SerializeField] private AudioMixerSnapshot gameOver_Snapshot;

    private void OnEnable()
    {
        SceneLoader.OnSceneLoaded += ChangeMusic;
    }
    private void OnDisable()
    {
        SceneLoader.OnSceneLoaded -= ChangeMusic;
    }

    private void ChangeMusic()
    {

        SceneName currentScene = SceneLoader.Instance.CurrentScene;

        switch (currentScene)
        {
            case SceneName.Game:
                game_Snapshot.TransitionTo(.4f);
                break;
            case SceneName.TitleScreen:
                game_Snapshot.TransitionTo(.4f);
                break;
            case SceneName.GameOverScreen:
                gameOver_Snapshot.TransitionTo(.4f);
                break;
        }
    }
}
