using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private int _amount;
    public int Amount => _amount;

    public int TakeAmount(int damage)
    {
        _amount -= damage;
        return damage;
    }
}
