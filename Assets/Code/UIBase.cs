using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    [SerializeField] private Slider _sliderProduction;
    [SerializeField] private TMP_Text _textAmount;
    [SerializeField] private TMP_Text _textMoney;

    public void UpdateSliderProduction(float current, float maximum)
    {
        _sliderProduction.maxValue = maximum;
        _sliderProduction.value = current;

    }

    public void UpdateTextAmount(int current, int maximum)
    {
        _textAmount.text = $"{current}/{maximum}";
    }

    public void UpdateMoney(int money)
    {
        _textMoney.text = money.ToString();
    }
}
