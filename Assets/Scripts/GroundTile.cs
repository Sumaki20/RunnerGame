using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject tallObstaclePrefab;
    [SerializeField] GameObject healPrefab;

    [SerializeField] float tallObstacleChance = 0.2f;
    [SerializeField] float healChance = 0.1f;
    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile(true);
        Destroy(gameObject, 5);
    }
    public void SpawnHeal()
    {
        // Choose a random point to spawn the obstacle
        int itemSpawnIndex = Random.Range(5, 11);
        Transform spawnPoint = transform.GetChild(itemSpawnIndex).transform;

        // Check if the spawn point is occupied by an obstacle
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.5f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newObstacleSpawnIndex = Random.Range(2, 11);
                spawnPoint = transform.GetChild(newObstacleSpawnIndex).transform;
                break;
            }
        }
        Vector3 healOffset = new Vector3(0f, 1f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 180f, 0f) * healOffset;
        Vector3 healPosition = spawnPoint.position + rotatedOffset;

        float ranspwan = Random.Range(0f, 1f);
        if (ranspwan < healChance)
        {
            Instantiate(healPrefab, healPosition, Quaternion.identity, transform);
        }
        
    }

    public void SpawnObstacle()
    {
        // Choose which obstacle to spawn
        GameObject obstacleToSpawn = obstaclePrefab;

        float random = Random.Range(0f, 1f);
        if(random < tallObstacleChance)
        {
            obstacleToSpawn = tallObstaclePrefab;
        }

        // Choose a random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        Vector3 offset = new Vector3(0f, 0f, 0f);
        Vector3 newPosition = spawnPoint.position + offset;
        
        Instantiate(obstacleToSpawn, newPosition, Quaternion.identity, transform);
    }


    public void SpawnCoins()
    {
        int coinsToSpawn = 5;
        for (int i = 0; i < coinsToSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }
    }

    public void SpawnEnemy()
    {
        // Choose a random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        // Check if the spawn point is occupied by an obstacle
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.5f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newObstacleSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newObstacleSpawnIndex).transform;
                break;
            }
        }
        Vector3 enemyOffset = new Vector3(0f, 1f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 180f, 0f) * enemyOffset;
        Vector3 enemyPosition = spawnPoint.position + rotatedOffset;

        // Spawn the obstace at the position
        Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, transform);
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }

        point.y = 0.5f;
        return point;
    }
}