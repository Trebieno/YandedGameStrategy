using CodeBase.SystemGame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnerOre : MonoCache
{
    [SerializeField] private List<Ore> orePrefabs; // Список префабов
    [SerializeField] private float spawnRadius = 5f; // Радиус спавна
    [SerializeField] private int initialPoolSize = 10; // Начальный размер пула

    [SerializeField] private int _countOre;
    private List<Ore> _oreList = new List<Ore>();

    private Queue<Ore> _orePool; // Пул для выключенных префабов

    [SerializeField] private float _spawnOreTimer = 10;
    [SerializeField] private float _maxSpawnOreTimer = 10;

    [SerializeField] private float _innerRadius;
    [SerializeField] private float _outerRadius;

    private void Awake()
    {
        // Инициализация пула
        _orePool = new Queue<Ore>();

        // Заполнение пула выключенными префабами
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateOre();
        }
    }

    private void Start()
    {
        AddFixedUpdate();
    }

    public override void OnFixedTick()
    {
        if(_spawnOreTimer > 0)
        {
            _spawnOreTimer -= Time.fixedDeltaTime;
            if (_spawnOreTimer <= 0)
            {
                SpawnOre(Vector2.zero);
                if (_oreList.Count < _countOre)
                    _spawnOreTimer = _maxSpawnOreTimer;
            }
        }

    }

    private void CreateOre()
    {
        // Создание нового префаба
        Ore oreObject = Instantiate(GetRandomOrePrefab());
        Ore oreComponent = oreObject.GetComponent<Ore>();

        oreComponent.OnDied += RemoveListOre;

        // Деактивация объекта и добавление в пул
        oreObject.gameObject.SetActive(false);
        _orePool.Enqueue(oreComponent);
    }

    private Ore GetRandomOrePrefab()
    {
        // Получение случайного префаба из списка
        if (orePrefabs.Count == 0)
            return null;

        int randomIndex = Random.Range(0, orePrefabs.Count);
        return orePrefabs[randomIndex];
    }

    public void SpawnOre(Vector2 position)
    {
        if (_orePool.Count > 0)
        {
            // Извлечение из пула
            Ore ore = _orePool.Dequeue();

            // Установка позиции и активация объекта
            ore.transform.position = GetRandomSpawnPosition(position);
            ore.gameObject.SetActive(true);

            // Установка случайного количества Amount
            ore.SetAmount(Random.Range(30f, 100f)); // Пример установки количества

            _oreList.Add(ore);
            ObjectStorage.Instance.MinerLogistics.Initialization();
            //ObjectStorage.Instance.NavMeshSurface.BuildNavMesh();

        }
        else
        {
            CreateOre();
            SpawnOre(position);
        }
    }

    private Vector2 GetRandomSpawnPosition(Vector2 center)
    {
        // Генерация случайного угла
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // Генерация случайного радиуса с учетом внутренних и внешних радиусов
        float radius = Mathf.Sqrt(Random.Range(Mathf.Pow(_innerRadius, 2), Mathf.Pow(_outerRadius, 2)));

        // Вычисление координат
        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);

        return new Vector2(x, y); // Возвращаем новую позицию как Vector2
    }

    private void RemoveListOre(Ore ore)
    {
        _oreList.Remove(ore);
        ObjectStorage.Instance.MinerLogistics.RemoveTargetOnUnits(ore);
        ObjectStorage.Instance.MinerLogistics.Initialization();
        _spawnOreTimer = _maxSpawnOreTimer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _innerRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _outerRadius);
    }
}
