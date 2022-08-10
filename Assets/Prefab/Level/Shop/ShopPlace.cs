using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlace : MonoBehaviour
{
    public Shop shop;

    private void OnTriggerEnter(Collider other)
    {
        shop.StartSell();
    }

}
