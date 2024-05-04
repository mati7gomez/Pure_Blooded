using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        //_selectedGrid = eventData.pointerEnter.transform.GetComponent<Grid>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_selectedGrid = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<NewInventoryItem>() != null)
        {
            if (_selectedGrid != null)
            {
                Vector2Int tilePos = GetTileInGridPosition(eventData.position);
                //Debug.Log($"Posicion: {tilePos} -\n Movimiento en pixeles: {GetTileCoordsToPositionImage(tilePos)}");
                eventData.pointerDrag.GetComponent<NewInventoryItem>().SetNewParent(transform, GetTileCoordsToPositionImage(tilePos) + _rectTransformAlignment);
                
            }

        }
    }

    //-----------Metodos de la clase---------------------//

    private Vector2Int GetTileInGridPosition(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, position, null, out _localPointInGrid);
        return _selectedGrid.GetTileInGridPosition(_localPointInGrid);
    }

    private Vector3 GetTileCoordsToPositionImage(Vector2Int tilePos)
    {
        int tileX = tilePos.x; //Valor x del tile donde se clickeo ej: (0,0) (1,0)
        int tileY = tilePos.y; //Valor y del tile donde se clickeo ej: (0,0) (0,1)

        float x = 0f; // valor en x a mover
        float y = 0f; // valor en y a mover

        int gridWidthTiles = _selectedGrid.GetGridWidth(); //El ancho de tiles de la grilla en ints
        int gridHeightTiles = _selectedGrid.GetGridHeight(); //El alto de tiles de la grilla en ints
        //Hay que ver esto, ya que si tiene 3 de alto, por ej, el medio en ints seria 1, ver como arreglar eso
        int gridMidWidhtPos = gridWidthTiles / 2; //El medio de la grilla, el tile central en x
        int gridMidHeightPos = gridHeightTiles / 2; //El medio de la grilla, el tile central en y


        //Debug.Log($"Tile en y: {tileY} -\n Medio de Y: {gridMidHeightPos}");

        //Luego intentar achicar este mecanismo... tiene codigo repetido
        //A tener en cuenta: en una grilla 2x2 y 3x3 su centro es el mismo, es decir (1;1) ya que 2/2 es 1 y 3/2 es 1.5, pero solo se toma en cuenta la parte entera, es decir 1
        if (gridWidthTiles > 1 && gridHeightTiles > 1) //Si la grilla es mayor o igual a 2x2 se puede interactuar
        {
            MoveItemPosition(tileX, gridMidWidhtPos, gridWidthTiles, _selectedGrid.GetTileSizeWidht(), out x);
            MoveItemPosition(tileY, gridMidHeightPos, gridHeightTiles, _selectedGrid.GetTileSizeHeight(), out y);
        }
        //Debug.Log($"TilePos: ({tileX},{tileY}) - TileMovement: ({x},{y})");
        return new Vector2(x, y);
    }

    private void MoveItemPosition(int tilePos , int tileMidPos, int tilesQuantity, float tileSize, out float i)
    {
        i = 0;
        if (tilePos < tileMidPos) //Si el tile donde se quiso interactuar se encuentra a la izquierda del centro de la grilla...  
        {
            if (tilesQuantity % 2 == 0) //Si la cantidad de tiles horizontales son pares
            {
                i -= tileSize / 2f; //Movemos el objeto la mitad del tamaño del Tile (por ahora, tamaño en pixeles)
            }
            else //Si no son pares...
            {
                i -= tileSize; //Movemos el objeto la cantidad del tamaño del Tile (por ahora, tamaño en pixeles)
            }
            for (int j = tilePos + 1; j < tileMidPos; j++) //Bucle que toma en cuenta la posicion en x de la interaccion y le suma 1, y suma i hasta que llegue a la misma posicion del tile del medio de x
                                                           //(le suma uno porque la division para obtener el medio se hace con ints, por lo que 3 / 2 es igual a 1 y no 1.5,

            {
                i -= tileSize; //Por cada grilla hacia la izquierda lo movemos el tamaño de la grilla
            }
        }
        else if (tilePos > tileMidPos) //Calculo para mover el objeto en pixeles hacia la derecha
        {
            if (tilesQuantity % 2 == 0)
            {
                i += tileSize / 2f;
            }
            for (int j = tilePos; j > tileMidPos; j--)
            {
                i += tileSize;
            }
        }
        else if (tilePos == tileMidPos) //Calculo para mover el objeto en pixeles hacia la derecha si es que fue colocado en la misma columna que el centro de X
        {
            if (tilesQuantity % 2 == 0)
            {
                i += tileSize / 2f;
            }
        }
    }
}
