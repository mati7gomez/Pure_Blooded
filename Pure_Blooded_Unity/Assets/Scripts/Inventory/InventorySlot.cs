using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Color _defaultColor;
    private void Awake()
    {
        _defaultColor = gameObject.GetComponent<Image>().color;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject itemDropped = eventData.pointerDrag;
            InventoryItem draggableItem = itemDropped.GetComponent<InventoryItem>();
            draggableItem.SetNewParent(transform);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            gameObject.GetComponent<Image>().color = Color.gray;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = _defaultColor;
    }
}
