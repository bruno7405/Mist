using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData itemData;
    [SerializeField] int quantity;

    public void OnInteract(PlayerInteractor player)
    {
        // Try adding to hotbar inventory
        player.GetComponent<PlayerEquip>().GetHotbarInventory().AddToInventory(itemData, quantity, out int remainingAmt1);

        if (remainingAmt1 > 0)
        {
            // Try adding to main inventory
            player.GetComponent<PlayerEquip>().GetMainInventory().AddToInventory(itemData, remainingAmt1, out int remainingAmt2);

            // no space in inventory to add all items
            if (remainingAmt2 > 0)
            {
                quantity = remainingAmt2;
                return;
            }
        }

        AudioManager.instance.PlayOneShot("PickupItem", 0.2f);

        Destroy(gameObject);
    }

    public void OnInteractExit()
    {
        throw new System.NotImplementedException();
    }

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
    }

}
