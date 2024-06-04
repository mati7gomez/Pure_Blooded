using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    [SerializeField] GameObject _megaPack;
    [SerializeField] Transform _roadRef;
    private Vector3 _roadMoveDir;
    [SerializeField] private float _roadMoveSpeed;

    private void Start()
    {
        _roadMoveDir = -_roadRef.up.normalized;
    }

    private void FixedUpdate()
    {
        transform.Translate(_roadMoveDir * Time.fixedDeltaTime * _roadMoveSpeed);
    }

}
