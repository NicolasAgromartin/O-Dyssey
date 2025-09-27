using System;
using UnityEngine;
using UnityEngine.UI;




public class TitleScreen : MonoBehaviour
{
    [Header("Instructions")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button instructionsButton;

    private bool instructionsOpened = false;
    private Navigation navigation;





    #region Life Cykle
    private void Awake()
    {
        navigation = GetComponent<Navigation>();
    }
    private void Start()
    {
        instructionsButton.onClick.AddListener(ToggleInstructions);
    }
    private void OnEnable()
    {
        navigation.OnStartGame_ButtonPressed += CloseInstructions;
    }
    private void OnDisable()
    {
        navigation.OnStartGame_ButtonPressed -= CloseInstructions;
    }
    #endregion



    private void ToggleInstructions()
    {
        instructionsOpened = !instructionsOpened;

        if (instructionsOpened)
        {
            instructionsPanel.SetActive(true);
        }
        else
        {
            instructionsPanel.SetActive(false);
        }
    }
    public void CloseInstructions()
    {
        instructionsOpened = false;
        instructionsPanel.SetActive(false);
    }
}
