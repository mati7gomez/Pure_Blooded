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
            Vector3 inspectOffSet = _itemSO.GetItemInspectOffSet(); //Eje X e Y
            Quaternion inspectRotation = _itemSO.GetItemInspectRotation(); //Rotacion del objeto
            Vector3 cameraPosition = _mainCamera.transform.position;
            Vector3 forwardPosition = _mainCamera.transform.forward;
            
            Vector3 spawnPosition = cameraPosition + forwardPosition + new Vector3(inspectOffSet.x, inspectOffSet.y, 0);
            
            Quaternion spawnRotation = _mainCamera.transform.rotation * inspectRotation;

            GameObject objetoPrefab = _itemSO.GetItemPrefab();
            _objectExaminated = Instantiate(objetoPrefab, spawnPosition, spawnRotation);
        
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
