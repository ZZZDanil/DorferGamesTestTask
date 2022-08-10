using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvestingDistance : MonoBehaviour
{
    public PlayerBagStack playerBagStack;
    public Transform playerBagStackTarget;
    public GameObject bagResource;

    private void OnTriggerEnter(Collider other)
    {
        if (playerBagStack.count < playerBagStack.maxCount)
        {
            GameObject newRes = other.gameObject;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            playerBagStack.AddRes(newRes);
        }
    }
}
