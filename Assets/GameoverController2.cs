using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverController2 : MonoBehaviour
{
    [Header("�����e���ʵe����")]
    public Animator aniFinal;
    [Header("�����e������")]
    public GameObject endMenuUI;
    [Header("�������D")]
    public Text textFinalTitle;
    [Header("�C��������r")]
    [TextArea(1, 3)]
    public string stringWin = "Game Over";
    [TextArea(1, 3)]
    public string stringLose = "Game Over";
    [Header("���s�P���}����")]
    public KeyCode kcReplay = KeyCode.R;
    public KeyCode kcQuit = KeyCode.Q;
    
    private Scene scene;
    private bool isGameover = false;
    public static int countAllCoins;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        countAllCoins = GameObject.FindGameObjectsWithTag("Collectable").Length;
    }

    private void Update()
    {
        Replay();
        Quit();

        if (countAllCoins == 0)
        {
            ShowGameoverView(true);
        }
    }

    private void Replay()
    {
        if (isGameover && Input.GetKeyDown(kcReplay)) SceneManager.LoadScene(scene.buildIndex);
    }

    private void Quit()
    {
        if (isGameover && Input.GetKeyDown(kcQuit)) SceneManager.LoadScene(0);
    }

    public void ShowGameoverView(bool win)
    {
        isGameover = true;
        aniFinal.enabled = true;
        endMenuUI.SetActive(true);

        if (win) textFinalTitle.text = stringWin;
        else textFinalTitle.text = stringLose;
    }
}
