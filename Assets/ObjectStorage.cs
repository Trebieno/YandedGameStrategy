using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : MonoBehaviour
{
    // �������� ���������
    public static ObjectStorage Instance { get; private set; }

    // ������ ��������
    private List<Ore> _ores = new List<Ore>();
    private List<Enemy> _enemies = new List<Enemy>();
    private List<EnemyBase> _enemyBases = new List<EnemyBase>();
    private List<Miner> _miners = new List<Miner>();
    private List<Knight> _knights = new List<Knight>();
    
    public List<Ore> Ores => _ores;
    public List<Enemy> Enemies => _enemies;
    public List<EnemyBase> EnemyBases => _enemyBases;
    public List<Miner> Miners => _miners;
    public List<Knight> Knights => _knights;


    private void Awake()
    {
        // ���������, ���������� �� ��� ���������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���������� ��������
            return;
        }

        Instance = this; // ������������� ���������
        DontDestroyOnLoad(gameObject); // �� ���������� ��� �������� ����� 
    }
}
