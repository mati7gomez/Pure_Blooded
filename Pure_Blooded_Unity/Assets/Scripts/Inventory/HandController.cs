using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IDropHandler
{
    //Implementacion de interfaces
    InventoryGridController _inventoryGridController;

    private void Start()
    {
        _inventoryGridController = GameObject.Find("Inventario").GetComponent<InventoryGridController>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleHandDrop(eventData);
        }
    }

    private void HandleHandDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem != null && transform.childCount == 0)
        {
            droppedItem.SetIsInHand(true);
            droppedItem.SetSizeOnHand();
            droppedItem.SetHasMovedToOtherPos(true);
            droppedItem.SetNewParent(transform);
            droppedItem.SetNewPivot(new Vector2(0.5f, 0.5f));
            droppedItem.SetNewPosition(Vector2.zero);
            droppedItem.SetHandRotation();
        }
        else if (droppedItem != null && transform.childCount == 1)
        {
            if (_inventoryGridController.MoveItem(transform.GetChild(0).gameObject.GetComponent<InventoryItem>()))
            {
                droppedItem.SetIsInHand(true);
                droppedItem.SetSizeOnHand();
                droppedItem.SetHasMovedToOtherPos(true);
                droppedItem.SetNewParent(transform);
                droppedItem.SetNewPivot(new Vector2(0.5f, 0.5f));
                droppedItem.SetNewPosition(Vector2.zero);
                droppedItem.SetHandRotation();
            }
            
        }
    }
}
