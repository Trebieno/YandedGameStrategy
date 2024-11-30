using CodeBase.SystemGame;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOre : MonoCache
{
    [SerializeField] private List<Ore> orePrefabs; // ������ ��������
    [SerializeField] private float spawnRadius = 5f; // ������ ������
    [SerializeField] private int initialPoolSize = 10; // ��������� ������ ����

    [SerializeField] private int _countOre;
    private List<Ore> oreList = new List<Ore>();

    private Queue<Ore> orePool; // ��� ��� ����������� ��������

    [SerializeField] private float _spawnOreTimer;
    [SerializeField] private float _maxSpawnOreTimer;

    private void Awake()
    {
        // ������������� ����
        orePool = new Queue<Ore>();

        // ���������� ���� ������������ ���������
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateOre();
        }
    }

    public override void OnFixedTick()
    {

    }

    private void CreateOre()
    {
        // �������� ������ �������
        Ore oreObject = Instantiate(GetRandomOrePrefab());
        Ore oreComponent = oreObject.GetComponent<Ore>();

        oreComponent.OnDied += 

        // ����������� ������� � ���������� � ���
        oreObject.gameObject.SetActive(false);
        orePool.Enqueue(oreComponent);
    }

    private Ore GetRandomOrePrefab()
    {
        // ��������� ���������� ������� �� ������
        if (orePrefabs.Count == 0)
            return null;

        int randomIndex = Random.Range(0, orePrefabs.Count);
        return orePrefabs[randomIndex];
    }

    public void SpawnOre(Vector3 position)
    {
        if (orePool.Count > 0)
        {
            // ���������� �� ����
            Ore ore = orePool.Dequeue();

            // ��������� ������� � ��������� �������
            ore.transform.position = GetRandomSpawnPosition(position);
            ore.gameObject.SetActive(true);

            // ��������� ���������� ���������� Amount
            ore.SetAmount(Random.Range(1f, 10f)); // ������ ��������� ����������
        }
        else
        {
            Debug.LogWarning("��� ��������� �������� � ����!");
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center)
    {
        // ��������� ��������� ������� � �������
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return new Vector3(center.x + randomCircle.x, center.y, center.z + randomCircle.y);
    }
}
