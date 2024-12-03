using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private float _amount;
    public float Amount => _amount;

    public Action<Ore> OnDied;


    private void Start()
    {
        ObjectStorage.Instance.Ores.Add(this);  
    }

    public float TakeAmount(float damage)
    {
        _amount -= damage;
        if (_amount <= 0)
            Die();
        
        return damage;
    }

    public void SetAmount(float amount)
    {
        _amount = amount;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        OnDied.Invoke(this);
    }
}
