using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public class EnemyTypeData
{
    public GameObject enemyPrefab;
    public int count;
}
[System.Serializable]
public class WaveData
{
    public List<EnemyTypeData> enemyTypes;
    public float spawnDelay;
    public float waveDelay;
    public GameObject bossPrefab;
}
public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    [SerializeField] private int maxEnemiesAlive = 10;

    [Space]
    [SerializeField] private float maxDistanceFromPlayer = 15f;
    [SerializeField] private float spawnRadius = 15f;

    [Space]
    [SerializeField] private EnemyBossUI enemybossUI;

    private int currentWaveIndex = 0;

    public List<NetworkObject> ActiveEnemies => activeEnemies;
    private List<NetworkObject> activeEnemies = new List<NetworkObject>();
    private Transform playerTransform;

    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playerTransform = NetworkManager.Singleton.
                ConnectedClients[0].PlayerObject.transform;
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {   
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;
            yield return new WaitForSeconds(waves[currentWaveIndex - 1].waveDelay);
        }

    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var enemyType in wave.enemyTypes)
        {
            int spawnedEnemiesOfType = 0;
            while (spawnedEnemiesOfType < enemyType.count)
            {
                if (activeEnemies.Count < maxEnemiesAlive)
                {
                    SpawnEnemy(enemyType.enemyPrefab);
                    spawnedEnemiesOfType++;
                }
                yield return new WaitForSeconds(wave.spawnDelay);
            }
        }

        if (wave.bossPrefab != null)
        {
            SpawnEnemy(wave.bossPrefab);
            ShowBossTextClientRpc();
        }
    }

    [ClientRpc]
    private void ShowBossTextClientRpc()
    {
        enemybossUI.ShowBoss();
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPoint = GetRandomPointAroundPlayer();
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        NetworkObject networkObject = enemyInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn();
            activeEnemies.Add(networkObject);
        }
    }

    private Vector3 GetRandomPointAroundPlayer()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float offsetX = Mathf.Cos(angle) * spawnRadius;
        float offsetY = Mathf.Sin(angle) * spawnRadius;

        Vector3 randomPoint = new Vector3(playerTransform.position.x + offsetX,
            playerTransform.position.y + offsetY, playerTransform.position.z);

        return randomPoint;
    }

    private void Update()
    {
        if (!IsServer || playerTransform == null) 
            return;

        for (int i = activeEnemies.Count - 1; i >=0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            Transform enemyTransform = activeEnemies[i].transform;
            float distanceToPlayer = Vector3.Distance(enemyTransform.position,
                playerTransform.position);
            if (distanceToPlayer > maxDistanceFromPlayer)
            {
                Vector3 directionToPlayer = (playerTransform.position 
                    - enemyTransform.position).normalized;
                enemyTransform.position = playerTransform.position - directionToPlayer
                    * (maxDistanceFromPlayer - 5f);
            }
        }
    }
}
