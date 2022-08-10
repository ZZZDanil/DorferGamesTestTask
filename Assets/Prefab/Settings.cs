using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public float playerSpeed = 5f;
    public int playerBagSize = 40;
    public float grassGrowTime = 10f;
    public float resourceGettingTime = 0.4f;
    public float resourceSellingTime = 0.2f;
    public float sellingNextResourceTime = 0.1f;
    public float playerCoinsCounting = 0.1f;
    public float playerCoinsCountingDelta = 0.1f;
    public Vector3 playerBagResourceScaler = Vector3.one;



    private void Awake()
    {
        StaticData.settings = this;
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

    }
}
