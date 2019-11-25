using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject levelBoss;

    public GameObject enemies;

    public GameObject pauseMenu;

    public GameObject winMenu;

    public void LateUpdate()
    {
        if (enemies.transform.childCount == 0)
        {
            SpawnBoss();
        }
    }

    public void HandleLevelComplete()
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void SpawnBoss()
    {
        levelBoss.SetActive(true);
    }

    public void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
