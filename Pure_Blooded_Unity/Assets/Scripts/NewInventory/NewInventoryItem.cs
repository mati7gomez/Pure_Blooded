using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static bool _anyItemIsBeingDragged;

    private Grid _itemGrid;
    private RectTransform _rectTransform;
    private Vector2 _localPointInGrid;

    [SerializeField] private ItemSO _itemSO;
    private bool _isBeingDragged;
    private Transform _lastParent;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Start()
    {
        _itemGrid = GetComponent<Grid>();
        _rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(GetTileInGridPosition(eventData.position));
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

    private void SetItemPivot()
    {

    }
    private Vector2Int GetTileInGridPosition(Vector2 position)
    {
        Vector2 pivotOffset = new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, position, null, out _localPointInGrid);
        Vector2 adjustedLocalPoint = _localPointInGrid + pivotOffset;
        return _itemGrid.GetTileInGridPosition(adjustedLocalPoint);
    }
}
