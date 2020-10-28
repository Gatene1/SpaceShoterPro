using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Text _gameOverMessage;
    [SerializeField] private Text _pressRRestart;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverMessage.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
            Debug.LogError("Game Manager is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int scoreToPush)
    {
        _scoreText.text = "Score: " + scoreToPush;
    }    

    public void UpdateLives(int currentLives)
    {        
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverMessage.gameObject.SetActive(true);
        _pressRRestart.gameObject.SetActive(true);
        StartCoroutine(StartGameOverMessage());
    }

    IEnumerator StartGameOverMessage()
    {
        while (true)
        {
            _gameOverMessage.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverMessage.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
