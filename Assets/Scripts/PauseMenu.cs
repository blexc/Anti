using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public PlayerController player;
    public Button resumeButton;
    public GameObject chipImageUI;
    public TextMeshProUGUI chipCountText;
    public GameObject noteImageUI;
    public TextMeshProUGUI noteCountText;
    public GameObject starObject;

    private void Start()
    {
        GameIsPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.IsGrounded())
        {
            if (!GameIsPaused)
                Pause();
        }

        chipImageUI.SetActive((player.chipCount > 0) ? true : false);
        chipCountText.text = "x " + player.chipCount;

        noteImageUI.SetActive((player.musicNoteCount > 0) ? true : false);
        noteCountText.text = "x " + player.musicNoteCount;
        if (player.HasStar()) starObject.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        player.hasSword = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        resumeButton.Select();
        Time.timeScale = 0f;
        player.hasSword = false;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<PersistantAudioManager>().StopWhateverIsPlaying();
        SceneManager.LoadScene("MainMenu");
    }
}
