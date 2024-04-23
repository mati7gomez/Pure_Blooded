using Cinemachine;
using UnityEngine;

public class CameraChanged : MonoBehaviour
{
    //[SerializeField] CinemachineVirtualCamera[] _cam;
    //bool[] camActivated;
    //bool[] camActive;

    //PlayerController playerRef;

    //public delegate void CameraChangedCallback();
    //public event CameraChangedCallback CamChangedEvent;


    //private void Start()
    //{
    //    playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
    //    CamChangedEvent += playerRef.DisableMovement;
    //    camActivated = new bool[_cam.Length];
    //    camActive = new bool[_cam.Length];
    //}
    //private void Update()
    //{
    //    UpdateCameraStatus(0);
    //    UpdateCameraStatus(1);
    //    UpdateCameraStatus(2);

    //}
    //private void UpdateCameraStatus(int camIndex)
    //{
    //    if (CinemachineCore.Instance.IsLive(_cam[camIndex]) && !camActivated[camIndex]) //Camara activada
    //    {
    //        camActive[camIndex] = true;
    //        camActivated[camIndex] = true;
    //        Debug.Log("Camara: " + camIndex + " Status: " + camActivated[camIndex]);
    //    }
    //    else if (!CinemachineCore.Instance.IsLive(_cam[camIndex]) && camActivated[camIndex])
    //    {
    //        camActive[camIndex] = false;
    //        camActivated[camIndex] = false;
    //        Debug.Log("Camara: " + camIndex + " Status: " + camActivated[camIndex]);
    //        DeactivatePlayerMovementTemporary();
    //    }
    //}

    //private void DeactivatePlayerMovementTemporary() 
    //{
    //    // Verifica si hay suscriptores antes de disparar el evento
    //    if (CamChangedEvent != null)
    //    {
    //        // Dispara el evento
    //        CamChangedEvent();
    //    }
    //}
}
