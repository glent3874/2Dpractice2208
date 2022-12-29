using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血條
/// </summary>
public class HealthBar : MonoBehaviour
{
    #region 欄位
    [SerializeField] Slider slider;         //血條UI
    #endregion

    #region 方法
    /// <summary>
    /// 設定最大血條
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;           //最大血量
        slider.value = health;              //設定血量
    }
    /// <summary>
    /// 設定血條
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        slider.value = health;              //設定血量
    }
    #endregion
}
