using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 暫停選單
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region 欄位
    public static bool GameIspaused = false;            //目前暫停狀態
    public GameObject pauseMenuUI;                      //暫停介面UI
    #endregion

    #region 事件
    void Update()
    {
        //ESC暫停
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIspaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 暫停
    /// </summary>
    public void Pause()
    {
        pauseMenuUI.SetActive(true);                    //開啟暫停選單互動
        Time.timeScale = 0f;                            //暫停時間
        GameIspaused = true;                            //暫停狀態
    }

    /// <summary>
    /// 回到遊戲
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);                   //關閉暫停選單互動
        Time.timeScale = 1f;                            //回復時間
        GameIspaused = false;                           //開始狀態
    }

    /// <summary>
    /// 回到目錄
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1f;                            //回復時間
        SceneManager.LoadScene(0);                      //載入目錄場景
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();                             //離開遊戲
    }
    #endregion
}
