using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : MonoBehaviour, IUIResourceSlider
{
    public TextMeshProUGUI value;
    public Slider slider;
    public int size;
    public int maxSize;

    private void Awake()
    {
        StaticData.UIBag = this;
    }
    private void Start()
    {
        maxSize = StaticData.settings.playerBagSize;
    }
    public void UpdateViewByPlayerBagStack(PlayerBagStack playerBagStack)
    {
        UpdateView(playerBagStack.count);
    }
    public void UpdateView(int newSize)
    {
        if (newSize <= maxSize)
        {
            size = newSize;

            slider.value = ((float)size) / maxSize;
            value.text = $"{size}/{maxSize}";
        }
    }

}
