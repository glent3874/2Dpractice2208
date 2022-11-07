using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverController : MonoBehaviour
{
    [Header("�����e���ʵe����")]
    public Animator aniFinal;
    [Header("�������D")]
    public TextMesh textFinalTitle;
    [Header("�C��������r")]
    [TextArea(1, 3)]
    public string stringWin = "Game Over";
    public string stringLose = "Game Over";
    [Header("���s�P���}����")]
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
