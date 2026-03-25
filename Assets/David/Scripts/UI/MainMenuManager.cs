using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] UnityEngine.UI.Image playImage;
    [SerializeField] UnityEngine.UI.Image exitImage;
    [SerializeField] Color offColor;

    private GameObject lastSelectedButton; 


    // Update is called once per frame
    void Update()
    {
        if(eventSystem.currentSelectedGameObject != null)
        {
            if(eventSystem.currentSelectedGameObject.name == "PlayButton")
            {
                print("Nig");
                exitImage.color = offColor;
                playImage.color = Color.white;

                lastSelectedButton = eventSystem.currentSelectedGameObject;
            }
            else
            {
                print("Nign't");
                playImage.color = offColor;
                exitImage.color = Color.white;

                lastSelectedButton = eventSystem.currentSelectedGameObject;
            }
        }
        else
        {
            eventSystem.SetSelectedGameObject(lastSelectedButton);
        }
    }

    public void ButtonAction(bool play)
    {
        if(play)
        {
            SceneManager.LoadScene("DefinitiveAdriScene");
        }
        else
        {
            Application.Quit();
        }
    }
}
