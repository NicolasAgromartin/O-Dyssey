using System;
using UnityEngine;
using UnityEngine.EventSystems;



[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public event Action OnVictoryPointReached;

    [Header("Movement")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 moveDirection;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Interaction")]
    [SerializeField] private GameObject interactionMark;
    [SerializeField] private GameObject interactionArea;
    [SerializeField] private LayerMask layersToInteract;

    [Header("References")]
    [SerializeField] private LevelManager levelManager;


    [Header("Audio")]
    [SerializeField] private AudioSource SFX_Source;
    [SerializeField] private AudioClip pickabeClip;
    [SerializeField] private AudioClip victoryClip;




    #region Life Cykle
    private void OnEnable()
    {
        levelManager = FindAnyObjectByType<LevelManager>();

        levelManager.OnLevelStart += EnableInputs;

        levelManager.OnLevelCompleted += DisableInputs;
        levelManager.OnGameWon += DisableInputs;
        levelManager.OnTimerEnd += DisableInputs;
    }
    private void OnDisable()
    {
        levelManager.OnLevelStart -= EnableInputs;

        levelManager.OnLevelCompleted -= DisableInputs;
        levelManager.OnGameWon -= DisableInputs;
        levelManager.OnTimerEnd -= DisableInputs;

        DisableInputs();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VictoryPoint"))
        {           
            SFX_Source.PlayOneShot(victoryClip); 

            OnVictoryPointReached?.Invoke();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            interactionMark.SetActive(true);
        }

        if (other.gameObject.CompareTag("Pickable"))
        {
            // pick
            SFX_Source.PlayOneShot(pickabeClip);
            other.gameObject.GetComponent<Pickable>().PickUp(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            interactionMark.SetActive(false);
        }
    }
    #endregion



    #region Inputs
    private void EnableInputs()
    {
        InputManager.OnInteract += Interact;
        InputManager.OnMovementAction += Move;
    }
    private void DisableInputs()
    {
        animator.SetFloat("MovementSpeed", 0f);
        InputManager.OnInteract -= Interact;
        InputManager.OnMovementAction -= Move;
    }
    #endregion



    #region Interaction
    private void Interact()
    {

        foreach(Collider collider in Physics.OverlapSphere(interactionArea.transform.position, .5f, layersToInteract))
        {
            if (collider.TryGetComponent(out IInteractable obj))
            {
                obj.Interact();
                if(obj is GateButton)
                {
                    // play button sound
                }
                if(obj is Pickable)
                {
                    // play pickupSound
                }
                break;
            }
        }

    }
    #endregion



    #region Movement
    private void Move(Vector3 moveInput)
    {
        characterController.SimpleMove(new Vector3(moveInput.x, characterController.velocity.y, moveInput.y) * moveSpeed);

        Rotate(moveInput);
    }
    private void Rotate(Vector3 moveInput)
    {
        moveDirection = new(moveInput.x, 0, moveInput.y);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDirection),
                rotationSpeed * Time.deltaTime
            );
        }

        animator.SetFloat("MovementSpeed", moveDirection.magnitude);
    }
    public void IncreaseMoveSpeed()
    {
        moveSpeed += 2;
    }
    public void GoToInitialPosition(Vector3 initialPos)
    {
        transform.position = initialPos;
    }
    #endregion



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(interactionArea.transform.position, .5f);
    }

}
