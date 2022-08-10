using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenCulture : MonoBehaviour
{
    public GameObject GrassView;
    public ParticleSystem hitVFX;
    public GameObject GrassBlockForSpawn;

    private bool isActive = true;
    private void OnTriggerEnter(Collider other)
    {
        if (isActive == true)
        {
            Mow();
        }
    }


    private void Mow()
    {
        isActive = false;
        hitVFX.Play();
        GrassView.SetActive(false);
        StartCoroutine(Fade());
        SpawnGrassBlock();
    }
    private void Grow()
    {
        GrassView.SetActive(true);
        isActive = true;
    }

    private void SpawnGrassBlock()
    {
        Instantiate(GrassBlockForSpawn, transform);
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(StaticData.settings.grassGrowTime);
        Grow();
    }
}

