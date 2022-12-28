using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 遊戲結束管理器
/// </summary>
public class GameoverController : MonoBehaviour
{
    #region 欄位
    [Header("結束畫面動畫元件")]
    public Animator aniFinal;
    [Header("結束畫面元件")]
    public GameObject endMenuUI;
    [Header("結束標題")]
    public Text textFinalTitle;
    [Header("遊戲結束文字")]
    [TextArea(1, 3)]
    public string stringWin = "Game Over";
    [TextArea(1, 3)]
    public string stringLose = "Game Over";
    [Header("重新與離開按鍵")]
    public KeyCode kcReplay = KeyCode.R;
    public KeyCode kcQuit = KeyCode.Q;

    private bool isGameover = false;
    #endregion

    #region 事件
    private void Update()
    {
        Replay();
        Quit();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 重新開始
    /// </summary>
    private void Replay()
    {
        if (isGameover && Input.GetKeyDown(kcReplay))                           //如果遊戲結束且按下重新開始鍵
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   //取得當前場景的編號
    }

    /// <summary>
    /// 離開
    /// </summary>
    private void Quit()
    {
        if (isGameover && Input.GetKeyDown(kcQuit))                             //如果遊戲結束且按下離開鍵 
            SceneManager.LoadScene(0);                                          //回到menu
    }

    /// <summary>
    /// 顯示遊戲結束
    /// </summary>
    /// <param name="win"></param>
    public void ShowGameoverView(bool win)
    {
        isGameover = true;                              //遊戲已結束
        aniFinal.enabled = true;                        //開啟動畫component
        endMenuUI.SetActive(true);                      //啟動動畫

        if (win) textFinalTitle.text = stringWin;       //勝利就顯示勝利字串
        else textFinalTitle.text = stringLose;          //失敗就顯示失敗字串
    }
    #endregion
}
