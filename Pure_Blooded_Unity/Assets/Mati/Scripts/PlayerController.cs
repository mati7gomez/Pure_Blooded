using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Camera _mainCamera;
    private Transform _playerBody;

    public bool CanMove = true;

    private float _horInput;
    private float _verInput;
    [SerializeField] private float _playerSpeed;

    private float _targetAngle;
    private float _finalAngle;
    private float _turnSmoothVelocity;


    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _playerDir;

    private void Start()
    {
        _playerBody = transform.GetChild(0).transform;
        _mainCamera = Camera.main;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Debug.Log(CanMove);
        if (CanMove)
            GetPlayerInputs();
    }

    private void FixedUpdate()
    {
        GetCamVectors();
        SetPlayerMoveDir();
        MovePlayer();
        RotatePlayer();

        //Debug.Log(_controller.velocity.magnitude);
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
    private void GetPlayerInputs()
    {
        _horInput = Input.GetAxis("Horizontal");
        _verInput = Input.GetAxis("Vertical");
    }
    private void SetPlayerMoveDir()
    {
        _playerDir = _camForward * _verInput + _camRight * _horInput;

        _playerDir = Vector3.ClampMagnitude(_playerDir, 1f);
    }
    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("El shift ta apretao");
            _playerSpeed = 8f;
        }
        else
        {
            _playerSpeed = 5f;

        }

        _controller.Move(_playerDir * _playerSpeed * Time.fixedDeltaTime);
    }
    private void RotatePlayer()
    {
        if (_playerDir.magnitude != 0)
        {
            _targetAngle = Mathf.Atan2(_playerDir.x, _playerDir.z) * Mathf.Rad2Deg;
            _finalAngle = Mathf.SmoothDampAngle(_playerBody.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.1f);
            _playerBody.rotation = Quaternion.Euler(0f, _finalAngle, 0f);
        }
        
    }

    public void DisableMovement()
    {
        CanMove = false;
        _horInput = 0;
        _verInput = 0;
        _playerDir = Vector3.zero;
        //Debug.Log("ayayaya q capo q soy");
        Invoke("ActivateMovement", 0.1f);
    }
    private void ActivateMovement()
    {
        CanMove = true;
    }

}
