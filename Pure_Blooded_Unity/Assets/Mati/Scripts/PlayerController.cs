using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Componentes y referencias fuera del player
    private CharacterController _controller;
    private Camera _mainCamera;
    private Transform _playerBody;
    private PlayerStats _playerStats;

    //Inputs
    private float _horInput;
    private float _verInput;
    private bool _runPressed;

    public bool CanMove = true;

    //Player extras
    [SerializeField] private float _playerSpeed;
    private float _initialPlayerSpeed;

    //Variables para calcular la rotacion del jugador
    private float _targetAngle;
    private float _finalAngle;
    private float _turnSmoothVelocity;

    //Variables para calcular la direccion de movimiento
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _playerDir;

    private void Start()
    {
        _playerStats = new PlayerStats();
        _initialPlayerSpeed = _playerSpeed;
        _playerBody = transform.GetChild(0).transform;
        _mainCamera = Camera.main;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        GetCamVectors();
        SetPlayerMoveDir();
        MovePlayer();
        RotatePlayer();

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
        _playerDir = _camForward * _verInput + _camRight * _horInput;

        _playerDir = Vector3.ClampMagnitude(_playerDir, 1f);
    }
    private void SetPlayerSpeed()
    {
        if (_runPressed && !_playerStats.IsRunning)
        {
            _playerStats.SetRunningState(true);
            _playerSpeed = 8f;
        }
        else if (!_runPressed && _playerStats.IsRunning)
        {
            _playerStats.SetRunningState(false);
            _playerSpeed = _initialPlayerSpeed;
        }
    }
    private void MovePlayer()
    {
        SetPlayerSpeed();
        _controller.Move(_playerDir * _playerSpeed * Time.fixedDeltaTime);
    }
    private void RotatePlayer()
    {
        if (_playerDir.magnitude != 0)
        {
            _targetAngle = Mathf.Atan2(_playerDir.x, _playerDir.z) * Mathf.Rad2Deg;
            _finalAngle = Mathf.SmoothDampAngle(_playerBody.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.2f);
            _playerBody.rotation = Quaternion.Euler(0f, _finalAngle, 0f);
        }
        
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
            _horInput = Input.GetAxis("Horizontal");
            _verInput = Input.GetAxis("Vertical");
            _runPressed = Input.GetButton("Run");
        }
        else
        {
            
        }
    }

}
