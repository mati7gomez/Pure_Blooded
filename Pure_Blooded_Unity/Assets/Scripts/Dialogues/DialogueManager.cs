using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] DialogueSO[] _dialogues;
    private Canvas _dialogueCanvas;
    private TextMeshProUGUI _txtDialogue;

    private bool _isInDialogue;


    private List<string> _lines = new List<string>();
    [SerializeField] private float _textSpeed;
    private int _index;


    private void Start()
    {
        LoadComponents();
        //foreach (string line in dialogue1) { }
        //StartDialogue(new Dialogues.Dialogue1());
        StartDialogue(_dialogues[0]);
    }
    private void Update()
    {
        if (_isInDialogue)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_txtDialogue.text == _lines[_index])
                {
                    SkipDialogue();
                }
                else
                {
                    StopAllCoroutines();
                    _txtDialogue.text = _lines[_index];
                }
            }
        }
    }

    private void StartDialogue(DialogueSO dialogue)
    {
        if (dialogue != null)
        {
            LoadLines(dialogue);
            _index = 0;
            _isInDialogue = true;
            EnableDialogueCanvas();
            StopPlayerMovement();
            StartCoroutine(TypeLine());
        }
    }
    private void SkipDialogue()
    {
        _txtDialogue.text = string.Empty;
        _index++;
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
