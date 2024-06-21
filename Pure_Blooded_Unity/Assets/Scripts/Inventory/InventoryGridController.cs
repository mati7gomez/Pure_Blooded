using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InventoryGridController : MonoBehaviour, IDropHandler
{
    private RectTransform _rectTransform; //Componente rect transform
    Grid _selectedGrid; //Referencia de la grilla seleccionada
    [SerializeField] GameObject _inventoryItemPrefab;
    public static InventoryGridController _instance;

    //------------Metodo que asegura que no se borre y que solo haya uno-------------//
    private void Awake(){
        if(InventoryGridController._instance == null){
            InventoryGridController._instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    //------------Metodos propios de MonoBehaviour-------------//
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _selectedGrid = GetComponent<Grid>();
    }

    //----------------Implementacion interfaces----------------//
    public void OnDrop(PointerEventData eventData) //Metodo que controla cuando un item se solto en la grilla del inventario
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Item droppeado");
            InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (droppedItem != null)
            {
                if (_selectedGrid != null)
                {
                    Vector2Int tilePos = _selectedGrid.GetTileInGrid(_rectTransform, eventData.position, Vector2.zero); //Obtenemos el tile donde se solto el item en la grilla del inventario
                    Grid droppedItemGrid = droppedItem.GetComponent<Grid>(); //Obtenemos el componente grid del item droppeado
                    if (CanItemBePlaced(tilePos, droppedItem.GetSelectedTile(), droppedItemGrid, (int)droppedItem.GetItemRotation()))
                    {
                        droppedItem.SetIsInHand(false);
                        droppedItem.SetNewParent(transform);
                        droppedItem.SetNewPivot();
                        droppedItem.SetNewPosition(GetNewPosition(tilePos));
                        droppedItem.SetNewRotation();
                        droppedItem.SetHasMovedToOtherPos(true);
                        SetInventoryOccupancyStateOnItemPicked(tilePos, droppedItem.GetSelectedTile(), droppedItemGrid, (int)droppedItem.GetItemRotation(), true);
                        Debug.Log("Item colocado correctamente en su nueva posicion");
                    }
                    else
                    {
                        Debug.Log("Error al colocar item en la nueva posicion, volviendo a la anterior");
                    }
                }
            }
        }
        
    }

    //-----------Metodos de la clase---------------------//

    //Metodos para calcular la nueva posicion del item en el inventario al ser colocado correctamente
    private Vector3 GetNewPosition(Vector2Int inventoryTile) //Metodo para obtener la nueva posicion del item al ser colocado correctamente en la grilla del inventario
    {
        float x = 0f; // valor en x a mover
        float y = 0f; // valor en y a mover

        int gridWidthTiles = _selectedGrid.GetGridWidth(); //El ancho de tiles de la grilla en ints
        int gridHeightTiles = _selectedGrid.GetGridHeight(); //El alto de tiles de la grilla en ints

        if (gridWidthTiles > 1 && gridHeightTiles > 1) //Si la grilla es igual o mayor a 2x2 se puede interactuar
        {
            CalculateNewPositionAxis(inventoryTile.x, _selectedGrid.GetTileWidthSize(), out x);
            CalculateNewPositionAxis(inventoryTile.y, _selectedGrid.GetTileHeightSize(), out y);
        }
        //Debug.Log($"TilePos: ({tileX},{tileY}) - TileMovement: ({x},{y})");
        return new Vector2(x, y);
    }
    private void CalculateNewPositionAxis(int tilePos, float tileSize, out float i) //Metodo para calcular la nueva posicion del item (en x o y)
    {
        i = tileSize / 2f; //Inicialmente movemos la ubicacion del item en el inventario la mitad del tama�o del tile para que quede en el centro del mismo
        for (int j = 0; j < tilePos; j++) //Por cada tile menor a la posicion del tile que queremos colocar el item, movemos el mismo el tama�o del tile
        {
            i += tileSize;
        }
    }

    public bool AddItem(ItemSO itemSO)
    {
        int invGridWidth = _selectedGrid.GetGridWidth();
        int invGridHeight = _selectedGrid.GetGridHeight();

        Vector2Int inventoryTile = new Vector2Int();
        Vector2Int defaultPivotTile = Vector2Int.zero;
        Grid itemGrid = new Grid(itemSO.GetItemSizeInInventory().x, itemSO.GetItemSizeInInventory().y);

        for (int itemRotation = 0; itemRotation <= 270; itemRotation += 90)
        {
            for (int i = 0; i < invGridWidth; i++)
            {
                inventoryTile.x = i;
                for (int j = 0; j < invGridHeight; j++)
                {
                    inventoryTile.y = j;
                    if (CanItemBePlaced(inventoryTile, defaultPivotTile, itemGrid, itemRotation))
                    {
                        SetInventoryOccupancyStateOnItemPicked(inventoryTile, defaultPivotTile, itemGrid, itemRotation, true);
                        Debug.Log("xxxxxxxxxxx");
                        PlaceNewItem(itemSO, itemRotation, GetNewPosition(inventoryTile));
                        Debug.Log("zzzzzzzzzzzz");
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool MoveItem(InventoryItem inventoryItem)
    {
        int invGridWidth = _selectedGrid.GetGridWidth();
        int invGridHeight = _selectedGrid.GetGridHeight();

        Vector2Int inventoryTile = new Vector2Int();
        Vector2Int defaultPivotTile = Vector2Int.zero;
        Grid itemGrid = new Grid(inventoryItem.GetItemSO().GetItemSizeInInventory().x, inventoryItem.GetItemSO().GetItemSizeInInventory().y);

        for (int itemRotation = 0; itemRotation <= 270; itemRotation += 90)
        {
            for (int i = 0; i < invGridWidth; i++)
            {
                inventoryTile.x = i;
                for (int j = 0; j < invGridHeight; j++)
                {
                    inventoryTile.y = j;
                    if (CanItemBePlaced(inventoryTile, defaultPivotTile, itemGrid, itemRotation))
                    {
                        SetInventoryOccupancyStateOnItemPicked(inventoryTile, defaultPivotTile, itemGrid, itemRotation, true);
                        Debug.Log("xxxxxxxxxxx");
                        PlaceMovedItem(inventoryItem, itemRotation, GetNewPosition(inventoryTile));
                        Debug.Log("zzzzzzzzzzzz");
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private bool CanItemBePlaced(Vector2Int inventoryTilePos, Vector2Int itemPivotTile , Grid itemGrid, int itemRot)
    {
        int maxW = _selectedGrid.GetGridWidth();
        int maxH = _selectedGrid.GetGridHeight();
        switch (itemRot)
        {
            case 0:
                if (!CheckInsideOfBounds(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH, itemRot)) return false;
                if (!CheckInventoryOccupancyState(inventoryTilePos, itemPivotTile, itemGrid, itemRot)) return false;
                break;

            case 90:
                if (!CheckInsideOfBounds(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH, itemRot)) return false;
                if (!CheckInventoryOccupancyState(inventoryTilePos, itemPivotTile, itemGrid, itemRot)) return false;
                break;

            case 180:
                if (!CheckInsideOfBounds(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH, itemRot)) return false;
                if (!CheckInventoryOccupancyState(inventoryTilePos, itemPivotTile, itemGrid, itemRot)) return false;
                break;

            case 270:
                if (!CheckInsideOfBounds(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH, itemRot)) return false;
                if (!CheckInventoryOccupancyState(inventoryTilePos, itemPivotTile, itemGrid, itemRot)) return false;
                break;
        }
        Debug.Log("Item se puede colocar");
        return true;
    }
    private void PlaceNewItem(ItemSO itemSO, int itemRotation, Vector3 itemPosition)
    {
        GameObject itemCreated = Instantiate(_inventoryItemPrefab, transform);
        itemCreated.GetComponent<InventoryItem>().SetItemVariablesOnCreated(itemSO, itemRotation, itemPosition);
    }
    private void PlaceMovedItem(InventoryItem inventoryItem, int itemRotation, Vector3 itemPosition)
    {
        inventoryItem.SetIsInHand(false);
        inventoryItem.SetHasMovedToOtherPos(true);
        inventoryItem.transform.SetParent(transform);
        inventoryItem.SetNewParent(transform);
        inventoryItem.SetItemVariablesOnCreated(inventoryItem.GetItemSO(), itemRotation, itemPosition);
    }


    //Metodos para calcular si el item se intento colocar dentro de los limites de la grilla del inventario
    private bool CheckInsideOfBounds(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int maxW, int maxH, int itemRot)
    {
        Vector2Int start = new Vector2Int();
        Vector2Int end = new Vector2Int();

        switch (itemRot)
        {
            case 0:
                start.x = inventoryTilePos.x - itemPivotTile.x;
                start.y = inventoryTilePos.y - itemPivotTile.y;
                end.x = start.x + itemGrid.GetGridWidth() - 1;
                end.y = start.y + itemGrid.GetGridHeight() - 1;
                break;
            case 90:
                start.x = inventoryTilePos.x - (itemGrid.GetGridHeight() - 1 - itemPivotTile.y);
                start.y = inventoryTilePos.y - itemPivotTile.x;
                end.x = start.x + itemGrid.GetGridHeight() - 1;
                end.y = start.y + itemGrid.GetGridWidth() - 1;
                break;
            case 180:
                start.x = inventoryTilePos.x - (itemGrid.GetGridWidth() - 1 - itemPivotTile.x);
                start.y = inventoryTilePos.y - (itemGrid.GetGridHeight() - 1 - itemPivotTile.y);
                end.x = start.x + itemGrid.GetGridWidth() - 1;
                end.y = start.y + itemGrid.GetGridHeight() - 1;
                break;
            case 270:
                start.x = inventoryTilePos.x - itemPivotTile.y;
                start.y = inventoryTilePos.y - (itemGrid.GetGridWidth() - 1 - itemPivotTile.x);
                end.x = start.x + itemGrid.GetGridHeight() - 1;
                end.y = start.y + itemGrid.GetGridWidth() - 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(itemRot), "Rotaci�n no v�lida");
        }

        return CheckBounds(start, end, maxW, maxH);
    }

    private bool CheckBounds(Vector2Int start, Vector2Int end, int maxW, int maxH)
    {
        if (start.x < 0 || end.x >= maxW || start.y < 0 || end.y >= maxH)
            return false;

        return true;
    }


    //Metodos para calcular si hay espacio en los tiles del inventario
    private bool CheckInventoryOccupancyState(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int itemRot)
    {
        Vector2Int start, end;
        CalculateInsideBounds(inventoryTilePos, itemPivotTile, itemGrid, itemRot, out start, out end);

        return CheckInventoryOccupancyState(start.x, start.y, end.x, end.y);
    }

    private void CalculateInsideBounds(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int itemRot, out Vector2Int start, out Vector2Int end)
    {
        switch (itemRot)
        {
            case 0:
                start = new Vector2Int(inventoryTilePos.x - itemPivotTile.x, inventoryTilePos.y - itemPivotTile.y);
                end = new Vector2Int(start.x + (itemGrid.GetGridWidth() - 1), start.y + (itemGrid.GetGridHeight() - 1));
                break;
            case 90:
                start = new Vector2Int(inventoryTilePos.x - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y), inventoryTilePos.y - itemPivotTile.x);
                end = new Vector2Int(start.x + (itemGrid.GetGridHeight() - 1), start.y + (itemGrid.GetGridWidth() - 1));
                break;
            case 180:
                start = new Vector2Int(inventoryTilePos.x - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x), inventoryTilePos.y - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y));
                end = new Vector2Int(start.x + (itemGrid.GetGridWidth() - 1), start.y + (itemGrid.GetGridHeight() - 1));
                break;
            case 270:
                start = new Vector2Int(inventoryTilePos.x - itemPivotTile.y, inventoryTilePos.y - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x));
                end = new Vector2Int(start.x + (itemGrid.GetGridHeight() - 1), start.y + (itemGrid.GetGridWidth() - 1));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(itemRot), "Rotaci�n no v�lida");
        }
    }

    private bool CheckInventoryOccupancyState(int startX, int startY, int endX, int endY)
    {
        for (int i = startX; i <= endX; i++)
        {
            for (int j = startY; j <= endY; j++)
            {
                if (_selectedGrid.GetTileOccupancyState(new Vector2Int(i, j)))
                    return false;
            }
        }
        return true;
    }


    //Metodos para establecer el estado de ocupacion de los tiles del inventario

    public void SetInventoryOccupancyStateOnDrag(Vector2 mousePos, Vector2Int itemPivotTile, Grid itemGrid, int itemRot, bool state)
    {
        int startX;
        int startY;
        int endX;
        int endY;
        Vector2Int inventoryTilePos = _selectedGrid.GetTileInGrid(_rectTransform, mousePos, Vector2.zero);
        switch (itemRot)
        {
            case 0:
                startX = inventoryTilePos.x - itemPivotTile.x;
                startY = inventoryTilePos.y - itemPivotTile.y;
                endX = startX + (itemGrid.GetGridWidth() - 1);
                endY = startY + (itemGrid.GetGridHeight() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;

            case 90:
                startX = inventoryTilePos.x - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y);
                startY = inventoryTilePos.y - itemPivotTile.x;
                endX = startX + (itemGrid.GetGridHeight() - 1);
                endY = startY + (itemGrid.GetGridWidth() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;

            case 180:
                startX = inventoryTilePos.x - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x);
                startY = inventoryTilePos.y - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y);
                endX = startX + (itemGrid.GetGridWidth() - 1);
                endY = startY + (itemGrid.GetGridHeight() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;

            case 270:
                startX = inventoryTilePos.x - itemPivotTile.y;
                startY = inventoryTilePos.y - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x);
                endX = startX + (itemGrid.GetGridHeight() - 1);
                endY = startY + (itemGrid.GetGridWidth() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;
        }
    }
    public void SetInventoryOccupancyStateOnItemPicked(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int itemRot, bool state)
    {
        int startX;
        int startY;
        int endX;
        int endY;
        switch (itemRot)
        {
            case 0:
                startX = inventoryTilePos.x - itemPivotTile.x;
                startY = inventoryTilePos.y - itemPivotTile.y;
                endX = startX + (itemGrid.GetGridWidth() - 1);
                endY = startY + (itemGrid.GetGridHeight() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;

            case 90:
                startX = inventoryTilePos.x - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y);
                startY = inventoryTilePos.y - itemPivotTile.x;
                endX = startX + (itemGrid.GetGridHeight() - 1);
                endY = startY + (itemGrid.GetGridWidth() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;

            case 180:
                startX = inventoryTilePos.x - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x);
                startY = inventoryTilePos.y - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y);
                endX = startX + (itemGrid.GetGridWidth() - 1);
                endY = startY + (itemGrid.GetGridHeight() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;

            case 270:
                startX = inventoryTilePos.x - itemPivotTile.y;
                startY = inventoryTilePos.y - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x);
                endX = startX + (itemGrid.GetGridHeight() - 1);
                endY = startY + (itemGrid.GetGridWidth() - 1);
                _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, state);
                break;
        }
    }

}
