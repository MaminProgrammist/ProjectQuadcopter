using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using General;
using Services;
using Ads;
using Chunk;
using UI;
using Components;
using Random = UnityEngine.Random;
using Level;
using static UnityEngine.Rendering.DebugUI.Table;

namespace Entities
{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField, BoxGroup("Configurations")] private QuadcopterConfig _quadcopterConfig;
        [SerializeField, BoxGroup("Configurations")] private BirdConfig _birdConfig;
        [SerializeField, BoxGroup("Configurations")] private CarConfig _carConfig;
        [SerializeField, BoxGroup("Configurations")] private GuyConfig _guyConfig;
        [SerializeField, BoxGroup("Configurations")] private BatteryConfig _batteryConfig;
        [SerializeField, BoxGroup("Configurations")] private ClientConfig _clientConfig;
        [SerializeField, BoxGroup("Configurations")] private PizzaEjectorConfig _pizzaEjectorConfig;
        [SerializeField, BoxGroup("Configurations")] private PizzaConfig _pizzaConfig;
        [SerializeField, BoxGroup("Configurations")] private RopeConfig _ropeConfig;
        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _birdsDensity;
        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _carsDensity;
        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _netGuysDensity;
        [SerializeField, Range(0, 100), BoxGroup("SpawnDensity")] private int _ropeDensity;
        [SerializeField, Range(0, 1000)] private int _spawnDistance;
        private readonly WayMatrix _wayMatrix = new();
        private ChunkGenerator _chunkGenerator;
        private Quadcopter _quadcopter;
        private bool _isClientRequested;
        private Deliverer _deliverer;
        private Pizza _pizza;
        private PizzaEjector _pizzaEjector;
        private bool _isPizzaRequested;
        private Pool<Car> _carPool;
        private Pool<Bird> _birdPool;
        private Pool<WindowGuy> _windowGuyPool;
        private Pool<Battery> _batteryPool;
        private Pool<Client> _clientPool;
        private Pool<Rope> _ropePool;

        private void Awake() => _chunkGenerator = FindObjectOfType<ChunkGenerator>();

        private void OnEnable()
        {
            _chunkGenerator.OnChunkSpawned += OnChunkSpawned;
            GlobalSpeed.Instance.OnStartup += SpawnCars;
            GlobalSpeed.Instance.OnStartup += SpawnBirds;
            GlobalSpeed.Instance.OnStartup += SpawnRopes;
            GlobalSpeed.Instance.OnStop += StopAllCoroutines;
        }

        public void ResetEntities()
        {
            DispawnAll();
        }

        public void RequestPizza() => _isPizzaRequested = true;

        private void DispawnAll()
        {
            _carPool?.ReleaseAll();
            _birdPool?.ReleaseAll();
            _windowGuyPool?.ReleaseAll();
            _batteryPool?.ReleaseAll();
            _clientPool?.ReleaseAll();
            _ropePool?.ReleaseAll();
            _quadcopter.gameObject.SetActive(true);
        }

        public bool EnableQuadcopter(Container entityContainer, DefeatPanel defeatPanel, out Quadcopter quadcopter)
        {
            LifeDisplayer lifeCounter = FindObjectOfType<LifeDisplayer>();
            AdsRewardedButton rewardedButton = FindObjectOfType<AdsRewardedButton>();
            RestartLevelButton levelRestarter = FindObjectOfType<RestartLevelButton>(); 

            _quadcopter = new QuadcopterFactory(_quadcopterConfig, entityContainer,
                lifeCounter, defeatPanel, rewardedButton, levelRestarter).GetCreated();

            _deliverer = _quadcopter.GetComponent<Deliverer>();
            quadcopter = _quadcopter;
            return true;
        }

        public bool EnableCarTraffic(Container entityContainer)
        {
            _carPool = new Pool<Car>(new CarFactory(_carConfig), entityContainer, 10);
            return true;
        }

        public bool EnableBirds(Container entityContainer)
        {
            _birdPool = new Pool<Bird>(new BirdFactory(_birdConfig), entityContainer, 10);
            return true;
        }

        public bool EnableWindowGuys(Container entityContainer)
        {
            _windowGuyPool = new Pool<WindowGuy>(new WindowGuyFactory(_guyConfig), entityContainer, 10);
            return true;
        }

        public bool EnableBatteries(Container entityContainer)
        {
            _batteryPool = new Pool<Battery>(new BatteryFactory(_batteryConfig), entityContainer, 3);
            _quadcopter.GetComponent<Charger>().OnDecreased += SpawnBattery;
            return true;
        }

        public bool EnableDelivery(Container entityContainer, ChunkGenerator chunkGenerator)
        {
            _deliverer.OnPizzeriaRequested += RequestPizza;
            EnablePizza(entityContainer);
            EnablePizzaGuy(entityContainer, chunkGenerator);
            EnableClient(entityContainer);
            return true;
        }

        public bool EnableRopes(Container container)
        {
            _ropePool = new(new RopeFactory(_ropeConfig), container, 10);
            return true;
        }

        private void EnablePizza(Container entityContainer)
        {
            _pizza = new PizzaFactory(_pizzaConfig, _deliverer).GetCreated();
            _pizza.transform.SetParent(entityContainer.transform);
        }

        private void EnablePizzaGuy(Container entityContainer, ChunkGenerator chunkGenerator)
        {
            _pizzaEjector = new PizzaEjectorFactory(_pizzaEjectorConfig, _deliverer, _pizza).GetCreated();
            _pizzaEjector.transform.SetParent(entityContainer.transform);
            //chunkGenerator.OnPizzeriaSpawned += SpawnPizzaGuy;
        }

        private void EnableClient(Container entityContainer)
        {
            _clientPool = new Pool<Client>(new ClientFactory(_clientConfig, _deliverer), entityContainer, 10);
            _deliverer.OnPizzaGrabbed += () => _isClientRequested = true;
            _deliverer.OnDeliverySequenceFailed += () => _isClientRequested = false;
        }

        //private void SpawnPizzaGuy(PizzaDispensePoint dispensePoint)
        //{
        //    PizzaEjector pizzaEjector = _pizzaEjector;
        //    pizzaEjector.gameObject.SetActive(true);
        //    pizzaEjector.transform.position = (dispensePoint.transform.position);
        //    pizzaEjector.transform.eulerAngles = Vector3.up * (pizzaEjector.transform.position.x < 0 ? 180 : 0);
        //}

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
                if (_carsDensity > Random.Range(0, 100))
                {
                    Car car = _carPool.Get(instancePosition);
                    if (car.CarColorChanger != null) car.CarColorChanger.ChangeColorRandom();

                    float distanceBetweenCars = Random.Range(minDistance, maxDistance);
                    float speed = GlobalSpeed.Instance.Value + _carConfig.SelfSpeed;
                    float halfSize = car.Size / 2;
                    delay = (Mathf.Sqrt(speed * speed + 2
                        * GlobalSpeed.Instance.Acceleration
                        * (distanceBetweenCars + halfSize + previousHalfSize))
                        - speed) / GlobalSpeed.Instance.Acceleration;
                    previousHalfSize = halfSize;

                    yield return new WaitForSeconds(delay);

                    car.transform.position = spawnPosition;
                }
                else yield return new WaitForSeconds(delay);
            }
        }

        public void SpawnCars()
        {
            if (_carPool != null)
            {
                for (int line = 0; line < WayMatrix.Width; line++)
                    StartCoroutine(CarSpawning(line));
            }
        }

        private IEnumerator BirdsSpawning(int row, int line)
        {
            float delay = 0.5f;
            float maxDistance = 5f;
            float minDistance = 2f;

            while (true)
            {
                Vector3 position = _wayMatrix.GetPositionByArrayCoordinates(new Vector2Int(row, line));

                if (_birdsDensity > Random.Range(0, 100))
                {
                    _birdPool.Get(position + Vector3.forward * _spawnDistance);
                    float speed = GlobalSpeed.Instance.Value + _birdConfig.SelfSpeed;
                    float distanceBetween = Random.Range(minDistance, maxDistance);
                    float acceleration = GlobalSpeed.Instance.Acceleration;
                    delay = (Mathf.Sqrt(speed * speed + 2 * acceleration * (distanceBetween)) - speed) / acceleration;
                    yield return new WaitForSeconds(delay);
                }

                yield return new WaitForSeconds(delay);
            }
        }

        public void SpawnBirds()
        {
            int carRow = 1;
            if (_birdPool != null)
            {
                for (int line = 0; line < WayMatrix.Height - carRow; line++)
                    for (int row = 0; row < WayMatrix.Width; row++)
                        StartCoroutine(BirdsSpawning(row, line));
            } 
        }

        private void OnChunkSpawned(IEnumerable<Window> windows)
        {
            if (_isPizzaRequested)
            {
                //Spawn pizza car
            }

            foreach (Window window in windows)
            {
                if (Random.Range(0, 100) > _netGuysDensity)
                {
                    window.Close();
                    continue;
                }

                if (_clientPool != null && _isClientRequested)
                {
                    Client client = _clientPool.Get(window.GetSpawnPoint());
                    client.GetComponentInChildren<Animator>().SetFloat(AnimationService.Parameters.Side, Mathf.Clamp(client.transform.position.x, -1, 1));
                    _isClientRequested = false;
                }
                else
                {
                    _windowGuyPool?.Get(window.GetSpawnPoint());
                }

                window.Open();
            }
        }

        private void SpawnBattery()
        {
            if (_batteryPool != null)
            {
                _batteryPool.Get(_wayMatrix.GetRandomPosition() + Vector3.forward * WayMatrix.Horizon);
            }
        }

        private void SpawnRopes()
        {
            if (_ropePool != null)
            {
                int carLine = 1;

                for (int i = 0; i < WayMatrix.Height - carLine; i++)
                {
                    StartCoroutine(RopeSpawning(i));
                }
            }
        }

        private IEnumerator RopeSpawning(int line)
        {
            float delay = 0.5f;
            float maxDistance = 5f;
            float minDistance = 2f;
            Vector3 position = _wayMatrix.GetPositionByArrayCoordinates(new Vector2Int(0, line));
            position.x = 0;
            position.z = WayMatrix.Horizon;

            while (true)
            {
                if (_ropeDensity > Random.Range(0, 100))
                {
                    _ropePool.Get(position);
                    float speed = GlobalSpeed.Instance.Value + _ropeConfig.SelfSpeed;
                    float distanceBetween = Random.Range(minDistance, maxDistance);
                    float acceleration = GlobalSpeed.Instance.Acceleration;
                    delay = (Mathf.Sqrt(speed * speed + 2 * acceleration * (distanceBetween)) - speed) / acceleration;
                    yield return new WaitForSeconds(delay);
                }

                yield return new WaitForSeconds(delay);
            }
        }

        private void OnDisable()
        {
            _chunkGenerator.OnChunkSpawned -= OnChunkSpawned;
            GlobalSpeed.Instance.OnStartup -= SpawnCars;
            GlobalSpeed.Instance.OnStartup -= SpawnBirds;
            GlobalSpeed.Instance.OnStartup -= SpawnRopes;
            GlobalSpeed.Instance.OnStop -= StopAllCoroutines;
        }
    }
}
