using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryMouseSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private ItemData itemData;
    private int quantity;

    private Vector2 offset = new Vector2 (10, -10);

    bool isActive = false;

    public PlayerEquip playerEquip;

    private void Start()
    {
        // Clear visuals
        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
        itemCountText.text = "";
    }

    private void Update()
    {

        if (itemData == null) return;

        // Follow mouse
        transform.position = Input.mousePosition + (Vector3) offset;
            
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // If not over UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Drop item
                //ItemSpawnManager.instance.SpawnItemOnPlayer(itemData, quantity);
                PlayerEquip.instance.DropItem(itemData, quantity);
                Clear();
            }
        }

        
    }

    public void SetItem(ItemData itemData, int amt)
    {
        this.itemData = itemData;
        quantity = amt;
        UpdateVisuals();
    }

    /// <summary>
    /// Clears data and updates visuals of slot
    /// </summary>
    public void Clear()
    {
        // Clear data
        itemData = null;
        quantity = 0;
        UpdateVisuals();
    }

    public bool SlotEmpty()
    {
        return itemData == null;
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // No item
        if (itemData == null)
        {
            // Clear visuals
            itemSprite.color = Color.clear;
            itemSprite.sprite = null;
            itemCountText.text = "";
            return;
        }

        // Yes item
        else
        {
            itemSprite.color = Color.white;
            itemSprite.sprite = itemData.itemImage;
            if (quantity > 1) itemCountText.text = quantity.ToString();
        }
    }
}
