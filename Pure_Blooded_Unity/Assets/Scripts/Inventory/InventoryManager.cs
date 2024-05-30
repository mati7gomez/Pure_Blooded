using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Canvas _inventoryCanvas;
    private InventoryGridController _inventoryGridController;
    private void Start()
    {
        _inventoryCanvas = transform.GetChild(0).GetComponent<Canvas>();
        _inventoryGridController = GetComponentInChildren<InventoryGridController>();
        DisableInventoryCanvas();
    }
    private void Update()
    {
        if (!InventoryItem.GetAnyItemIsBeingDragged() && Input.GetButtonDown("Inventory"))
        {
            ToggleInventoryCanvas();
        }
    }

    public bool CanAddItem(ItemSO itemSO)
    {
        if (_inventoryGridController.AddItem(itemSO))
        {
            Debug.Log("Item agregado");
            return true;
        }
        else Debug.Log("Item no se pudo agregar");
        return false;
    }

    private void ToggleInventoryCanvas()
    {
        if (_inventoryCanvas.enabled) DisableInventoryCanvas();
        else if (!_inventoryCanvas.enabled) EnableInventoryCanvas();
    }
    private void EnableInventoryCanvas() => _inventoryCanvas.enabled = true;
    private void DisableInventoryCanvas() => _inventoryCanvas.enabled = false;
}
