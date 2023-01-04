using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 場景控制器
/// </summary>
public class SceneController : MonoBehaviour
{
    #region 方法
    /// <summary>
    /// 延遲載入場景1
    /// </summary>
    public void LoadGameScene1()
    {
        Invoke("DelayLoadGameScene1", 1);
    }

    /// <summary>
    /// 延遲載入場景2
    /// </summary>
    public void LoadGameScene2()
    {
        Invoke("DelayLoadGameScene2", 1);
    }

    /// <summary>
    /// 載入場景1
    /// </summary>
    private void DelayLoadGameScene1()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 載入場景2
    /// </summary>
    private void DelayLoadGameScene2()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
