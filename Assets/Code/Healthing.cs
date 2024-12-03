using System;
using UnityEngine;
using UnityEngine.Events;

public class Healthing : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 100f; // ������������ ��������
    [SerializeField] private float _curHealth = 100f; // ������� ��������
    [SerializeField] private float _healthRegeneration = 5f; // ����������� �������� � �������

    public float MaxHealth => _maxHealth;
    public float CurHealth => _curHealth;
    public float HealthRegeneration => _healthRegeneration;

    [Header("Events")]
    public Action<float, float> OnHealthChanged; // ������� ��������� ��������
    public Action<float, float, float> OnHealingChanged;
    public Action OnDeath; // ������� ������

    private void Start()
    {
        _curHealth = _maxHealth; // �������������� ������� ��������
    }

    public void TakeDamage(float amount)
    {
        _curHealth -= amount;
        _curHealth = Mathf.Clamp(_curHealth, 0, _maxHealth);
        OnHealthChanged.Invoke(_curHealth, _maxHealth);

        if (_curHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _curHealth += amount;
        _curHealth = Mathf.Clamp(_curHealth, 0, _maxHealth);
        OnHealingChanged.Invoke(_curHealth, _maxHealth, amount);
    }

    private void Die()
    {
        OnDeath.Invoke();
        // ������ ��� ������
        gameObject.SetActive(false);
    }

    // �������������� ������
    public void Revive(float healthAmount)
    {
        _curHealth = healthAmount;
        _curHealth = Mathf.Clamp(_curHealth, 0, _maxHealth);
        OnHealthChanged.Invoke(_curHealth, _maxHealth);
        gameObject.SetActive(true);
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        _curHealth = Mathf.Clamp(_curHealth, 0, _maxHealth);
        OnHealthChanged.Invoke(_curHealth, _maxHealth);
    }
}
