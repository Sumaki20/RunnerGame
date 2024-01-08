using System.Collections;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

    [SerializeField] GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile(bool spawnItems)
    {
        StartCoroutine(DestroyAfterTime());
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;

        if (spawnItems)
        {
            temp.GetComponent<GroundTile>().SpawnObstacle();
            temp.GetComponent<GroundTile>().SpawnObstacleTwo();
            temp.GetComponent<GroundTile>().SpawnEnemy();
            temp.GetComponent<GroundTile>().SpawnCoins();
            temp.GetComponent<GroundTile>().SpawnHeal();
            temp.GetComponent<GroundTile>().SpawnEnemyMelee();
            temp.GetComponent<GroundTile>().SpawnStamina();
            //temp.GetComponent<GroundTile>().SpawnSpeed();
        }
    }
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(2f);

    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < 2)
            {
                SpawnTile(false);
            }
            else
            {
                SpawnTile(true);
            }
        }
    }
}