using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Keeps track of player input and connects it to other classes
/// </summary>
[RequireComponent(typeof(PlayerInteractor), typeof(PlayerEquip), typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction equipAction;
    private InputAction inventoryAction;
    private InputAction dropItemAction;
    private InputAction useItemAction;
    private Vector2 moveInput;
    private Vector2 lookInput;


    public bool active = true;
    public bool inventoryOpen = false;

    private PlayerMovement playerMovement;
    private PlayerInteractor playerInteraction;
    private PlayerLook playerLook;
    private PlayerEquip playerEquip;

    public UnityEvent ChangeInventoryUIState;

    [SerializeField] private MuzzleFlash muzzleFlash;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteractor>();
        playerLook = GetComponent<PlayerLook>();
        playerEquip = GetComponent<PlayerEquip>();

        ChangeInventoryUIState.AddListener(GameObject.Find("UIController").GetComponent<UIController>().UpdateInventoryState);
        
        moveAction = playerControls.FindActionMap("Gameplay").FindAction("Move");
        lookAction = playerControls.FindActionMap("Gameplay").FindAction("Look");
        jumpAction = playerControls.FindActionMap("Gameplay").FindAction("Jump");
        sprintAction = playerControls.FindActionMap("Gameplay").FindAction("Sprint");
        interactAction = playerControls.FindActionMap("Gameplay").FindAction("Interact");
        equipAction = playerControls.FindActionMap("Gameplay").FindAction("Equip");
        inventoryAction = playerControls.FindActionMap("Gameplay").FindAction("Inventory");
        dropItemAction = playerControls.FindActionMap("Gameplay").FindAction("Drop");
        useItemAction = playerControls.FindActionMap("Gameplay").FindAction("Use");

        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        // Movement Actions (send to PlayerMovement script)
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();

        // Inventory Actions
        interactAction.Enable();
        equipAction.Enable();
        inventoryAction.Enable();
        dropItemAction.Enable();
        useItemAction.Enable();

        interactAction.performed += Interact;
        equipAction.performed += Equip;
        inventoryAction.performed += Inventory;
        dropItemAction.performed += DropItem;
        useItemAction.started += UseItem;
        useItemAction.canceled += StopUseItem;

    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
        interactAction.Disable();
        equipAction.Disable();
        inventoryAction.Disable();
        dropItemAction.Disable();
        useItemAction.Disable();

        interactAction.started -= Interact;
        equipAction.performed -= Equip;
        inventoryAction.started -= Inventory;
        dropItemAction.started -= DropItem;
        useItemAction.started -= UseItem;
    }


    void Update()
    {
        if (!active) return;
        
        playerMovement.HandleMovement(moveInput, sprintAction);
        playerMovement.HandleJump(jumpAction);
        playerLook.HandleRotation(lookInput);
    }



    private void Interact(InputAction.CallbackContext context)
    {
        if (!active) return;
        playerInteraction.Interact();
    }

    private void Equip(InputAction.CallbackContext context)
    {
        if (!active) return;
        if (inventoryOpen) return;

        var key = context.control.name;
        switch (key)
        {
            case "1":
                playerEquip.EquipItem(0);
                break;
            case "2":
                playerEquip.EquipItem(1);
                break;
            case "3":
                playerEquip.EquipItem(2);
                break;
            case "4":
                playerEquip.EquipItem(3);
                break;
            case "5":
                playerEquip.EquipItem(4);
                break;
        }
    }

    private void Inventory(InputAction.CallbackContext context)
    {
        if (!active) return;
        ChangeInventoryUIState.Invoke();
        playerLook.ChangeActiveState();
        inventoryOpen = !inventoryOpen;
    }

    private void DropItem(InputAction.CallbackContext context)
    {
        if (!active) return;
        playerEquip.DropCurrentItem();
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        if (!active) return;
        
        //muzzleFlash.Flash();
        playerEquip.UseItem();
    }

    private void StopUseItem(InputAction.CallbackContext context)
    {
        if (!active) return;

        playerEquip.StopUseItem();
    }
}
