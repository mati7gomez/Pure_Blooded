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
        _rectTransformAlignment = new Vector2(_rectTransform.rect.width / 2f, _rectTransform.rect.height / 2f);
    }
    private void Update()
    {
        if (_selectedGrid == null) return;
    }

    //----------------Implementacion interfaces----------------//

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<NewInventoryItem>() != null)
        {
            if (_selectedGrid != null)
            {
                Vector2Int tilePos = _selectedGrid.GetTileInGridPosition(_rectTransform, eventData.position, Vector2.zero);
                eventData.pointerDrag.GetComponent<NewInventoryItem>().SetNewParent(transform, GetNewPosition(tilePos));
            }

        }
    }

    //-----------Metodos de la clase---------------------//

    //Ver despues si quitamos las varibles declaradas en este metodo y las ponemos dentro de la clase, para optimizar el uso del garbage collector de c# xd
    private Vector3 GetNewPosition(Vector2Int tilePos)
    {
        int tileX = tilePos.x; //Valor x del tile donde se clickeo ej: (0,0) (1,0)
        int tileY = tilePos.y; //Valor y del tile donde se clickeo ej: (0,0) (0,1)

        float x = 0f; // valor en x a mover
        float y = 0f; // valor en y a mover

        int gridWidthTiles = _selectedGrid.GetGridWidth(); //El ancho de tiles de la grilla en ints
        int gridHeightTiles = _selectedGrid.GetGridHeight(); //El alto de tiles de la grilla en ints

        if (gridWidthTiles > 1 && gridHeightTiles > 1) //Si la grilla es igual o mayor a 2x2 se puede interactuar
        {
            MoveItemPosition(tileX, _selectedGrid.GetTileWidthSize(), out x);
            MoveItemPosition(tileY, _selectedGrid.GetTileHeightSize(), out y);
        }
        //Debug.Log($"TilePos: ({tileX},{tileY}) - TileMovement: ({x},{y})");
        return new Vector2(x, y);
    }
    private void MoveItemPosition(int tilePos, float tileSize, out float i)
    {
        i = tileSize / 2f; //Inicialmente movemos la ubicacion del item en el inventario la mitad del tamaño del tile para que quede en el centro del mismo
        if (tilePos != 0) //Si la posicion del tile esta en 0, no debemos mover la ubicacion del item, ya que anteriormente lo ubicamos en el centro del tile 0, pero si no es 0...
        {
            for (int j = 0; j < tilePos; j++) //Por cada tile menor a la posicion del tile que queremos colocar el item, movemos el mismo el tamaño del tile
            {
                i += tileSize;
            }
        }
    }
    //Por ahora este codigo de abajo ya no sirve, se resume con el metodo de arriba nomas
    //Yo creo que este metodo se puede achicar, o subdivir en varios metodos para que sea mas legible, pero por ahora me da paja
    //private void MoveItemPosition3(int tilePos , int tileMidPos, int tilesQuantity, float tileSize, out float i)
    //{
    //    i = 0;
    //    //Calculo para mover el objeto en pixeles hacia la izquierda o abajo
    //    if (tilePos < tileMidPos) //Si el tile donde se quiso interactuar se encuentra a la izquierda o debajo del centro de la grilla...  
    //    {
    //        if (tilesQuantity % 2 == 0) //Si la cantidad de tiles son pares
    //        {
    //            i -= tileSize / 2f; //Movemos el objeto la mitad del tamaño del Tile (por ahora, tamaño en pixeles)
    //        }
    //        else //Si no son pares...
    //        {
    //            i -= tileSize; //Movemos el objeto la cantidad del tamaño del Tile (por ahora, tamaño en pixeles)
    //        }
    //        for (int j = tilePos + 1; j < tileMidPos; j++) //Bucle que toma en cuenta la posicion en "x" o "y" de la interaccion y le suma 1, y suma i hasta que llegue a la misma posicion del tile del medio de "x" o "y"
    //                                                       //(le suma uno a tilePos porque la division para obtener el medio se hace con ints, por lo que 3 / 2 es igual a 1 y no 1.5,

    //        {
    //            i -= tileSize; //Por cada tile hacia la izquierda lo movemos el tamaño del tile
    //        }
    //    }
    //    else if (tilePos > tileMidPos) //Calculo para mover el objeto en pixeles hacia la derecha o arriba
    //    {
    //        if (tilesQuantity % 2 == 0) //Si la cantidad de tiles son pares
    //        {
    //            i += tileSize / 2f; //Movemos el objeto la mitad del tamaño del Tile (por ahora, tamaño en pixeles)
    //        }
    //        for (int j = tilePos; j > tileMidPos; j--) //Bucle que toma en cuenta la posicion en "x" o "y" de la interaccion, y suma i hasta que llegue a la misma posicion del tile del medio de "x" o "y"
    //        {
    //            i += tileSize;
    //        }
    //    }
    //    else if (tilePos == tileMidPos) //Calculo para mover el objeto en pixeles hacia la derecha si es que fue colocado en la misma columna que el centro de X
    //    {
    //        if (tilesQuantity % 2 == 0) //Si es par le sumamos unicamente la mitad del tamaño del tile, ya que si es impar, el objeto ya se encuentra en el centro por asi decir xd
    //        {
    //            i += tileSize / 2f;
    //        }
    //    }
    //}
}
