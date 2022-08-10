using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public static bool isAttack = false;
    public void Attack()
    {
        if (isAttack == false)
        {
            isAttack = true;
            StaticData.player.animatorModel.SetInteger("State", 2);
            StaticData.player.sickle.SetActive(true);
            StartCoroutine(Fade());
        }
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.4f); // animation attack time
        StaticData.player.animatorModel.SetInteger("State", 0);
        StaticData.player.sickle.SetActive(false);
        isAttack = false;
    }
}
