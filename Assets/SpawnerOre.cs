using CodeBase.SystemGame;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOre : MonoCache
{
    [SerializeField] private List<Ore> orePrefabs; // Список префабов
    [SerializeField] private float spawnRadius = 5f; // Радиус спавна
    [SerializeField] private int initialPoolSize = 10; // Начальный размер пула

    [SerializeField] private int _countOre;
    private List<Ore> oreList = new List<Ore>();

    private Queue<Ore> orePool; // Пул для выключенных префабов

    [SerializeField] private float _spawnOreTimer;
    [SerializeField] private float _maxSpawnOreTimer;

    private void Awake()
    {
        // Инициализация пула
        orePool = new Queue<Ore>();

        // Заполнение пула выключенными префабами
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
        // Создание нового префаба
        Ore oreObject = Instantiate(GetRandomOrePrefab());
        Ore oreComponent = oreObject.GetComponent<Ore>();

        oreComponent.OnDied += 

        // Деактивация объекта и добавление в пул
        oreObject.gameObject.SetActive(false);
        orePool.Enqueue(oreComponent);
    }

    private Ore GetRandomOrePrefab()
    {
        // Получение случайного префаба из списка
        if (orePrefabs.Count == 0)
            return null;

        int randomIndex = Random.Range(0, orePrefabs.Count);
        return orePrefabs[randomIndex];
    }

    public void SpawnOre(Vector3 position)
    {
        if (orePool.Count > 0)
        {
            // Извлечение из пула
            Ore ore = orePool.Dequeue();

            // Установка позиции и активация объекта
            ore.transform.position = GetRandomSpawnPosition(position);
            ore.gameObject.SetActive(true);

            // Установка случайного количества Amount
            ore.SetAmount(Random.Range(1f, 10f)); // Пример установки количества
        }
        else
        {
            Debug.LogWarning("Нет доступных объектов в пуле!");
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center)
    {
        // Генерация случайной позиции в радиусе
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return new Vector3(center.x + randomCircle.x, center.y, center.z + randomCircle.y);
    }
}
