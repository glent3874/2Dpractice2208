using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �C�������޲z��
/// </summary>
public class GameoverController : MonoBehaviour
{
    #region ���
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

    private bool isGameover = false;
    #endregion

    #region �ƥ�
    private void Update()
    {
        Replay();
        Quit();
    }
    #endregion

    #region ��k
    /// <summary>
    /// ���s�}�l
    /// </summary>
    private void Replay()
    {
        if (isGameover && Input.GetKeyDown(kcReplay))                           //�p�G�C�������B���U���s�}�l��
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   //���o��e�������s��
    }

    /// <summary>
    /// ���}
    /// </summary>
    private void Quit()
    {
        if (isGameover && Input.GetKeyDown(kcQuit))                             //�p�G�C�������B���U���}�� 
            SceneManager.LoadScene(0);                                          //�^��menu
    }

    /// <summary>
    /// ��ܹC������
    /// </summary>
    /// <param name="win"></param>
    public void ShowGameoverView(bool win)
    {
        isGameover = true;                              //�C���w����
        aniFinal.enabled = true;                        //�}�Ұʵecomponent
        endMenuUI.SetActive(true);                      //�Ұʰʵe

        if (win) textFinalTitle.text = stringWin;       //�ӧQ�N��ܳӧQ�r��
        else textFinalTitle.text = stringLose;          //���ѴN��ܥ��Ѧr��
    }
    #endregion
}
