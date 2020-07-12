using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI newPlayerText;

    private void Start()
    {
        if (SaveSystem.IsNewPlayer())
        {
            newPlayerText.gameObject.SetActive(true);
        }
        else
        {
            newPlayerText.gameObject.SetActive(false);
        }
    }


    public void NewGame()
    {
        SaveSystem.NewPlayer();
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
