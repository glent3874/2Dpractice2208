using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public void LoadGameScene1()
    {
        Invoke("DelayLoadGameScene1", 1);
    }

    public void LoadGameScene2()
    {
        Invoke("DelayLoadGameScene2", 1);
    }

    public void LoadGameScene3()
    {
        Invoke("DelayLoadGameScene3", 1);
    }

    private void DelayLoadGameScene1()
    {
        SceneManager.LoadScene(1);
    }

    private void DelayLoadGameScene2()
    {
        SceneManager.LoadScene(2);
    }

    private void DelayLoadGameScene3()
    {
        SceneManager.LoadScene(3);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
