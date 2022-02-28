using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public GameObject UI;

    private void OnTriggerEnter(Collider other)
    {
        UI.active = true;
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseUI()
    {
        UI.active = false;
    }
}
