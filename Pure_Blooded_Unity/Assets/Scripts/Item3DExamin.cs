using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item3DExamin : MonoBehaviour, IDragHandler
{
    private Camera _mainCamera;
    private GameObject _objectExaminated;

    //Encuentra el objeto que se quiere examinar y crea una instancia del mismo
    public GameObject Examine(ItemSO _itemSO){
        _mainCamera = Camera.main;
        
        if(_mainCamera !=null){
            Vector3 spawnPosition = _mainCamera.transform.position + _mainCamera.transform.forward * 0.5f;

            GameObject objetoPrefab = _itemSO.GetItemPrefab();
            _objectExaminated = Instantiate(objetoPrefab, spawnPosition, _mainCamera.transform.rotation);
        
        }
        return _objectExaminated;
    }

    //Recolecta los valores del mouse cuando se mantiene apretado el click izq.
    //Esos valore los traduce a la rotacion del objeto a examinar
    public void OnDrag(PointerEventData eventData){
        if(_objectExaminated != null){
            float rotationSpeed = 0.2f;

            float rotationX = eventData.delta.y * rotationSpeed;
            float rotationY = -eventData.delta.x * rotationSpeed;

            //La rotacion debe ser distinta para cada eje
            _objectExaminated.transform.Rotate(Vector3.up, rotationY, Space.World);
            _objectExaminated.transform.Rotate(Vector3.right, rotationX, Space.Self);
        }


    }
}
