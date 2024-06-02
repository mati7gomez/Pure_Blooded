using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Componentes y referencias fuera del player
    private CharacterController _controller;
    private Camera _mainCamera;
    private Transform _playerBody;
    private PlayerStats _playerStats;
    private GameObject _itemUbication;

    //Inputs
    private float _horInput;
    private float _verInput;
    private bool _runPressed;
    

    //Creo q hay q borrar este
    public bool CanMove = true;
    private bool CanRun = true;
    

    //Player extras
    private float _playerSpeed;
    [SerializeField] private float _playerNormalSpeed;
    [SerializeField] private float _playerRunSpeed;

    //Variables para calcular la rotacion del jugador
    private float _targetAngle;
    private float _finalAngle;
    private float _turnSmoothVelocity;

    //Variables para calcular la direccion de movimiento
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _camForwardTemporal;
    private Vector3 _camRightTemporal;
    private Vector3 _playerDir;
    public Vector3 GetPlayerDir => _playerDir;
    private Vector3 _moveDir;

    //Variables para calcular la gravedad
    private float _initialGravity = -9.8f;
    [SerializeField] private float _gravityMultiplier;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _playerBody = transform.GetChild(0).transform;
        _playerStats = new PlayerStats();

        _playerSpeed = _playerNormalSpeed;

        _itemUbication = FindChildByName(this.gameObject, "itemUbication");
    }

    private void Update()
    {
        HandleInputs();
        Debug.DrawRay(transform.position - new Vector3(0f,_controller.height / 2f,0f), _moveDir, Color.red);
    }

    private void FixedUpdate()
    {
        GetCamVectors();
        SetPlayerMoveDir();
        HandlePlayerGravity();
        if (CanMove)
        {
            MovePlayer();
            RotatePlayer();
        }
        

        HandleInputs(true);
    }
    private void GetCamVectors()
    {
        _camForward = _mainCamera.transform.forward;
        _camRight = _mainCamera.transform.right;

        _camForward.y = 0;
        _camRight.y = 0;

        _camForward.Normalize();
        _camRight.Normalize();
    }
    private void SetPlayerMoveDir()
    {
        if (_horInput == 0)
            {
                // Bloquea el componente vertical del movimiento horizontal para que este no afecte la direccion general, solo
                // la horizontal
                _camRightTemporal = Vector3.ProjectOnPlane(_camRight, Vector3.up);
            }

        if (_verInput == 0)
            {
                // Bloquea el componente horizontal del movimiento horizontal para que este no afecte la direccion general, solo
                // la vertical
                _camForwardTemporal = Vector3.ProjectOnPlane(_camForward, Vector3.up);
            }
        
        _playerDir = _camForwardTemporal * _verInput + _camRightTemporal * _horInput;

        _playerDir = Vector3.ClampMagnitude(_playerDir, 1f);
    }
    private void SetPlayerSpeed()
    {
        if (_runPressed && !_playerStats.IsRunning && CanRun)
        {
            _playerStats.SetRunningState(true);
            _playerSpeed = _playerRunSpeed;
        }
        else if (!_runPressed && _playerStats.IsRunning)
        {
            _playerStats.SetRunningState(false);
            _playerSpeed = _playerNormalSpeed;
        }
    }
    private void MovePlayer()
    {
        SetPlayerSpeed();
        _moveDir.x = _playerDir.x * _playerSpeed;
        _moveDir.z = _playerDir.z * _playerSpeed;
        if (_playerDir.magnitude != 0) _controller.Move(_playerBody.forward * _playerSpeed * Time.fixedDeltaTime);

        //_controller.Move(_moveDir * Time.fixedDeltaTime);
    }
    private void RotatePlayer()
    {
        if (_playerDir.x != 0f || _playerDir.z != 0f)
        {
            _targetAngle = Mathf.Atan2(_playerDir.x, _playerDir.z) * Mathf.Rad2Deg;
            _finalAngle = Mathf.SmoothDampAngle(_playerBody.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.2f);
            _playerBody.rotation = Quaternion.Euler(0f, _finalAngle, 0f);
        }
        //if (_playerDir.magnitude != 0)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(_playerDir, Vector3.up);
        //    _playerBody.rotation = Quaternion.RotateTowards(_playerBody.rotation, toRotation, 500f * Time.fixedDeltaTime);
        //}

    }
    private void HandlePlayerGravity()
    {
        if (_controller.isGrounded)
        {
            _moveDir.y = -1f;
        }
        else
        {
            _moveDir.y += _initialGravity * _gravityMultiplier * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("NoCorrer")){
            CanRun = false;
        }

    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("NoCorrer")){
            CanRun = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
            
        
        //float hitAngle = Vector3.Angle(hit.normal, Vector3.up);
        //if (hitAngle > _controller.slopeLimit)
        //{
        //    Debug.DrawRay(hit.point, hit.normal.normalized, Color.red, 15f);
        //    Debug.Log("Jugador debe deslizar");
        //}
        //else Debug.DrawRay(hit.point, hit.normal.normalized, Color.cyan, 15f);
    }

    //public void DisableMovement()
    //{
    //    CanMove = false;
    //    _horInput = 0;
    //    _verInput = 0;
    //    _playerDir = Vector3.zero;
    //    //Debug.Log("ayayaya q capo q soy");
    //    Invoke("ActivateMovement", 0.1f);
    //}
    //private void ActivateMovement()
    //{
    //    CanMove = true;
    //}

    private void HandleInputs(bool deactivate = false)
    {
        if (!deactivate)
        {
            _horInput = Input.GetAxisRaw("Horizontal");
            _verInput = Input.GetAxisRaw("Vertical");
            _runPressed = Input.GetButton("Run");
        }
        else
        {
            
        }
    }

    //Esta funcion se encarga de buscar recursivamente el hijo del objeto padre
    //En este caso, busca a itemUbication, pero puede usarse para encontrar cualquier hijo de cualquier objeto;
    private GameObject FindChildByName(GameObject parent, string name){
        //El parametro "name" es el nombre del objeto que buscamos
        if(parent.name == name){
            return parent;
        }

        foreach(Transform child in parent.transform){
            GameObject result = FindChildByName(child.gameObject, name);

            if(result != null){
                return result;
            }
        }

        return null;
    }

    public GameObject GetItemUbication(){
        return _itemUbication;
    }

}
