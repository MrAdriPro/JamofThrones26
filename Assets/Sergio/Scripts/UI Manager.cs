using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text infoText;
    public string winMenssage;
    public string loseMessage;

    void Start()
    {
        canvas.SetActive(false);
    }

    public void ShowCanvas(bool gameFinished)
    {
        infoText.text = gameFinished ? winMenssage : loseMessage;
        canvas.SetActive(true);
    }
}
