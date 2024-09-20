using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] float _actOfGodTime = 4f;
    [SerializeField] GameObject _victoryScreen;
    [SerializeField] GameObject _inGameMenu;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Slingshot _slingshot;

    public static GameManager instance;

    private int _enemyCount;

    public int BirdCount = 3;
    public int AvailableBirds = 3;
    public bool BringNextBird = false;
    public bool GamePaused = false;

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            _victoryScreen.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (instance == null)
        {
            instance = this;
        }

        _inGameMenu.SetActive(true);
        _pauseMenu.SetActive(false);
        _victoryScreen.SetActive(false);
        _enemyCount = FindObjectsOfType<Enemy>().Length;
    }

    public void BirdsFired()
    {
        AvailableBirds--;

        CheckGameState();
    }

    public void EnemyDown()
    {
        _enemyCount--;

        CheckGameState();
    }

    void CheckGameState()
    {
        if (_enemyCount == 0)
        {
            Victory();
        }

        if (AvailableBirds <= 0)
        {
            StartCoroutine(WaitForActOfGod());
        }
    }

    IEnumerator WaitForActOfGod()
    {
        yield return new WaitForSeconds(_actOfGodTime);

        if (_enemyCount == 0)
        {
            Victory();
        }
        else if (AvailableBirds <= 0)
        {
            RestartLevel();
        }
    }

    public void PauseGame()
    {
        _inGameMenu.SetActive(false);
        _pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Resume()
    {
        _inGameMenu.SetActive(true);
        _pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Victory()
    {
        _inGameMenu.SetActive(false);
        _victoryScreen.SetActive(true);

        _slingshot.enabled = false;
    }

    public void RestartLevel()
    {
        DOTween.Clear(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
