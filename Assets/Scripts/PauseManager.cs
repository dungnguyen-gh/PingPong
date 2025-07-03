using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button btnResume, btnRestart;
    [SerializeField] private GameManager gameManager;
    private void Start()
    {
        pauseMenu.SetActive(false);
        btnRestart.onClick.AddListener(RestartGame);
        btnResume.onClick.AddListener(ResumeGame);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameManager.isPlaying)
        {
            if (pauseMenu.activeSelf) ResumeGame();
            else PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
