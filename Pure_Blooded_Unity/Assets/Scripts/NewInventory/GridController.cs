using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rectTransform;
    [SerializeField] Grid selectedGrid;

    private Vector2 localPoint;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedGrid = eventData.pointerEnter.transform.GetComponent<Grid>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedGrid = null;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (selectedGrid == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, null, out localPoint);
        Debug.Log(selectedGrid.GetTileGridPosition(localPoint));
    }
}
