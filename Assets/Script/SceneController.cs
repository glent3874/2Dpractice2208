using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public void LoadGameScene()
    {
        Invoke("DelayLoadGameScene", 1);
    }

    private void DelayLoadGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
