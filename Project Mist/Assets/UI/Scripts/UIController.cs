using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public GameObject settingsCanvas;

    private void Awake()
    {
        // Initialize all inventory UI (main inventory and hotbar)
        InventoryUI[] inventoryUIs = inventoryCanvas.GetComponentsInChildren<InventoryUI>();
        foreach (InventoryUI i in inventoryUIs)
        {
            i.InitializeUI();
        }
    }

    #region Inventory UI

    /// <summary>
    /// Turns on/off inventory UI
    /// </summary>
    public void UpdateInventoryState()
    {
        if (inventoryCanvas.activeInHierarchy)
        {
            CloseInventoryUI();
        }
        else OpenInventoryUI();
    }

    private void OpenInventoryUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        inventoryCanvas.SetActive(true);
    }

    private void CloseInventoryUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inventoryCanvas.SetActive(false);
    }

    #endregion


    #region Settings UI
    public void UpdateSettingsState()
    {
        if(settingsCanvas.activeInHierarchy) CloseSettingsUI();
        else OpenSettingsUI();
    }

    private void OpenSettingsUI()
    {
        Debug.Log("setting");

        Cursor.lockState = CursorLockMode.Confined;
        settingsCanvas.SetActive(true);
    }

    private void CloseSettingsUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        settingsCanvas.SetActive(false);
    }

    #endregion
}
