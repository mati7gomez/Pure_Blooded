using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private string[] _lines;

    public string[] Lines
    {
        get { return _lines; }
        set { _lines = value; }
    }

}
