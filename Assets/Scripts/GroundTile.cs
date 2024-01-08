using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyMeleePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject tallObstaclePrefab;
    [SerializeField] GameObject obstacleSlide;
    [SerializeField] GameObject healPrefab;
    [SerializeField] GameObject staminaPrefab;
    //[SerializeField] GameObject speedPrefab;

    [SerializeField] float tallObstacleChance = 0.7f;
    [SerializeField] float obstacleSlideChance = 0.3f;
    [SerializeField] float healChance = 0.1f;
    [SerializeField] float staminaChance = 0.1f;
    //[SerializeField] float speedChance = 0.1f;
    [SerializeField] int coinsToSpawn = 1;
    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            groundSpawner.SpawnTile(true);
            StartCoroutine(DestroyAfterTime());
        }
        
    }
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5f);
        
        Destroy(gameObject);
    }
    /*public void SpawnSpeed()
    {
        // Choose a random point to spawn the obstacle
        int itemSpawnIndex = Random.Range(3, 23);
        Transform spawnPoint = transform.GetChild(itemSpawnIndex).transform;

        // Check if the spawn point is occupied by an obstacle
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newSpeedSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newSpeedSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newSpeedSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newSpeedSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Coin"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newSpeedSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newSpeedSpawnIndex).transform;
                break;
            }
        }
        Vector3 speedOffset = new Vector3(0f, 1f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 180f, 0f) * speedOffset;
        Vector3 speedPosition = spawnPoint.position + rotatedOffset;

        float ranspwan = Random.Range(0f, 1f);
        if (ranspwan < speedChance)
        {
            Instantiate(speedPrefab, speedPosition, Quaternion.identity, transform);
        }

    }*/
    public void SpawnHeal()
    {
        // Choose a random point to spawn the obstacle
        int itemSpawnIndex = Random.Range(3, 23);
        Transform spawnPoint = transform.GetChild(itemSpawnIndex).transform;

        // Check if the spawn point is occupied by an obstacle
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newHealSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newHealSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newHealSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newHealSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Coin"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
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
    public void SpawnStamina()
    {
        // Choose a random point to spawn the obstacle
        int itemSpawnIndex = Random.Range(3, 23);
        Transform spawnPoint = transform.GetChild(itemSpawnIndex).transform;

        // Check if the spawn point is occupied by an obstacle
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newHealSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newHealSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newHealSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newHealSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Coin"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
        }
        Vector3 healOffset = new Vector3(0f, 1f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 180f, 0f) * healOffset;
        Vector3 healPosition = spawnPoint.position + rotatedOffset;

        float ranspwan = Random.Range(0f, 1f);
        if (ranspwan < staminaChance)
        {
            Instantiate(staminaPrefab, healPosition, Quaternion.identity, transform);
        }

    }

    public void SpawnCoins()
    {
        int itemSpawnIndex = Random.Range(3, 23);
        Transform spawnPoint = transform.GetChild(itemSpawnIndex).transform;

        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Item"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(3, 23);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
        }
        Vector3 coinOffset = new Vector3(0f, 0.5f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 0f, 0f) * coinOffset;
        Vector3 coinPosition = spawnPoint.position + rotatedOffset;

        for (int i = 0; i < coinsToSpawn; i++)
        {
            Instantiate(coinPrefab, coinPosition, Quaternion.identity, transform);
        }
    }

    /*public void SpawnObstacleSlide()
    {
        // Choose which obstacle to spawn
        GameObject obstacleToSpawn = obstacleSlide;

        float random = Random.Range(0f, 1f);
        if (random < tallObstacleChance)
        {
            obstacleToSpawn = tallObstaclePrefab;
        }

        // Choose a random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;


        Vector3 offset = new Vector3(0f, 0f, 0f);
        Vector3 newPosition = spawnPoint.position + offset;


        Instantiate(obstacleToSpawn, newPosition, Quaternion.identity, transform);
    }*/
    public void SpawnObstacleTwo()
    {
        // Choose which obstacle to spawn
        GameObject obstacleToSpawn = obstaclePrefab;

        float random = Random.Range(0f, 1f);
        if (random < obstacleSlideChance)
        {
            obstacleToSpawn = obstacleSlide;
        }
        // Choose a random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Item"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
        }
        Vector3 offset = new Vector3(0f, 0f, 0f);
        Vector3 newPosition = spawnPoint.position + offset;


        Instantiate(obstacleToSpawn, newPosition, Quaternion.identity, transform);
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

        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f); // 1f is the radius, adjust as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Item"))
            {
                // Spawn point is occupied, choose a new spawn point
                int newCoinSpawnIndex = Random.Range(2, 5);
                spawnPoint = transform.GetChild(newCoinSpawnIndex).transform;
                break;
            }
        }
        Vector3 offset = new Vector3(0f, 0f, 0f);
        Vector3 newPosition = spawnPoint.position + offset;

        
        Instantiate(obstacleToSpawn, newPosition, Quaternion.identity, transform);
    }

    public void SpawnEnemyMelee()
    {
        int[] coinLeft = { 3, 6, 7, 8, 9, 10, 11 };
        int[] coinMid = { 4, 12, 13, 14, 15, 16, 17 };
        int[] coinRight = { 5, 18, 19, 20, 21, 22, 23 };

        int[][] rowCoin = { coinLeft, coinMid, coinRight };
        int itemSpawnIndex = Random.Range(0, rowCoin.Length);
        int[] selectedRow = rowCoin[itemSpawnIndex];


        List<int> possibleIndices = new List<int> { 6, 12, 18 };

        int enemySpawnIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
        Transform spawnPoint = transform.GetChild(enemySpawnIndex).transform;
        foreach (int checkSpawnIndex in selectedRow)
        {
            Transform checkSpawnPoint = transform.GetChild(checkSpawnIndex).transform;

            // Check if the spawn point is occupied by an obstacle
            Collider[] colliders = Physics.OverlapSphere(checkSpawnPoint.position, 0.3f); // 1f is the radius, adjust as needed
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Obstacle"))
                {
                    spawnPoint = transform.GetChild(enemySpawnIndex).transform;
                    break;
                }
                if (collider.CompareTag("Enemy"))
                {
                    // Spawn point is occupied, choose a new spawn point
                    spawnPoint = transform.GetChild(enemySpawnIndex).transform;
                    break;
                }
            }
        }
        Vector3 enemyOffset = new Vector3(0f, 0f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 0f, 0f) * enemyOffset;
        Vector3 enemyPosition = spawnPoint.position + rotatedOffset;

        // Spawn the obstace at the position
        Instantiate(enemyMeleePrefab, enemyPosition, Quaternion.identity, transform);
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
        Vector3 enemyOffset = new Vector3(0f, 0f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 180f, 0f) * enemyOffset;
        Vector3 enemyPosition = spawnPoint.position + rotatedOffset;

        // Spawn the obstace at the position
        Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, transform);
    }
    /*int[] coinLeft = { 3, 6, 7, 8, 9, 10, 11 };
    int[] coinMid = { 4, 12, 13, 14, 15, 16, 17 };
    int[] coinRight = { 5, 18, 19, 20, 21, 22, 23 };

    public void SpawnCoins()
    {
        int[][] rowCoin = { coinLeft, coinMid, coinRight };
        int itemSpawnIndex = Random.Range(0, rowCoin.Length);
        int[] selectedRow = rowCoin[itemSpawnIndex];

        foreach (int spawnPointIndex in selectedRow)
        {
            Transform spawnPoint = transform.GetChild(spawnPointIndex).transform;

            // Check if the spawn point is occupied
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.5f);
            bool spawnPointOccupied = colliders.Any(collider => collider.CompareTag("Obstacle"));

            Vector3 coinOffset = new Vector3(0f, spawnPointOccupied ? 2f : 0.5f, 0f);
            Vector3 rotatedOffset = Quaternion.Euler(0f, 0f, 0f) * coinOffset;
            Vector3 coinPosition = spawnPoint.position + rotatedOffset;

            Instantiate(coinPrefab, coinPosition, Quaternion.identity, transform);
        }
    }*/


    /*public void SpawnEnemyMelee()// randomspawn and check other spawn
    {
        // Define the rows where enemies can spawn
        int[] enemyLeft = { 3, 6, 7, 8, 9, 10, 11 };
        int[] enemyMid = { 4, 12, 13, 14, 15, 16, 17 };
        int[] enemyRight = { 5, 18, 19, 20, 21, 22, 23 };

        int[][] rowEnemies = { enemyLeft, enemyMid, enemyRight };

        // Choose a random row for enemy spawn
        int selectedRowIndex = Random.Range(0, rowEnemies.Length);
        int[] selectedRow = rowEnemies[selectedRowIndex];

        // Choose a random column within the selected row
        int selectedColumnIndex = Random.Range(0, selectedRow.Length);
        int enemySpawnIndex = selectedRow[selectedColumnIndex];

        // Get the spawn point transform
        Transform spawnPoint = transform.GetChild(enemySpawnIndex).transform;

        // Check if the spawn point is occupied by an enemy
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Spawn point is occupied, choose a new spawn point
                selectedRowIndex = Random.Range(0, rowEnemies.Length);
                selectedRow = rowEnemies[selectedRowIndex];
                selectedColumnIndex = Random.Range(0, selectedRow.Length);
                enemySpawnIndex = selectedRow[selectedColumnIndex];
                spawnPoint = transform.GetChild(enemySpawnIndex).transform;
                break;
            }
            if (collider.CompareTag("Obstacle"))
            {
                // Spawn point is occupied, choose a new spawn point
                selectedRowIndex = Random.Range(0, rowEnemies.Length);
                selectedRow = rowEnemies[selectedRowIndex];
                selectedColumnIndex = Random.Range(0, selectedRow.Length);
                enemySpawnIndex = selectedRow[selectedColumnIndex];
                spawnPoint = transform.GetChild(enemySpawnIndex).transform;
                break;
            }
        }

        Vector3 enemyOffset = new Vector3(0f, 0f, 0f);
        Vector3 rotatedOffset = Quaternion.Euler(0f, 0f, 0f) * enemyOffset;
        Vector3 enemyPosition = spawnPoint.position + rotatedOffset;

        // Spawn the enemy at the position
        Instantiate(enemyMeleePrefab, enemyPosition, Quaternion.identity, transform);
    }*/


    /*Vector3 GetRandomPointInCollider(Collider collider)
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
    }*/
    /*for (int i = 0; i < coinsToSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }*/
}