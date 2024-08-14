using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public struct Dialogues
{
    public struct Dialogue1
    {
        public static string line1 { get { return "Prota: -Hola, soy la prota"; } }
        public static string line2 { get { return "Amigo: -Hola, soy el amigo"; } }
        public string line3 { get { return "Prota: -Hola amigo gorriau"; } }
    }
    public struct Dialogue2
    {
        public string line1 { get { return "Linea1 dialogo 2"; } }
        public string line2 { get { return "Linea2 dialogo 2"; } }
    }

}

public class DialogueManager : MonoBehaviour
{
    public DialogueSO[] _dialogues;
    public DialogueSO _actualDialogue;
    private Canvas _dialogueCanvas;
    private TextMeshProUGUI _txtDialogue;

    private bool _isInDialogue;

    //Son un conjunto de esperas con colliders; al principio solo habilita a la primera, pero cada vez que se realiza algun dialogo, se
    // deshabilita esa esfera y se habilita la siguiente, que estará en la posicion donde queremos que ocurra el siguiente dialogo
    [SerializeField] private GameObject[] _orderOfDialogues;

    //Este indice es para los dialogos por separado
    public int _indexOfDialogues = 0;

    private List<string> _lines;
    [SerializeField] private float _textSpeed;

    //Este indice es para las lineas de un dialogo
    private int _index;
    [SerializeField] private AudioSource _audioSource;


    private void Start()
    {
        LoadComponents();

        foreach(var dialogo in _orderOfDialogues)
        {
            dialogo.SetActive(false);
        }   
        _orderOfDialogues[_indexOfDialogues].SetActive(true);
        //foreach (string line in dialogue1) { }
        //StartDialogue(new Dialogues.Dialogue1());

        //StartDialogue(_dialogues[0]);
    }
    private void Update()
    {


        if (_isInDialogue)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_txtDialogue.text == _lines[_index])
                {
                    SkipDialogue(_actualDialogue);
                }
                else
                {
                    StopAllCoroutines();
                    _txtDialogue.text = _lines[_index];
                }
            }
        }
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        if (dialogue != null)
        {
            _actualDialogue = dialogue;

            _lines = new List<string>();

            //Inmediatamente desactiva el collider del circulo alrededor del dialogo, para que no estorbe con el skip de las lineas
            _orderOfDialogues[_indexOfDialogues].SetActive(false);

            

            LoadLines(dialogue);
            _index = 0;
            _isInDialogue = true;

            _audioSource = dialogue.AudioSource.GetComponent<AudioSource>();
            _audioSource.clip = dialogue.Audios[_index];
            _audioSource.Play();

            EnableDialogueCanvas();
            StopPlayerMovement();
            StartCoroutine(TypeLine());
        }
    }
    private void SkipDialogue(DialogueSO _actualDialogue)
    {
        _txtDialogue.text = string.Empty;
        _index++;
        
        _audioSource.clip = _actualDialogue.Audios[_index];
        _audioSource.Play();
        if (IsLastDialogue())
        {
            EndDialogue();
            return;
        }
        
        StartCoroutine(TypeLine());
    }
    private void EndDialogue()
    {
        _lines = null;
        _index = 0;
        _isInDialogue = false;
        EnablePlayerMovement();
        DisableDialogueCanvas();

        //Cuando termina un dialogo, este indice determina que debe avanzar al siguiente
        _indexOfDialogues += 1;
        

        //Si ya se completaron todos los dialogos, entonces no hace falta activar ninguna otra zona donde pueda ocurrir un dialogo
        if(_indexOfDialogues + 1 == _orderOfDialogues.Length)
        {
            _orderOfDialogues[_indexOfDialogues].SetActive(true);
            return;
        }
    }
    IEnumerator TypeLine()
    {
        _txtDialogue.text = String.Empty;
        foreach (var c in _lines[_index].ToCharArray())
        {
            _txtDialogue.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }
    }
    private bool IsLastDialogue()
    {
        if (_index == _lines.Count)
        {
            return true;
        }
        return false;
    }
    private void LoadLines(DialogueSO dialogue)
    {
        Debug.Log(_lines);
        _lines.Clear();
        foreach (string str in dialogue.Lines)
        {
            _lines.Add(str);
        }
    }
    private void StopPlayerMovement()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = false;
    }
    private void EnablePlayerMovement()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = true;
    }

    private void EnableDialogueCanvas() { _dialogueCanvas.enabled = true;}
    private void DisableDialogueCanvas() { _dialogueCanvas.enabled = false;}


    private void LoadComponents()
    {
        _dialogueCanvas = transform.Find("CanvasDialogue").GetComponent<Canvas>();
        _txtDialogue = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
}
