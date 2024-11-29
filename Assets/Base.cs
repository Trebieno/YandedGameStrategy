using CodeBase.SystemGame;
using System;
using UnityEngine;

public class Base : MonoCache
{
    [SerializeField] private int _money;
    [SerializeField]  private int _curOre;
    [SerializeField] private int _maxOre;
    [SerializeField] private float _curTimeProduction;
    [SerializeField] private float _maxTimeProduction;
    [SerializeField] private bool _defenceMode;

    [SerializeField] private Transform _moveToPointForUnits;

    public int Money => _money;
    public int Ore => _curOre;
    public float MaxOre => _maxOre;
    public bool DefenceMode => _defenceMode;

    public Action<float, float> OnUpdateTimer;
    public Action<int> OnUpdateMoney;
    public Action<int, int> OnUpdateOre;

    public override void OnFixedTick()
    {
        if(_curTimeProduction > 0)
            UpdateTimeProduction();
    }

    private void UpdateTimeProduction()
    {
        _curTimeProduction -= Time.fixedDeltaTime;
        OnUpdateTimer.Invoke(_curTimeProduction, _maxTimeProduction);
    }
}
