using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Canvas _inventoryCanvas;
    private InventoryGridController _inventoryGridController;
    private void Start()
    {
        _inventoryCanvas = GetComponent<Canvas>();
        _inventoryGridController = GetComponentInChildren<InventoryGridController>();
        DisableInventoryCanvas();
    }

    public bool AddItem(ItemSO itemSO)
    {
        if (_inventoryGridController.TryToAddItem(itemSO))
        {
            
        }
        return true;
    }


    private void EnableInventoryCanvas() => _inventoryCanvas.enabled = true;
    private void DisableInventoryCanvas() => _inventoryCanvas.enabled = false;
}
