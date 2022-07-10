﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class EntitySpawner : MonoBehaviour
    {
        private Dictionary<Type, IPool> _pools = new();
        private readonly WayMatrix _wayMatrix = new();
        private Quadcopter _quadcopter;
        private Client _client;
        private bool _isClientRequested;

        [Header("Configurations")]
        [SerializeField] private QuadcopterConfig _quadcopterConfig;
        [SerializeField] private PlayerCameraConfig _playerCameraConfig;
        [SerializeField] private AggressiveBirdConfig _aggressiveBirdConfig;
        [SerializeField] private CarConfig _carConfig;
        [SerializeField] private NetGuyConfig _netGuyConfig;
        [SerializeField] private BatteryConfig _batteryConfig;
        [SerializeField] private ClientConfig _clientConfig;
        [SerializeField] private PizzeriaGuyConfig _pizzeriaGuyConfig;
        [Space(30)]
        [Header("SpawnDensity")]
        [SerializeField][Range(0, 100)] private int _aggressiveBirdDensity;
        [SerializeField][Range(0, 100)] private int _carDensity;
        [SerializeField][Range(0, 100)] private int _netGuyDensity;
        [Space(30)]
        [SerializeField][Range(0, 1000)] private int _spawnDistance;

        private void OnEnable() => GameStopper.OnPlay += Setup;
        

        private void Setup()
        {
            FindObjectOfType<ChunkGenerator>().OnSpawnChunk += SettleWindows;

            if (IsEnabled<Car>())
                SpawnCars();

            if (IsEnabled<AggressiveBird>())
                SpawnAggressiveBirds();
        }

        public PlayerCamera EnablePlayerCamera(Container entityContainer)
        {
            return GetCreatedEntity(new PlayerCameraFactory(_playerCameraConfig, entityContainer, _wayMatrix.GetPosition(MatrixPosition.Center)));
        }

        public Quadcopter EnableQuadcopter(Container entityContainer)
        {
            LifeCounter lifeCounter = FindObjectOfType<LifeCounter>();
            ChargeCounter chargeCounter = FindObjectOfType<ChargeCounter>();
            MoneyCounter moneyCounter = FindObjectOfType<MoneyCounter>();

            _quadcopter = GetCreatedEntity(new QuadcopterFactory(_quadcopterConfig, entityContainer, lifeCounter, chargeCounter, moneyCounter));
            _quadcopter.gameObject.SetActive(false);
            return _quadcopter;
        }

        public void EnableCarTraffic(Container entityContainer)
        {
            _pools[typeof(Car)] = new Pool<Car>(new CarFactory(_carConfig), entityContainer, 10);
        }

        public void EnableAggressiveBirds(Container entityContainer)
        {
            _pools[typeof(AggressiveBird)] = new Pool<AggressiveBird>(new AggressiveBirdFactory(_aggressiveBirdConfig), entityContainer, 10);
        }

        public void EnableNetGuys(Container entityContainer)
        {
            _pools[typeof(NetGuy)] = new Pool<NetGuy>(new NetGuyFactory(_netGuyConfig), entityContainer, 10);
        }

        public void EnableBatteries(Container entityContainer)
        {
            _pools[typeof(Battery)] = new Pool<Battery>(new BatteryFactory(_batteryConfig), entityContainer, 3);
            _quadcopter.GetComponent<Charger>().OnDecreased += SpawnBattery;
        }

        public void EnableDelivery(Container entityContainer, ChunkGenerator chunkGenerator)
        {
            EnablePizzeriaGuy(entityContainer, chunkGenerator);
            EnableClient(entityContainer);
        }

        private void EnablePizzeriaGuy(Container entityContainer, ChunkGenerator chunkGenerator)
        {
            _pools[typeof(PizzeriaGuy)] = new Pool<PizzeriaGuy>(new PizzeriaGuyFactory(_pizzeriaGuyConfig), entityContainer, 2);
            chunkGenerator.OnPizzeriaSpawned += SpawnPizzeriaGuy;
        }

        private void EnableClient(Container entityContainer)
        {
            ClientFactory clientFactory = new ClientFactory(_clientConfig);
            _client = clientFactory.GetCreated();
            _client.gameObject.SetActive(false);
            _client.transform.SetParent(entityContainer.transform);
            Deliverer.OnPizzaGrabbed += () => _isClientRequested = true;
        }

        private void SpawnPizzeriaGuy(PizzaDispensePoint dispensePoint)
        {
            PizzeriaGuy pizzeriaGuy = GetPool<PizzeriaGuy>().Get(dispensePoint.transform.position);
            BoxCollider pizzeriaGuyCollider = pizzeriaGuy.GetComponent<BoxCollider>();
            pizzeriaGuyCollider.center = new Vector3(-1 * Mathf.Abs(dispensePoint.transform.position.x) + WayMatrix.HorizontalSpacing/2, WayMatrix.VerticalSpacing / 2, 0);
            pizzeriaGuy.transform.eulerAngles = Vector3.up * (pizzeriaGuy.transform.position.x < 0 ? 180 : 0);
        }

        private IEnumerator CarSpawning(int line)
        {
            float delay = 0.5f;
            float maxDistance = 15f;
            float minDistance = 5f;
            float previousHalfSize = 0;
            float offset = 1.5f;

            Vector3 instancePosition = new Vector3(-250f, -15f, 1000f);
            Vector3 spawnPosition = _wayMatrix.GetPositionByArrayCoordinates(new Vector2Int(line, WayMatrix.Height - 1)) + Vector3.down * offset + Vector3.forward * _spawnDistance;

            while (true)
            {
                if (_carDensity > Random.Range(0, 100))
                {
                    Car car = GetPool<Car>().Get(instancePosition);
                    if (car.CarColorChanger != null) car.CarColorChanger.ChangeColorRandom();

                    float distanceBetweenCars = Random.Range(minDistance, maxDistance);
                    float speed = GlobalSpeedService.Speed + _carConfig.SelfSpeed;
                    float halfSize = car.Size / 2;
                    delay = (Mathf.Sqrt(speed * speed + 2 * GlobalSpeedService.Acceleration * (distanceBetweenCars + halfSize + previousHalfSize)) - speed) / GlobalSpeedService.Acceleration;
                    previousHalfSize = halfSize;

                    yield return new WaitForSeconds(delay);

                    car.transform.position = spawnPosition;
                }
                else yield return new WaitForSeconds(delay);
            }
        }

        public void SpawnCars()
        {
            for (int line = 0; line < WayMatrix.Width; line++)
            {
                StartCoroutine(CarSpawning(line));
            }
        }

        private IEnumerator AggressiveBirdsSpawning(int line, int row)
        {
            WaitForSeconds spawnDelay = new(Random.Range(0.15f * GlobalSpeedService.Speed / GlobalSpeedService.Speed, 0.5f * GlobalSpeedService.Speed / GlobalSpeedService.Speed));

            while (true)
            {
                Vector3 position = _wayMatrix.GetPositionByArrayCoordinates(new Vector2Int(line, row));

                if (_aggressiveBirdDensity > Random.Range(0, 100))
                {
                    GetPool<AggressiveBird>().Get(position + Vector3.forward * _spawnDistance);
                }

                yield return spawnDelay;
            }
        }

        public void SpawnAggressiveBirds()
        {
            for (int row = 0; row < 2; row++)
            {
                for (int i = 0; i < WayMatrix.Width; i++)
                {
                    StartCoroutine(AggressiveBirdsSpawning(i, row));
                }
            }
        }

        private void SettleWindows(IEnumerable<Window> windows)
        {
            foreach (Window window in windows)
            {
                if (Random.Range(0, 100) > _netGuyDensity)
                {
                    window.Close();
                    continue;
                }

                if (_client != null && _isClientRequested)
                {
                    _client.gameObject.SetActive(true);
                    _client.transform.position = window.GetSpawnPoint();
                    Debug.Log("Появился клиент!");
                    _isClientRequested = false;
                }

                else if (IsEnabled<NetGuy>()) GetPool<NetGuy>().Get(window.GetSpawnPoint());

                window.Open();
            }
        }

        private void SpawnBattery()
        {
            GetPool<Battery>().Get(_wayMatrix.GetRandomPosition() + Vector3.forward * WayMatrix.Horizon);
        }

        public Pool<T> GetPool<T>() where T : Entity => _pools[typeof(T)] as Pool<T>;

        public bool IsEnabled<T>() where T : Entity => _pools.ContainsKey(typeof(T));

        private E GetCreatedEntity<E>(IFactory<E> entityFactory) where E : Entity => entityFactory.GetCreated();

        private void OnDisable() => GameStopper.OnPlay -= Setup;
     
        private void OnDestroy() => StopAllCoroutines();
    }
}
