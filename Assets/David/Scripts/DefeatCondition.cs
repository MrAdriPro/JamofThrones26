using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatCondition : MonoBehaviour
{
    public string destinyName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            SceneManager.LoadScene(destinyName);
        }
    }
}
