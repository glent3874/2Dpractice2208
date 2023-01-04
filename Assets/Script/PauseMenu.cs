using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Ȱ����
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region ���
    public static bool GameIspaused = false;            //�ثe�Ȱ����A
    public GameObject pauseMenuUI;                      //�Ȱ�����UI
    #endregion

    #region �ƥ�
    void Update()
    {
        //ESC�Ȱ�
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

    #region ��k
    /// <summary>
    /// �Ȱ�
    /// </summary>
    public void Pause()
    {
        pauseMenuUI.SetActive(true);                    //�}�ҼȰ���椬��
        Time.timeScale = 0f;                            //�Ȱ��ɶ�
        GameIspaused = true;                            //�Ȱ����A
    }

    /// <summary>
    /// �^��C��
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);                   //�����Ȱ���椬��
        Time.timeScale = 1f;                            //�^�_�ɶ�
        GameIspaused = false;                           //�}�l���A
    }

    /// <summary>
    /// �^��ؿ�
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1f;                            //�^�_�ɶ�
        SceneManager.LoadScene(0);                      //���J�ؿ�����
    }

    /// <summary>
    /// ���}�C��
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();                             //���}�C��
    }
    #endregion
}
