using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //  Variable static para saber si alguno de los items del inventario esta siendo arrastrado (esto sirve para
    //que no se pueda cerrar el inventario hasta soltar el item, y no generar bugs)
    private static bool _dragging; 

    private Transform _parentAfterDrag;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragging = true;
        Debug.Log("Comienza el drag");
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
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
        _image.raycastTarget = true;
        _dragging = false;
    }

    public void SetNewParent(Transform newParentTransform)
    {
        _parentAfterDrag = newParentTransform;
    }

}
