using CodeBase.SystemGame;
using System;
using UnityEngine;

public class Base : MonoCache
{
    [SerializeField] private float _money;
    [SerializeField]  private float _curOre;
    [SerializeField] private float _maxOre;
    [SerializeField] private float _curTimeProduction;
    [SerializeField] private float _maxTimeProduction;
    [SerializeField] private bool _defenceMode;

    [SerializeField] private float _requiredResourceAmount; // ���������� ��������, ����������� ��� ������������ �����
    [SerializeField] private float _moneyProduced; // ���������� �����, ���������� �� ����� ���������� ��������

    [SerializeField] private Transform _moveToPointForUnits;
    [SerializeField] private UIBase _ui;

    public float Money => _money;
    public float CurOre => _curOre;
    public float MaxOre => _maxOre;
    public bool DefenceMode => _defenceMode;

    public Action<float, float> OnUpdateTimer;
    public Action<int> OnUpdateMoney;
    public Action<int, int> OnUpdateOre;

    private void OnEnable()
    {
        AddFixedUpdate();
    }

    private void Start()
    {
        OnUpdateTimer += _ui.UpdateSliderProduction;
        OnUpdateMoney += _ui.UpdateMoney;
        OnUpdateOre += _ui.UpdateTextAmount;
    }

    public override void OnFixedTick()
    {
        if(_curTimeProduction > 0)
            UpdateTimeProduction();
        else
            if (_curOre >= _requiredResourceAmount)
                _curTimeProduction = _maxTimeProduction;
    }

    private void UpdateTimeProduction()
    {
        _curTimeProduction -= Time.fixedDeltaTime;

        if (_curTimeProduction <= 0)
        {
            _curOre -= _requiredResourceAmount;
            _money += _moneyProduced;
            OnUpdateMoney.Invoke((int)_money);
            OnUpdateOre.Invoke((int)_curOre, (int)_maxOre);
            if (_curOre >= _requiredResourceAmount)
                _curTimeProduction = _maxTimeProduction;
        }

        OnUpdateTimer?.Invoke(_curTimeProduction, _maxTimeProduction);
    }


    public float TakeOre(float amount)
    {
        // ���������, ���� ���������� ���� ��������� ������������ �����
        if (_curOre + amount > _maxOre)
        {
            // ���������, ������� ���� �� ����������
            float excessOre = (_curOre + amount) - _maxOre;

            // ������������� ������� ���������� ���� �� ��������
            _curOre = _maxOre;

            OnUpdateOre.Invoke((int)_curOre, (int)_maxOre);

            // ���������� ���������� ����, ������� �� ����������
            return excessOre;
        }
        else
        {
            // ���� ��� ����������, ��������� ����
            _curOre += amount;
            OnUpdateOre.Invoke((int)_curOre, (int)_maxOre); 
            return 0; // ���������� 0, ��� ��� ��� ��������
        }
    }
}
