using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item3DExamin : MonoBehaviour, IDragHandler
{

    private bool _isExaminating = false;

    private GameObject _objectExaminated;

    private CinemachineBrain _mainCamera;

    public GameObject Examine(ItemSO _itemSO){
        //Solo hay uno, que pertenece al Main Camera
        _mainCamera = FindObjectOfType<CinemachineBrain>();

        if(_mainCamera != null){
            UnityEngine.Vector3 spawnPosition = _mainCamera.transform.position + _mainCamera.transform.forward * 0.5f;
        
            GameObject objetoPrefab = _itemSO.GetItemPrefab();

            _objectExaminated = Instantiate(objetoPrefab, spawnPosition, _mainCamera.transform.rotation);
        } 

        Debug.Log(_objectExaminated.name);
        return _objectExaminated;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //_objectExaminated.transform.eulerAngles = new Vector3(-eventData.delta.y * 0.1f, -eventData.delta.x * 0.1f);
        float rotationSpeed = 0.2f;

            // Aplicar una rotación incremental
            float rotX = eventData.delta.y * rotationSpeed;
            float rotY = -eventData.delta.x * rotationSpeed;

            // Rotar el objeto alrededor de los ejes Y y X
            _objectExaminated.transform.Rotate(Vector3.up, rotY, Space.World);
            _objectExaminated.transform.Rotate(Vector3.right, rotX, Space.Self);
    }
}
