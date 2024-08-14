using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private string[] _lines;
    [SerializeField] private AudioClip[] _audios;
    [SerializeField] private GameObject _audioSource;
    public string[] Lines
    {
        get { return _lines; }
        set { _lines = value; }
    }
    public AudioClip[] Audios
    {
        get { return _audios; }
        set { _audios = value; }
    }
    public GameObject AudioSource
    {
        get { return _audioSource; }
        set { _audioSource = value; }
    }
}
