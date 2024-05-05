using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    static bool _anyItemIsBeingDragged;

    private Grid _itemGrid;
    private RectTransform _rectTransform;
    private Vector2 _localPointInGrid;
    private Vector2Int _lasTileSelected;
    private Vector2 _wantedPivot;

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

    private void SetItemPivot(Vector2Int tilePos)
    {
        if (tilePos == new Vector2Int(0, 0))
        {
            int wTiles = (int)(_rectTransform.rect.width / 180f);
            float xPivot = 1f / wTiles;
            xPivot = xPivot / 2;

            int hTiles = (int)(_rectTransform.rect.height / 180f);
            float yPivot = 1f / hTiles;
            yPivot = yPivot / 2;
            Debug.Log($"Pivot deseado: {xPivot},{yPivot}");
            _wantedPivot = new Vector2(xPivot, yPivot);

        }
        else if (tilePos == new Vector2Int(1,0))
        {
            int wTiles = (int)(_rectTransform.rect.width / 180f);
            float xPivot = 1f / wTiles;
            float addPivot = xPivot;
            xPivot = xPivot / 2 + addPivot;

            int hTiles = (int)(_rectTransform.rect.height / 180f);
            float yPivot = 1f / hTiles;
            yPivot = yPivot / 2;
            Debug.Log($"Pivot deseado: {xPivot},{yPivot}");
            _wantedPivot = new Vector2(xPivot, yPivot);
        }
    }
    private Vector2Int GetTileInGridPosition(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, position, null, out _localPointInGrid);
        Vector2 pivotOffset = new Vector2(_rectTransform.rect.width * _rectTransform.pivot.x, _rectTransform.rect.height * _rectTransform.pivot.y);
        Vector2 adjustedLocalPoint = _localPointInGrid + pivotOffset;
        return _itemGrid.GetTileInGridPosition(adjustedLocalPoint);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _lasTileSelected = GetTileInGridPosition(Input.mousePosition);
        SetItemPivot(_lasTileSelected);
    }
}
