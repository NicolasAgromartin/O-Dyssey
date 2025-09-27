using System.Collections;
using TMPro;
using UnityEngine;




public class Gate : MonoBehaviour, IInteractable
{
    [Header("Gate Parts")]
    [SerializeField] private GameObject rightSide;
    private readonly Vector3 rightSideClosedPos = new(-0.00100647088f, 0.0165056996f, 0.0701216459f);
    private readonly Vector3 rightSideOpenPos = new(-0.00101904036f, -0.0489999987f, 0.068400003f);

    [SerializeField] private GameObject leftSide;
    private readonly Vector3 leftSideClosedPos = new(-0.00101904036f, -0.016225433f, 0.0684309751f);
    private readonly Vector3 leftSideOpenPos = new(0f, 0.0549999997f, 0.070100002f);

    private readonly Vector3 openedScale = new(1f, .1f, 1f);
    private readonly Vector3 closedScale = new(1f, 1f, 1f);

    [Header("Canvas")]
    [SerializeField] private GameObject messageBox;
    [SerializeField] private TMP_Text message;
    [SerializeField] private MeshRenderer stateIndicator;

    [Header("Gate Initial State")]
    [SerializeField] private bool isOpen = false;






    private void Awake()
    {
        if (isOpen) Open();
        else Close();

        ChangeMaterialColor();
    }
    public void Open()
    {
        rightSide.transform.localPosition = rightSideOpenPos;
        leftSide.transform.localPosition = leftSideOpenPos;
    }
    public void Close()
    {
        rightSide.transform.localPosition = rightSideClosedPos;
        leftSide.transform.localPosition = leftSideClosedPos;
    }
    public void ToggleOpen()
    {


        if (isOpen) Close();
        else Open();

        isOpen = !isOpen;

        ChangeMaterialColor();
    }


    private void ChangeMaterialColor()
    {
        Material material = stateIndicator.materials[0];

        if (isOpen)
        {
            material.SetColor("_BaseColor",Color.green);
        }
        else
        {
            material.SetColor("_BaseColor",Color.red);
        }
    }



    #region Interaction
    public void Interact()
    {
        if (messageBox.activeSelf) return;

        StartCoroutine(ShowMessage());
    }
    private IEnumerator ShowMessage()
    {
        message.text = isOpen ? "Im Open" : "Im Closed";
        messageBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        messageBox.SetActive(false);
    }
    #endregion


}
