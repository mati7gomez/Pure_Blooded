using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static bool _anyItemIsBeingDragged;

    [SerializeField] private ItemSO _itemSO;
    private bool _isBeingDragged;
    private Transform _lastParent;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.raycastTarget = false;
        _lastParent = transform.parent;
        transform.SetParent(transform.root.GetChild(0));
        transform.SetAsLastSibling();
        //Debug.Log("Comienza el drag");
        _anyItemIsBeingDragged = true;
        _isBeingDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        //Debug.Log("Draggeando");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        transform.SetParent(_lastParent);
        //Debug.Log("Termina el drag");
        Debug.Log(transform.localPosition);
        _anyItemIsBeingDragged = false;
        _isBeingDragged = false;
    }

    public void SetNewParent(Transform newParentTransform, Vector3 newPos)
    {
        _lastParent = newParentTransform;
        transform.SetParent(_lastParent);
        transform.localPosition = newPos;
        Debug.Log("Deberia estar en: " + newPos);
    }
    public void SetNewLocation()
    {

    }

}
