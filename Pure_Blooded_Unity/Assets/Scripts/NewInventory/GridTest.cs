using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform _rectTransform;
    [SerializeField] Grid selectedItemGrid;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedItemGrid = eventData.pointerEnter.transform.GetComponent<Grid>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedItemGrid = null;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (selectedItemGrid == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, null, out localPoint);
        Debug.Log(localPoint);
        //Debug.Log(Input.mousePosition);
        //Debug.Log(Input.mousePosition.x / Screen.width + ";" + Input.mousePosition.y / Screen.height);
        Debug.Log(selectedItemGrid.GetTileGridPosition(localPoint));
    }
}
