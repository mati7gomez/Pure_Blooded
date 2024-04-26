using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //  Variable static para saber si alguno de los items del inventario esta siendo arrastrado (esto sirve para
    //que no se pueda cerrar el inventario hasta soltar el item, y no generar bugs)
    private static bool _dragging;

    private string _itemName;
    private string _itemDescription;
    private Image _itemImage;
    

    private Transform _parentAfterDrag;

    private void Awake()
    {
        _itemImage = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragging = true;
        Debug.Log("Comienza el drag");
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _itemImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Draggeando");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Termina el drag");
        transform.SetParent(_parentAfterDrag);
        _itemImage.raycastTarget = true;
        _dragging = false;
    }

    public void SetItemAttributes(ItemSO itemSO)
    {
        SetItemName(itemSO);
        SetItemDescription(itemSO);
        SetItemImage(itemSO);
    }
    private void SetItemName(ItemSO itemSO)
    {
        _itemName = itemSO.GetItemName();
    }
    private void SetItemDescription(ItemSO itemSO)
    {
        _itemDescription = itemSO.GetItemDescription();
    }
    private void SetItemImage(ItemSO itemSO)
    {
        _itemImage = GetComponent<Image>();
        _itemImage.sprite = itemSO.GetItemImage();
    }
    public void SetNewParent(Transform newParentTransform)
    {
        _parentAfterDrag = newParentTransform;
    }
    public string GetItemName()
    {
        return _itemName;
    }

}
