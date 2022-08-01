﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Random = UnityEngine.Random;
using General;
using Services;
using Chunk;
using UI;
using Components;

namespace Entities
{
    public class EntitySpawner : MonoBehaviour
    {
        private Dictionary<Type, IPool> _pools = new();
        private readonly WayMatrix _wayMatrix = new();
        private Quadcopter _quadcopter;
        private Client _client;
        private bool _isClientRequested;
        private Deliverer _deliverer;
        private Pizza _pizza;
        private PizzaGuy _pizzaGuy;

        [SerializeField, BoxGroup("Configurations")] private QuadcopterConfig _quadcopterConfig;
        [SerializeField, BoxGroup("Configurations")] private BirdConfig _birdConfig;
        [SerializeField, BoxGroup("Configurations")] private CarConfig _carConfig;
        [SerializeField, BoxGroup("Configurations")] private NetGuyConfig _netGuyConfig;
        [SerializeField, BoxGroup("Configurations")] private BatteryConfig _batteryConfig;
        [SerializeField, BoxGroup("Configurations")] private ClientConfig _clientConfig;
        [SerializeField, BoxGroup("Configurations")] private PizzaGuyConfig _pizzeriaGuyConfig;
        [SerializeField, BoxGroup("Configurations")] private PizzaConfig _pizzaConfig;

        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _aggressiveBirdDensity;
        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _carDensity;
        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _netGuyDensity;

        [SerializeField][Range(0, 1000)] private int _spawnDistance;

        private void OnEnable()
        {
            GameFlowService.OnPlay += () =>
            {
                ChunkGenerator chunkGenerator = FindObjectOfType<ChunkGenerator>();
                chunkGenerator.OnSpawnChunk += SettleWindows;
                _deliverer.OnPizzeriaRequested += chunkGenerator.RequestPizzeria;

                if (IsEnabled<Car>())
                    SpawnCars();

                if (IsEnabled<Bird>())
                    SpawnAggressiveBirds();
            };
        }

        public Quadcopter EnableQuadcopter(Container entityContainer)
        {
            LifeCounter lifeCounter = FindObjectOfType<LifeCounter>();
            MoneyCounter moneyCounter = FindObjectOfType<MoneyCounter>();

            _quadcopter = GetCreatedEntity(new QuadcopterFactory(_quadcopterConfig, entityContainer, lifeCounter, moneyCounter, _pizzaConfig.PizzaPrefab));
            _deliverer = _quadcopter.GetComponent<Deliverer>();
            _quadcopter.gameObject.SetActive(false);
            return _quadcopter;
        }

        public void EnableCarTraffic(Container entityContainer)
        {
            _pools[typeof(Car)] = new Pool<Car>(new CarFactory(_carConfig), entityContainer, 10);
        }

        public void EnableAggressiveBirds(Container entityContainer)
        {
            _pools[typeof(Bird)] = new Pool<Bird>(new BirdFactory(_birdConfig), entityContainer, 10);
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
            EnablePizza(entityContainer);
            EnablePizzeriaGuy(entityContainer, chunkGenerator);
            EnableClient(entityContainer);
        }

        private void EnablePizza(Container entityContainer)
        {
            _pizza = new PizzaFactory(_pizzaConfig, _deliverer, _quadcopter).GetCreated();
            _pizza.transform.SetParent(entityContainer.transform);
        }

        private void EnablePizzeriaGuy(Container entityContainer, ChunkGenerator chunkGenerator)
        {
            _pizzaGuy = new PizzaGuyFactory(_pizzeriaGuyConfig, _deliverer, _pizza, _quadcopter).GetCreated();
            _pizzaGuy.transform.SetParent(entityContainer.transform);
            chunkGenerator.OnPizzeriaSpawned += SpawnPizzeriaGuy;
        }

        private void EnableClient(Container entityContainer)
        {
            ClientFactory clientFactory = new ClientFactory(_clientConfig, _deliverer);
            _client = clientFactory.GetCreated();
            _client.gameObject.SetActive(false);
            _client.transform.SetParent(entityContainer.transform);
            _deliverer.OnPizzaGrabbed += () => _isClientRequested = true;
        }

        private void SpawnPizzeriaGuy(PizzaDispensePoint dispensePoint)
        {
            PizzaGuy pizzeriaGuy = _pizzaGuy;
            pizzeriaGuy.gameObject.SetActive(true);
            pizzeriaGuy.transform.position = (dispensePoint.transform.position);
            BoxCollider pizzeriaGuyCollider = pizzeriaGuy.GetComponent<BoxCollider>();
            pizzeriaGuyCollider.center = new Vector3(-1 * Mathf.Abs(dispensePoint.transform.position.x) + WayMatrix.HorizontalSpacing / 2, WayMatrix.VerticalSpacing / 2, 0);
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
                    GetPool<Bird>().Get(position + Vector3.forward * _spawnDistance);
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
                    _client.transform.eulerAngles = Vector3.up * (_client.transform.position.x < 0 ? 180 : 0);
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

        private void OnDisable()
        {
            GameFlowService.OnPlay -= () =>
            {
                FindObjectOfType<ChunkGenerator>().OnSpawnChunk += SettleWindows;

                if (IsEnabled<Car>())
                    SpawnCars();

                if (IsEnabled<Bird>())
                    SpawnAggressiveBirds();
            };
        }
    }
}
