using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    [Header("Configuracion")]
    public Text textComponent;
    public Canvas canvas;
    public float delay = 0.05f;
    public string[] dialogLines;
    public Collider coll;
    public string detectObjective;

    private int _currentIndex = 0;
    private bool _isTyping = false;
    private bool _waitingForClick = false;
    private bool shown = false;
    private Coroutine _typingCoroutine;

    private bool _isActiveInstance = false;
    private static bool _isAnyDialogActive = false;
    private static Queue<TextWriter> _dialogQueue = new Queue<TextWriter>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(detectObjective) && !shown)
        {
            shown = true;

            if (_isAnyDialogActive)
            {
                _dialogQueue.Enqueue(this);
            }
            else
            {
                StartDialog();
            }
        }
    }

    public void StartDialog()
    {
        _isAnyDialogActive = true;
        _isActiveInstance = true;
        canvas.gameObject.SetActive(true);
        _currentIndex = 0;

        if (dialogLines.Length > 0)
        {
            _typingCoroutine = StartCoroutine(ShowCurrentLine());
        }
        else
        {
            OnDialogFinished();
        }
    }

    void Update()
    {
        if (!_isActiveInstance) return;

        if (!Input.GetMouseButtonDown(0)) return;

        if (_isTyping)
        {
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);

            _isTyping = false;
            textComponent.text = dialogLines[_currentIndex];
            _waitingForClick = true;
        }
        else if (_waitingForClick)
        {
            _waitingForClick = false;
            _currentIndex++;

            if (_currentIndex < dialogLines.Length)
            {
                _typingCoroutine = StartCoroutine(ShowCurrentLine());
            }
            else
            {
                OnDialogFinished();
            }
        }
    }

    IEnumerator ShowCurrentLine()
    {
        _isTyping = true;
        textComponent.text = "";
        string line = dialogLines[_currentIndex];

        for (int i = 0; i <= line.Length; i++)
        {
            textComponent.text = line.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }

        _isTyping = false;
        _waitingForClick = true;
    }

    void OnDialogFinished()
    {
        canvas.gameObject.SetActive(false);
        _isActiveInstance = false;
        _isAnyDialogActive = false;

        while (_dialogQueue.Count > 0)
        {
            TextWriter nextDialog = _dialogQueue.Dequeue();
            if (nextDialog != null)
            {
                nextDialog.StartDialog();
                break;
            }
        }
    }
}