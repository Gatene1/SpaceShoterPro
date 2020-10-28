using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    private bool _isPaused = false;

    [SerializeField] private GameObject _pauseMenu;
    private Animator _anim;

    private void Start()
    {
        _anim = _pauseMenu.gameObject.GetComponent<Animator>();
        _anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
            SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (!_isPaused)
            {
                PauseGame();
            }
            else if (_isPaused)
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {
        _isPaused = true;
        _pauseMenu.SetActive(true);
        _anim.SetBool("isPaused", true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        _isPaused = false;
        //_pauseMenu.SetActive(false);
        _anim.SetBool("isPaused", false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        _isPaused = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
