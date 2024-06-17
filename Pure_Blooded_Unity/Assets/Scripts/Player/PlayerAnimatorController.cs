using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    PlayerController _playerController;
    Animator _animatorController;

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _animatorController = GetComponent<Animator>();
    }
    private void Update()
    {
        //if (_playerController.GetPlayerDir.magnitude != 0)
        //{
        //    _animatorController.SetFloat("Movement", _playerController.GetPlayerDir.magnitude);
        //}
        //else
        //{
        //    _animatorController.SetFloat("Movement", 0);
        //}
    }
    public void SetTrigger(string triggerName)
    {
        _animatorController.SetTrigger(triggerName);
    }
}
