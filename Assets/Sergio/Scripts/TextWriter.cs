using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    [Header("Configuración")]
    public Text textComponent;
    public Canvas canvas;
    public float delay = 0.05f;
    public string[] dialogLines; 
    public Collider coll;

    private int _currentIndex = 0;
    private bool _isTyping = false;
    private bool _waitingForClick = false;
    private bool shown = false;
    private Coroutine _typingCoroutine;



    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !shown)
        {
            canvas.gameObject.SetActive(true);

            if (dialogLines.Length > 0)
                StartCoroutine(ShowCurrentLine());

            shown = true;
        }
    }

    void Start()
    {
        // textComponent = GetComponent<Text>();

        // if (dialogLines.Length > 0)
        //     StartCoroutine(ShowCurrentLine());
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (_isTyping)
        {
            // Mostrar linea entera si se hace click mientras esta escribiendo
            StopCoroutine(_typingCoroutine);
            _isTyping = false;
            textComponent.text = dialogLines[_currentIndex];
            _waitingForClick = true;
        }
        else if (_waitingForClick)
        {
            // Siguiente linea al hacer click cuando ha terminado de escribir
            _waitingForClick = false;
            _currentIndex++;

            if (_currentIndex < dialogLines.Length)
                _typingCoroutine = StartCoroutine(ShowCurrentLine());
            else
                OnDialogFinished();
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
        Debug.Log("Diálogo terminado");
    }
}