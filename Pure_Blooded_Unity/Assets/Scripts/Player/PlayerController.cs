using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Componentes y referencias fuera del player
    private CharacterController _controller;
    private Camera _mainCamera;
    private Transform _playerBody;
    private PlayerStats _playerStats;
    private GameObject _itemUbication;
    private GameObject _itemInstance;

    //Inputs
    private float _horInput;
    private float _verInput;
    private bool _runPressed;
    

    //Variables Bandera
    public bool CanMove = true;
    private bool _hasProjectedRight;
    private bool _hasProjectedForward;
    private bool CanRun = true;
    private bool CanTalk = false; //Este permite que el personaje hable cuando apriete "F"
    private bool SurpriceTalk = false; //Este desencadena una conversacion cuando se aleja de la "Conversacion Sorpresa" 
    private bool InactivityAnimation = false;
    

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

    public float inactivityTime = 10.0f; // Tiempo de inactividad para activar la animación
    private float timeSinceLastMove = 0.0f; // Temporizador de cuanto tiempo lleva sin moverse
    private Vector3 lastPosition; // La última posición conocida del jugador


    //Si no existe un DialogueManager en la escena, este no tendrá referencia
    DialogueManager _dialogueManager;


    public static PlayerController instance;
    //Esto permite que solo haya un unico PlayerController y que se mantenga durante todo el juego
    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        //Esto permite que el programa vuelva a determinar cual es el MainCamera una vez cambia de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainCamera = Camera.main;
    }

    private void OnDestroy(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _playerBody = transform.GetChild(0).transform;
        _playerStats = new PlayerStats();

        _playerSpeed = _playerNormalSpeed;
        _itemUbication = FindChildByName(this.gameObject, "itemUbication");

        _dialogueManager = FindObjectOfType<DialogueManager>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        HandleInputs();
        Debug.DrawRay(transform.position - new Vector3(0f,_controller.height / 2f,0f), _moveDir, Color.red);
    
        if(Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("1.1_Juego");
        }

        if (Input.GetKeyDown(KeyCode.F) && CanTalk)
        {
            CanTalk = false;
            _dialogueManager.StartDialogue(_dialogueManager._dialogues[_dialogueManager._indexOfDialogues]);
        }

        if(SurpriceTalk)
        {
            SurpriceTalk = false;
            _dialogueManager.StartDialogue(_dialogueManager._dialogues[_dialogueManager._indexOfDialogues]);
        }        
    }

    private void FixedUpdate()
    {
        //Si la posicion es distinta, el contador se resetea y la bandera se mantiene como falsa
        if(transform.position != lastPosition)
        {
            timeSinceLastMove = 0.0f;
            lastPosition = transform.position;
            InactivityAnimation = false;

        } else {
            //Temporizador
            timeSinceLastMove += Time.deltaTime;
        }

        //Minetras la bandera sea falsa, puede controlar el temporizador
        if(!InactivityAnimation)
        {
            if(timeSinceLastMove >= inactivityTime)
            {
                //Cuando la bandera sea true, activa por unica vez la animacion y ya no es capaz de revisar el valor del temporizador
                InactivityAnimation = true;
                Inactivity();
            }
        }

        if (CanMove)
        {
            GetCamVectors();
            SetPlayerMoveDir();
            
            RotatePlayer();
            MovePlayer();
        }
        HandlePlayerGravity();

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
        if (_horInput == 0 && !_hasProjectedRight)
        {
            // Bloquea el componente vertical del movimiento horizontal para que este no afecte la direccion general, solo
            // la horizontal
            Debug.Log("Optimizar 1");
            _hasProjectedRight = true;
            _camRightTemporal = Vector3.ProjectOnPlane(_camRight, Vector3.up);
        }
        if (_horInput != 0 && _hasProjectedRight)
        {
            Debug.Log("Optimizar 2");
            _hasProjectedRight = false;
        }

        if (_verInput == 0 && !_hasProjectedForward)
        {
            //Bloquea el componente horizontal del movimiento horizontal para que este no afecte la direccion general, solo
            // la vertical
            Debug.Log("Optimizar 3");
            _hasProjectedForward = true;
            _camForwardTemporal = Vector3.ProjectOnPlane(_camForward, Vector3.up);
        }
        if (_verInput != 0 && _hasProjectedForward)
        {
            Debug.Log("Optimizar 4");
            _hasProjectedForward = false;
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

        _controller.Move(_moveDir * Time.fixedDeltaTime);
    }
    private void RotatePlayer()
    {
        if (_playerDir.x != 0f || _playerDir.z != 0f)
        {
            _targetAngle = Mathf.Atan2(_playerDir.x, _playerDir.z) * Mathf.Rad2Deg;
            _finalAngle = Mathf.SmoothDampAngle(_playerBody.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.1f);
            _playerBody.rotation = Quaternion.Euler(0f, _finalAngle, 0f);
        }
    }
    private void HandlePlayerGravity()
    {
        if (_controller.isGrounded)
        {
            if (_moveDir.y != -1f)
                _moveDir.y = -1f;

        }
        else
        {
            if (_moveDir.y > -15f)
            {
                _moveDir.y += _initialGravity * _gravityMultiplier * Time.fixedDeltaTime;
                _moveDir.y = Mathf.Clamp(_moveDir.y, -15f, -1f);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("NoCorrer"))
        {
            CanRun = false;
        }

        
        if(other.CompareTag("PuedeHablar"))
        {
            CanTalk = true;
        }
        

    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("NoCorrer"))
        {
            CanRun = true;
        }

        if(other.CompareTag("PuedeHablar"))
        {
            CanTalk = false;
        }

        if(other.CompareTag("ConversacionSorpresa"))
        {
            SurpriceTalk = true;
        }
    }

    private void HandleInputs(bool deactivate = false)
    {
        if (!deactivate)
        {
            _horInput = Input.GetAxisRaw("Horizontal");
            _verInput = Input.GetAxisRaw("Vertical");
            _runPressed = Input.GetButton("Run");
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

    public void PickUpItem(ItemSO _itemSO){
        if(_itemInstance != null){
            Destroy(_itemInstance);
        }
        GameObject _itemPrefab = _itemSO.GetItemPrefab();


        /*
        Parece que cambiaron las texturas del personaje, por eso ya no existe el objeto "_itemUbication"
        */
        _itemInstance = Instantiate(_itemPrefab, _itemUbication.transform.position, _itemUbication.transform.rotation);
        _itemInstance.transform.SetParent(_itemUbication.transform, true);

        // Ajustar la posición, rotación y escala usando las variables especificadas
        _itemInstance.transform.localPosition += _itemSO.GetItemHoldPosition(); // Sumar la posición
        _itemInstance.transform.localRotation *= _itemSO.GetItemHoldRotation(); // Multiplicar la rotación
        _itemInstance.transform.localScale *= _itemSO.GetItemHoldScale(); // Multiplicar la escala

        //Tengo que desactivar el collider del objeto que agarro para que no estorbe en las demas funciones
        //que requieren colliders
        _itemInstance.GetComponent<SphereCollider>().enabled = false;
    }

    public void Inactivity(){
        Debug.Log("XXXXXXXXXXXXXXXXXXX");
        Debug.Log("Animacion Activada");
        Debug.Log("XXXXXXXXXXXXXXXXXXX");
    }

}
