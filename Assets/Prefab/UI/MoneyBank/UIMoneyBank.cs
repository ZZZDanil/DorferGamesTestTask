using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class UIMoneyBank : MonoBehaviour, IUIResourceSlider
{
    public GameObject targetCoin;
    public RectTransform childCoins;
    public PlayableDirector playable;
    public GameObject coinPrefab;
    public TextMeshProUGUI value;

    private int coins = 0;
    private Vector3 targetPosition;

    private void Start()
    {
        StaticData.UIMoneyBank = this;
        targetPosition = targetCoin.GetComponent<RectTransform>().position;
        value.text = coins.ToString();
    }
    public void MoveToCoins(int newValue, Transform bankTransform)
    {
        GameObject coin = InitCoin(bankTransform);
        MoveAnimation(coin, StaticData.settings.resourceSellingTime);
    }
    public void UpdateView(int newSize)
    {
        playable.Play();
        value.text = newSize.ToString();
    }
    private void MoveCoin(Transform bankTransform)
    {

    }
    private GameObject InitCoin(Transform bankTransform)
    {
        return (GameObject)Instantiate(coinPrefab
            , Camera.main.WorldToScreenPoint(bankTransform.position)
            , Quaternion.identity
            , transform);
    }
    private void MoveAnimation(GameObject coin, float animationTime)
    {
        RectTransform transform = coin.GetComponent<RectTransform>();
        transform.SetParent(childCoins);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        DOTween.Sequence()
            .Append(transform.DOMove(targetPosition, animationTime))
            .Join(transform.DOScale(Vector3.one, animationTime))
            .OnComplete(()=> {
                Destroy(coin);
                StartCoroutine(ChangeValue(coins, 15
                    , StaticData.settings.playerCoinsCounting
                    , StaticData.settings.playerCoinsCountingDelta));
            });

    }
    IEnumerator ChangeValue(int oldTotalCoins, int newAmount, float time, float deltaTime)
    {
        coins = oldTotalCoins + newAmount;
        float animationTime = 0.001f;
        for (int i = 0; time > animationTime && i < 100; i++)
        {
            UpdateView(oldTotalCoins + (int) (newAmount * animationTime / time));
            animationTime += deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }
        UpdateView(oldTotalCoins + newAmount);
        playable.time = 0;
        playable.Stop();
    }
}
