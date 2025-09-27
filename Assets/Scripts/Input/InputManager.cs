using System;
using UnityEngine;
using UnityEngine.InputSystem;




public class InputManager : MonoBehaviour
{
    #region Events
    public static event Action OnInteract;
    public static event Action<Vector3> OnMovementAction;
    #endregion


    private PlayerInput playerInput;

    private InputAction movementAction;
    private InputAction interactAction;



    #region Life Cykle
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        movementAction = playerInput.actions.FindAction("Movement");
        interactAction = playerInput.actions.FindAction("Interact");
    }
    private void OnEnable()
    {
        interactAction.performed += InteractAction;
    }
    private void OnDisable()
    {
        interactAction.performed -= InteractAction;
    }
    private void Update()
    {
        OnMovementAction?.Invoke(movementAction.ReadValue<Vector2>().normalized);
    }
    #endregion


    private void InteractAction(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

}
