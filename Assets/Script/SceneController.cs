using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �������
/// </summary>
public class SceneController : MonoBehaviour
{
    #region ��k
    /// <summary>
    /// ������J����1
    /// </summary>
    public void LoadGameScene1()
    {
        Invoke("DelayLoadGameScene1", 1);
    }

    /// <summary>
    /// ������J����2
    /// </summary>
    public void LoadGameScene2()
    {
        Invoke("DelayLoadGameScene2", 1);
    }

    /// <summary>
    /// ���J����1
    /// </summary>
    private void DelayLoadGameScene1()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// ���J����2
    /// </summary>
    private void DelayLoadGameScene2()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// ���}�C��
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
