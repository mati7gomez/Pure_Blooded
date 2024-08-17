using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    [SerializeField] AudioClip[] _listAudioClips;

    public AudioClip[] ListAudioClips 
    {
        get { return _listAudioClips; }
        set { _listAudioClips = value; }
    }
}
