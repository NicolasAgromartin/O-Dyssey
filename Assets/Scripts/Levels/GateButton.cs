using System;
using System.Collections.Generic;
using UnityEngine;





public class GateButton : MonoBehaviour, IInteractable
{
    public static event Action<int> OnButtonPressed;

    [Header("Canva")]
    [SerializeField] private GameObject buttonUI;

    [Header("Gates")]
    [SerializeField] List<Gate> linkedGates = new();

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip gateButtonClip;


    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) buttonUI.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) buttonUI.SetActive(false);
    }
    #endregion




    public void Interact()
    {
        OnButtonPressed?.Invoke(0);


        foreach(Gate gate in linkedGates)
        {
            gate.ToggleOpen();
            sfxSource.PlayOneShot(gateButtonClip);
        }

    }

}