using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress Instance;
    public float fillTime = 1;
    public float barIncreaseValue = 7;
    public Image fillBar;
    public Image icon;

    private float storedFillAmount;
    private float xIncreasedValue;

    private void Awake()
    {
        Instance = this;
    }

    public void AddValueToBar()
    {
        storedFillAmount += barIncreaseValue / 100f;

        fillBar.DOFillAmount(storedFillAmount, fillTime).startValue = storedFillAmount - barIncreaseValue / 100f;
        var iconPos = icon.transform.localPosition;
        if (iconPos.x >= 670)
            return;
        xIncreasedValue += barIncreaseValue * 670 / 100;
        icon.transform.DOLocalMoveX(Mathf.Clamp(xIncreasedValue, 0, 670), fillTime).startValue = new Vector3(xIncreasedValue - barIncreaseValue * 670 / 100, iconPos.y, iconPos.z);
    }
}