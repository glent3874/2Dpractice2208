using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverController : MonoBehaviour
{
    [Header("結束畫面動畫元件")]
    public Animator aniFinal;
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

    private void Update()
    {
        Replay();
        Quit();
    }

    private void Replay()
    {
        if (isGameover && Input.GetKeyDown(kcReplay)) SceneManager.LoadScene(1);
    }

    private void Quit()
    {
        if (isGameover && Input.GetKeyDown(kcQuit)) SceneManager.LoadScene(0);
    }

    public void ShowGameoverView(bool win)
    {
        isGameover = true;
        aniFinal.enabled = true;

        if (win) textFinalTitle.text = stringWin;
        else textFinalTitle.text = stringLose;
    }
}
