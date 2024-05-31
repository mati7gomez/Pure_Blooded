using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IDropHandler
{



    //Implementacion de interfaces
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag);
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        Debug.Log(droppedItem);
        if (droppedItem != null && transform.childCount == 0)
        {
            droppedItem.SetSizeOnHand();
            droppedItem.SetIsInHand(true);
            droppedItem.SetHasMovedToOtherPos(true);
            Debug.Log("bbbbbbbbbbbbb");
            droppedItem.SetNewParent(transform);
            droppedItem.SetNewPivot(new Vector2(0.5f,0.5f));
            droppedItem.SetNewPosition(Vector2.zero);
            droppedItem.SetHandRotation();
        }
    }
}
