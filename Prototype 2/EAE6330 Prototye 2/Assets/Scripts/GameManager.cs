using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalBoxes;
    public int arrivedBoxes;

    public Canvas WinUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void CheckFinish()
    {
        if(arrivedBoxes == totalBoxes)
        {
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                WinUI.gameObject.SetActive(true);
            }
            else
            {
                print("Win!!!");
                StartCoroutine(LoadNextScene());
                WinUI.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
