using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���
/// </summary>
public class HealthBar : MonoBehaviour
{
    #region ���
    [SerializeField] Slider slider;         //���UI
    #endregion

    #region ��k
    /// <summary>
    /// �]�w�̤j���
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;           //�̤j��q
        slider.value = health;              //�]�w��q
    }
    /// <summary>
    /// �]�w���
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        slider.value = health;              //�]�w��q
    }
    #endregion
}
