using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverController : MonoBehaviour
{
    [Header("結束畫面動畫元件")]
    public Animator aniFinal;
    [Header("結束標題")]
    public TextMesh textFinalTitle;
    [Header("遊戲結束文字")]
    [TextArea(1, 3)]
    public string stringWin = "Game Over";
    public string stringLose = "Game Over";
    [Header("重新與離開按鍵")]
    public KeyCode kcReplay = KeyCode.R;
    public KeyCode kcQuit = KeyCode.Q;

    private bool isGameover;

    public void ShowGameoverView(bool win)
    {
        aniFinal.enabled = true;

        if (win) textFinalTitle.text = stringWin;
        else textFinalTitle.text = stringLose;
    }
}
