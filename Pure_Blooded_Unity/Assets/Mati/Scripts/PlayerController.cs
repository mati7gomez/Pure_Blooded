using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    Camera _mainCamera;

    [SerializeField] private float _playerSpeed;
    private float _horInput;
    private float _verInput;

    

    private Vector3 _playerDir;

    private void Start()
    {
        _mainCamera = Camera.main;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _horInput = Input.GetAxis("Horizontal");
        _verInput = Input.GetAxis("Vertical");

        _playerDir = new Vector3(_horInput, 0f, _verInput);
        _playerDir = Vector3.ClampMagnitude(_playerDir, 1f);

        _controller.Move(_playerDir * _playerSpeed * Time.deltaTime);
    }
}
