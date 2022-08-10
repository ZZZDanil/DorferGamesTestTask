using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public PlayerBagStack playerBagStack;

    public Transform shopCenter;

    private Transform cameraTransform;
    private Vector3 shopCenterPosition;
    private bool isSelling = false;

    private void Awake()
    {
        shopCenterPosition = shopCenter.position;
    }
    public void StartSell()
    {
        if (isSelling == false)
        {
            isSelling = true;
            StartCoroutine(DoSell());
        }
    }

    private IEnumerator DoSell()
    {
        for (int i = playerBagStack.count - 1; i > -1; i--)
        {
            GameObject obj = playerBagStack.RemoveRes(i);
            MoveToShop(obj, shopCenterPosition, StaticData.settings.resourceSellingTime);
            
            yield return new WaitForSeconds(StaticData.settings.sellingNextResourceTime);
        }
        isSelling = false;
        yield return null;
    }

    private void MoveToShop(GameObject gameObject, Vector3 target, float sellTime)
    {
        Transform objectTransform = gameObject.transform;
        objectTransform.SetParent(transform);
        objectTransform
            .DOMove((objectTransform.position + target)/2 + new Vector3(0, 1, 0), sellTime / 2)
            .OnComplete(() => {
                objectTransform.DOMove(target, sellTime / 2).OnComplete(() => {
                    StaticData.UIMoneyBank.MoveToCoins(15, shopCenter);
                    Destroy(gameObject);
                });
            });
        objectTransform.DOScale(Vector3.one, sellTime);
    }
}
