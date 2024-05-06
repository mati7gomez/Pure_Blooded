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
    private void Update()
    {
        if (_selectedGrid == null) return;
    }

    //----------------Implementacion interfaces----------------//
    public void OnDrop(PointerEventData eventData)
    {
        NewInventoryItem droppedItem = eventData.pointerDrag.GetComponent<NewInventoryItem>();
        if (droppedItem != null)
        {
            if (_selectedGrid != null)
            {
                Vector2Int tilePos = _selectedGrid.GetTileInGrid(_rectTransform, eventData.position, Vector2.zero);
                Grid droppedItemGrid = droppedItem.GetComponent<Grid>();
                if (CanItemBePlaced(tilePos, droppedItem.GetSelectedTile(), droppedItemGrid))
                {
                    droppedItem.SetNewParent(GetNewPosition(tilePos));
                }
            }
        }
    }

    //-----------Metodos de la clase---------------------//

    //Ver despues si quitamos las varibles declaradas en este metodo y las ponemos dentro de la clase, para optimizar el uso del garbage collector de c# xd
    private Vector3 GetNewPosition(Vector2Int tilePos)
    {
        float x = 0f; // valor en x a mover
        float y = 0f; // valor en y a mover

        int gridWidthTiles = _selectedGrid.GetGridWidth(); //El ancho de tiles de la grilla en ints
        int gridHeightTiles = _selectedGrid.GetGridHeight(); //El alto de tiles de la grilla en ints

        if (gridWidthTiles > 1 && gridHeightTiles > 1) //Si la grilla es igual o mayor a 2x2 se puede interactuar
        {
            MoveItemPosition(tilePos.x, _selectedGrid.GetTileWidthSize(), out x);
            MoveItemPosition(tilePos.y, _selectedGrid.GetTileHeightSize(), out y);
        }
        //Debug.Log($"TilePos: ({tileX},{tileY}) - TileMovement: ({x},{y})");
        return new Vector2(x, y);
    }
    private void MoveItemPosition(int tilePos, float tileSize, out float i)
    {
        i = tileSize / 2f; //Inicialmente movemos la ubicacion del item en el inventario la mitad del tamaño del tile para que quede en el centro del mismo
        for (int j = 0; j < tilePos; j++) //Por cada tile menor a la posicion del tile que queremos colocar el item, movemos el mismo el tamaño del tile
        {
            i += tileSize;
        }
    }

    private bool CanItemBePlaced(Vector2Int inventoryTilePos, Vector2Int itemPivotTile ,Grid itemGrid)
    {
        int pivotX = itemPivotTile.x;
        int pivotY = itemPivotTile.y;
        int itemGridWidth = itemGrid.GetGridWidth();
        int itemGridHeight = itemGrid.GetGridHeight();

        int xOutOfBoundsRight = inventoryTilePos.x + ((itemGridWidth - 1) - pivotX);
        int xOutOfBoundsLeft = inventoryTilePos.x - pivotX;

        int yOutOfBoundsUp = inventoryTilePos.y;
        int yOutOfBoundsBottom = inventoryTilePos.y - pivotY;

        Debug.Log($"x: {xOutOfBoundsLeft}");
        Debug.Log($"y: {yOutOfBoundsBottom}");
        //for (int i = inventoryTilePos.x - pivotX; i < itemGridWidth; i++)
        //{
        //    for (int j = inventoryTilePos.y - pivotY; j < itemGridHeight; j++)
        //    {
        //        if (_selectedGrid.GetTileOccupancyState(new Vector2Int(i, j)))
        //        {
        //            Debug.Log("No puede ser colocado");
        //            return false;
        //        }
        //    }
        //}
        //Debug.Log("Puede ser colocado");

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
