using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour, IDropHandler
{
    private RectTransform _rectTransform; //Componente rect transform
    [SerializeField] Grid _selectedGrid; //Referencia de la grilla seleccionada

    private Vector2 _localPointInGrid; //Posicion del mouse dentro de la grilla

    private Vector3 _rectTransformAlignment; //Vector que sirve para calcular la posicion de los items droppeados
                                             //dentro de la grilla del inventario

    //------------Metodos propios de MonoBehaviour-------------//
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    //----------------Implementacion interfaces----------------//
    public void OnDrop(PointerEventData eventData) //Metodo que controla cuando un item se solto en la grilla del inventario
    {
        NewInventoryItem droppedItem = eventData.pointerDrag.GetComponent<NewInventoryItem>();
        if (droppedItem != null)
        {
            if (_selectedGrid != null)
            {
                Vector2Int tilePos = _selectedGrid.GetTileInGrid(_rectTransform, eventData.position, Vector2.zero);
                Grid droppedItemGrid = droppedItem.GetComponent<Grid>();
                if (CanItemBePlaced(tilePos, droppedItem.GetSelectedTile(), droppedItemGrid, droppedItem.GetItemRotationDir()))
                {
                    droppedItem.SetNewPivot();
                    droppedItem.SetNewPosition(GetNewPosition(tilePos));
                    droppedItem.SetNewRotation();
                    
                }
                else
                {
                    Debug.Log("Mal colocado");
                }
            }
        }
    }

    //-----------Metodos de la clase---------------------//

    private Vector3 GetNewPosition(Vector2Int tilePos) //Metodo para obtener la nueva posicion del item al ser colocado correctamente en la grilla del inventario
    {
        float x = 0f; // valor en x a mover
        float y = 0f; // valor en y a mover

        int gridWidthTiles = _selectedGrid.GetGridWidth(); //El ancho de tiles de la grilla en ints
        int gridHeightTiles = _selectedGrid.GetGridHeight(); //El alto de tiles de la grilla en ints

        if (gridWidthTiles > 1 && gridHeightTiles > 1) //Si la grilla es igual o mayor a 2x2 se puede interactuar
        {
            GetNewPositionAxis(tilePos.x, _selectedGrid.GetTileWidthSize(), out x);
            GetNewPositionAxis(tilePos.y, _selectedGrid.GetTileHeightSize(), out y);
        }
        //Debug.Log($"TilePos: ({tileX},{tileY}) - TileMovement: ({x},{y})");
        return new Vector2(x, y);
    }
    private void GetNewPositionAxis(int tilePos, float tileSize, out float i) //Metodo para obtener la nueva posicion del item (en x o y)
    {
        i = tileSize / 2f; //Inicialmente movemos la ubicacion del item en el inventario la mitad del tamaño del tile para que quede en el centro del mismo
        for (int j = 0; j < tilePos; j++) //Por cada tile menor a la posicion del tile que queremos colocar el item, movemos el mismo el tamaño del tile
        {
            i += tileSize;
        }
    }

    private bool CanItemBePlaced(Vector2Int inventoryTilePos, Vector2Int itemPivotTile ,Grid itemGrid, int itemRot)
    {
        int maxW = _selectedGrid.GetGridWidth();
        int maxH = _selectedGrid.GetGridHeight();
        switch (itemRot)
        {
            case 0:
                //Primero comprobamos si se intento colocar dentro de la grilla
                if (!CheckOutOfBoundsRot0(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH)) return false;
                //Si esta dentro de los bounds de la grilla, checkeamos la disponibilidad de las casillas dentro de la misma
                if (!CheckInventoryOccupancyRot0(inventoryTilePos, itemPivotTile, itemGrid)) return false;
                break;

            case 90:
                //Primero comprobamos si se intento colocar dentro de la grilla
                if (!CheckOutOfBoundsRot90(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH)) return false;
                //Si esta dentro de los bounds de la grilla, checkeamos la disponibilidad de las casillas dentro de la misma

                break;

            case 180:
                //Primero comprobamos si se intento colocar dentro de la grilla
                if (!CheckOutOfBoundsRot180(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH)) return false;
                //Si esta dentro de los bounds de la grilla, checkeamos la disponibilidad de las casillas dentro de la misma

                break;

            case 270:
                //Primero comprobamos si se intento colocar dentro de la grilla
                if (!CheckOutOfBoundsRot270(inventoryTilePos, itemPivotTile, itemGrid, maxW, maxH)) return false;
                //Si esta dentro de los bounds de la grilla, checkeamos la disponibilidad de las casillas dentro de la misma

                break;
        }
        Debug.Log("Item se puede colocar");
        return true;
    }

    private bool CheckOutOfBoundsRot0(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int maxW, int maxH)
    {
        if (inventoryTilePos.x + ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x) < maxW) /*Debug.Log("Rot: 0 - Inside x right")*/;
        else /*Debug.Log("Rot: 0 - Outside x right")*/return false;

        if (inventoryTilePos.x - itemPivotTile.x >= 0) /*Debug.Log("Rot: 0 - Inside x left")*/;
        else /*Debug.Log("Rot: 0 - Outside x left") */return false;

        if (inventoryTilePos.y + ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y) < maxH) /*Debug.Log("Rot: 0 - Inside x up")*/;
        else /*Debug.Log("Rot: 0 - Outside x up") */return false;

        if (inventoryTilePos.y - itemPivotTile.y >= 0) /*Debug.Log("Rot: 0 - Inside x down")*/;
        else /*Debug.Log("Rot: 0 - Outside x down") */return false;

        return true;
    }
    private bool CheckInventoryOccupancyRot0(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid)
    {
        int startX = inventoryTilePos.x - itemPivotTile.x;
        int startY = inventoryTilePos.y - itemPivotTile.y;
        int endX = startX + (itemGrid.GetGridWidth() - 1);
        int endY = startY + (itemGrid.GetGridHeight() - 1);
        //Debug.Log($"Start: ({startX},{startY}");
        //Debug.Log($"End: ({endX},{endY}");
        for (int i = startX; i <= endX; i++)
        {
            for (int j = startY; j <= endY; j++)
            {
                Debug.Log("ayaya");
                if (_selectedGrid.GetTileOccupancyState(new Vector2Int(i, j))) return false;
                
            }
        }
        _selectedGrid.SetTilesOccupancyState(startX, startY, endX, endY, true);
        return true;
    }

    private bool CheckOutOfBoundsRot90(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int maxW, int maxH)
    {
        if (inventoryTilePos.y + ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x) < maxH) /*Debug.Log("Rot: 90 - Inside x right")*/;
        else /*Debug.Log("Rot: 90 - Outside x right") */return false;

        if (inventoryTilePos.y - itemPivotTile.x >= 0) /*Debug.Log("Rot: 90 - Inside x left")*/;
        else /*Debug.Log("Rot: 90 - Outside x left") */return false;

        if (inventoryTilePos.x - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y) >= 0) /*Debug.Log("Rot: 90 - Inside x up")*/;
        else /*Debug.Log("Rot: 90 - Outside x up") */return false;

        if (inventoryTilePos.x + itemPivotTile.y < maxW) /*Debug.Log("Rot: 90 - Inside x down")*/;
        else /*Debug.Log("Rot: 90 - Outside x down") */return false;

        return true;
    }

    private bool CheckOutOfBoundsRot180(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int maxW, int maxH)
    {
        if (inventoryTilePos.x - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x) >= 0) /*Debug.Log("Rot: 180 - Inside x right")*/;
        else /*Debug.Log("Rot: 180 - Outside x right") */return false;

        if (inventoryTilePos.x + itemPivotTile.x < maxW) /*Debug.Log("Rot: 180 - Inside x left")*/;
        else /*Debug.Log("Rot: 180 - Outside x left") */return false;

        if (inventoryTilePos.y - ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y) >= 0) /*Debug.Log("Rot: 180 - Inside x up")*/;
        else /*Debug.Log("Rot: 180 - Outside x up") */return false;

        if (inventoryTilePos.y + itemPivotTile.y < maxH) /*Debug.Log("Rot: 180 - Inside x down")*/;
        else /*Debug.Log("Rot: 180 - Outside x down") */return false;

        return true;
    }

    private bool CheckOutOfBoundsRot270(Vector2Int inventoryTilePos, Vector2Int itemPivotTile, Grid itemGrid, int maxW, int maxH)
    {
        if (inventoryTilePos.y - ((itemGrid.GetGridWidth() - 1) - itemPivotTile.x) >= 0) /*Debug.Log("Rot: 270 - Inside x right")*/;
        else /*Debug.Log("Rot: 270 - Outside x right") */return false;

        if (inventoryTilePos.y + itemPivotTile.x < maxH) /*Debug.Log("Rot: 270 - Inside x left")*/;
        else /*Debug.Log("Rot: 270 - Outside x left") */return false;

        if (inventoryTilePos.x + ((itemGrid.GetGridHeight() - 1) - itemPivotTile.y) < maxW) /*Debug.Log("Rot: 270 - Inside x up")*/;
        else /*Debug.Log("Rot: 270 - Outside x up") */return false;

        if (inventoryTilePos.x - itemPivotTile.y >= 0) /*Debug.Log("Rot: 270 - Inside x down")*/;
        else /*Debug.Log("Rot: 270 - Outside x down") */return false;

        return true;
    }


    //Por ahora este codigo de abajo ya no sirve, se resume con el metodo de arriba nomas
    //Yo creo que este metodo se puede achicar, o subdivir en varios metodos para que sea mas legible, pero por ahora me da paja
    //private void MoveItemPosition3(int inventoryTilePos , int tileMidPos, int tilesQuantity, float tileSize, out float i)
    //{
    //    i = 0;
    //    //Calculo para mover el objeto en pixeles hacia la izquierda o abajo
    //    if (inventoryTilePos < tileMidPos) //Si el tile donde se quiso interactuar se encuentra a la izquierda o debajo del centro de la grilla...  
    //    {
    //        if (tilesQuantity % 2 == 0) //Si la cantidad de tiles son pares
    //        {
    //            i -= tileSize / 2f; //Movemos el objeto la mitad del tamaño del Tile (por ahora, tamaño en pixeles)
    //        }
    //        else //Si no son pares...
    //        {
    //            i -= tileSize; //Movemos el objeto la cantidad del tamaño del Tile (por ahora, tamaño en pixeles)
    //        }
    //        for (int j = inventoryTilePos + 1; j < tileMidPos; j++) //Bucle que toma en cuenta la posicion en "x" o "y" de la interaccion y le suma 1, y suma i hasta que llegue a la misma posicion del tile del medio de "x" o "y"
    //                                                       //(le suma uno a inventoryTilePos porque la division para obtener el medio se hace con ints, por lo que 3 / 2 es igual a 1 y no 1.5,

    //        {
    //            i -= tileSize; //Por cada tile hacia la izquierda lo movemos el tamaño del tile
    //        }
    //    }
    //    else if (inventoryTilePos > tileMidPos) //Calculo para mover el objeto en pixeles hacia la derecha o arriba
    //    {
    //        if (tilesQuantity % 2 == 0) //Si la cantidad de tiles son pares
    //        {
    //            i += tileSize / 2f; //Movemos el objeto la mitad del tamaño del Tile (por ahora, tamaño en pixeles)
    //        }
    //        for (int j = inventoryTilePos; j > tileMidPos; j--) //Bucle que toma en cuenta la posicion en "x" o "y" de la interaccion, y suma i hasta que llegue a la misma posicion del tile del medio de "x" o "y"
    //        {
    //            i += tileSize;
    //        }
    //    }
    //    else if (inventoryTilePos == tileMidPos) //Calculo para mover el objeto en pixeles hacia la derecha si es que fue colocado en la misma columna que el centro de X
    //    {
    //        if (tilesQuantity % 2 == 0) //Si es par le sumamos unicamente la mitad del tamaño del tile, ya que si es impar, el objeto ya se encuentra en el centro por asi decir xd
    //        {
    //            i += tileSize / 2f;
    //        }
    //    }
    //}
}
