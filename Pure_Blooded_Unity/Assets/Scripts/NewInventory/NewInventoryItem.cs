using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    //Variable static para controlar el abrir y cerrar del menu
    static bool _anyItemIsBeingDragged;

    [SerializeField] private ItemSO _itemSO;
    private Grid _itemGrid;
    private RectTransform _rectTransform;
    private Vector2 _localPointInGrid;
    private Vector2Int _lastTileSelected;
    private Vector2 _wantedPivot;

    
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
    //----------------Implementacion interfaces----------------//
    #region Interfaces
    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform.pivot = _wantedPivot;
        _image.raycastTarget = false;
        _lastParent = transform.parent;
        transform.SetParent(transform.root.GetChild(0));
        transform.SetAsLastSibling();
        _anyItemIsBeingDragged = true;
        _isBeingDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;
        transform.SetParent(_lastParent);
        _anyItemIsBeingDragged = false;
        _isBeingDragged = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _lastTileSelected = GetTileInGridPosition(Input.mousePosition);
        SetItemPivot(_lastTileSelected, ref _wantedPivot);
    }
    #endregion

    public void SetNewParent(Transform newParentTransform, Vector3 newPos)
    {
        _lastParent = newParentTransform;
        transform.SetParent(_lastParent);
        transform.localPosition = newPos;
        Debug.Log("Deberia estar en: " + newPos);
    }

    private void SetItemPivot(Vector2Int tilePos, ref Vector2 wantedPivot)
    {
        int wTiles = (int)(_rectTransform.rect.width / _itemGrid.GetTileWidthSize());
        float xPivot = 1f / wTiles;
        xPivot = xPivot / 2;
        for (int i = 0; i < tilePos.x; i++)
        {
            xPivot += 1f / wTiles;
        }

        int hTiles = (int)(_rectTransform.rect.height / _itemGrid.GetTileHeightSize());
        float yPivot = 1f / hTiles;
        yPivot = yPivot / 2;
        for (int i = 0; i < tilePos.y; i++)
        {
            yPivot += 1f / hTiles;
        }
        Debug.Log($"Pivot deseado: {xPivot},{yPivot}");
        wantedPivot = new Vector2(xPivot, yPivot);
    }
    private Vector2Int GetTileInGridPosition(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, position, null, out _localPointInGrid);
        Vector2 pivotOffset = new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
        Vector2 adjustedLocalPoint = _localPointInGrid + pivotOffset;
        return _itemGrid.GetTileInGridPosition(adjustedLocalPoint);
    }
    private Vector2 GetPivotOffset(ref Vector2 pivotOffset)
    {
        pivotOffset = new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
        return pivotOffset;
    }

    
}
