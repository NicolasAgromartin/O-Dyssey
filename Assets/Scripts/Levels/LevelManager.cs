using System;
using System.Collections;
using TMPro;
using UnityEngine;



public class LevelManager : MonoBehaviour
{
    #region Events
    public event Action OnLevelStart;
    public event Action OnLevelCompleted;
    public event Action OnGameWon;
    public event Action OnTimerEnd;
    #endregion

    [Header("References")]
    [SerializeField] private Player player;

    [Header("UI")]
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TMP_Text countdownValue;
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private TMP_Text timerValue;


    [Header("Timer Audio")]
    [SerializeField] private AudioSource counterSource;
    [SerializeField] private AudioClip countdownVoiceClip;
    [SerializeField] private AudioClip countClip;
    [SerializeField] private AudioClip goClip;
    [Header("Items Audio")]


    
    private readonly SceneName finalLevel = SceneName.Level_03;





    #region Life Cykle
    private void OnEnable()
    {
        SceneLoader.Instance.OnLevelLoaded += HandleLevelLoaded;
    }
    private void OnDisable()
    {
        SceneLoader.Instance.OnLevelLoaded -= HandleLevelLoaded;
    }
    #endregion

    #region Timers
    private void HandleLevelLoaded()
    {
        player = FindAnyObjectByType<Player>();
        player.OnVictoryPointReached += HandleLevelCompleted;

        StartCoroutine(StartTimer(countdownValue, 3f, true)); // ejecuta el evento countdown end
    }

    public void IncreaseTime()
    {
        elapsedTime += 10f;
    }
    #endregion



    private void HandleLevelCompleted()
    {
        player.OnVictoryPointReached -= HandleLevelCompleted;

        StopTimer();

        if (SceneLoader.Instance.CurrentLevel == finalLevel)
        {
            OnGameWon?.Invoke();
            SceneLoader.Instance.GoToGameOverScreen();
        }
        else
        {
            OnLevelCompleted?.Invoke();
        }
    }




    private float elapsedTime;
    private int lastNumber = -1;

    public IEnumerator StartTimer(TMP_Text value, float startAt, bool initial)
    {
        if (initial) countdownPanel.SetActive(true);
        else timerPanel.SetActive(true);

        elapsedTime = startAt;

        if(initial) counterSource.PlayOneShot(countdownVoiceClip);

        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime;
            int currentNumber = Mathf.CeilToInt(elapsedTime);

            // Solo actualiza y reproduce sonido si el número cambió
            if (currentNumber != lastNumber)
            {
                lastNumber = currentNumber;
                value.text = currentNumber.ToString();

                //if(initial && currentNumber != 0) counterSource.PlayOneShot(countClip);
            }

            yield return null;
        }

        // Al terminar el conteo
        if (initial)
        {
            value.text = "!GO";
            //counterSource.PlayOneShot(goClip);
            yield return new WaitForSeconds(1f);
            counterSource.Stop();

            countdownPanel.SetActive(false);
            OnLevelStart?.Invoke();
            StartCoroutine(StartTimer(timerValue, 15f, false));
        }
        else
        {
            timerPanel.SetActive(false);
            OnTimerEnd?.Invoke();
        }
    }


    private void StopTimer()
    {
        StopAllCoroutines();
        timerPanel.SetActive(false);
    }
}
