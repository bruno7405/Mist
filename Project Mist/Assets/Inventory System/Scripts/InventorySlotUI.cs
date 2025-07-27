using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour
{
    public InventoryUI inventoryUI;

    // Frontend
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCountText;
    private Button button;

    // Backend
    InventoryData inventory; // the inventory the slot is in
    InventorySlot slot; // slot in backend

    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        // Data
        this.slot = slot;

        if (slot.itemData == null)
        {
            ClearSlotVisuals();
            return;
        }

        // Visuals
        itemSprite.color = Color.white;
        itemSprite.sprite = slot.itemData.itemImage;
        if (slot.quantity > 1) itemCountText.text = slot.quantity.ToString();
        else itemCountText.text = "";
    }

    public void ClearSlotVisuals()
    {
        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
        itemCountText.text = "";
    }

    public bool SlotEmpty()
    {
        return slot.itemData == null;
    }

    public void OnUISlotClick()
    {
        inventoryUI?.SlotClicked(this);
    }

}
