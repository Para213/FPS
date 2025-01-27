using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; 
    public GameObject[] enemySpawnPoints; 
    public GameObject enemyPrefab; 
    public RoundUI roundUI; 
    public GameObject player; 
    public GameObject[] weaponPrefabs; 
    public GameObject[] ammoPrefabs; 
    public GameObject[] enemyPrefabs;

    public AudioClip helicopterAudioClip;
    public AudioClip weaponDropAudioClip;
    public AudioClip ammoDropAudioClip;

    private int currentRound = 0; 
    private int enemiesLeft; 
    private int totalEnemiesToSpawn; 
    private bool isRoundInProgress = false; 
    private bool hasGameStarted = false; 
    private int enemiesDefeatedCount = 0; 
    private int enemiesToDropSupply = 10; 
    private int supplyCounter = 0; 
    private int cycleCount = 0;
    private int spawnCounter = 0; 

    private List<GameObject> allWeaponsCollected = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) 
        {
            Destroy(gameObject);
        }
    }

    public void PlayerEnteredStart() 
    {
        if (!hasGameStarted) 
        {
            hasGameStarted = true; 
            BeginRound(); 
        }
    }

    private Vector3 GetRandomSpawnPositionNearPlayer(float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        Vector3 spawnPosition = player.transform.position + randomDirection;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, distance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return spawnPosition;
    }

    private void DropSupply()
    {
        Vector3 spawnPosition = GetRandomSpawnPositionNearPlayer(4);
        spawnPosition += Vector3.up * 5;

        Debug.Log("Playing helicopter sound");
        AudioSource.PlayClipAtPoint(helicopterAudioClip, spawnPosition);

        if (cycleCount == 0)
        {
            switch (supplyCounter)
            {
                case 0:
                    DropWeapon(2, spawnPosition);
                    break;
                case 1:
                    DropAmmo(spawnPosition);
                    break;
                case 2:
                    DropWeapon(4, spawnPosition);
                    break;
                case 3:
                    DropWeapon(3, spawnPosition);
                    break;
                case 4:
                    DropAmmo(spawnPosition);
                    break;
                case 5:
                    DropWeapon(5, spawnPosition);
                    break;
                case 6:
                    DropAmmo(spawnPosition);
                    break;
                case 7:
                    DropWeapon(6, spawnPosition);
                    break;
                case 8:
                    DropAmmo(spawnPosition);
                    break;
                case 9:
                    DropAmmo(spawnPosition);
                    break;
                case 10:
                    DropWeapon(7, spawnPosition);
                    DropAmmo(spawnPosition);
                    break;
                case 11:
                    DropAmmo(spawnPosition);
                    break;
                case 12:
                    DropAmmo(spawnPosition);
                    break;
            }
            supplyCounter = (supplyCounter + 1) % 12;

            
            if (supplyCounter == 0)
            {
                cycleCount++;
            }
        }
        else 
        {
            DropAmmo(spawnPosition);
        }

    }

    private void DropWeapon(int index, Vector3 spawnPosition)
    {
        GameObject weaponToDrop = weaponPrefabs[index];
        GameObject droppedWeapon = Instantiate(weaponToDrop, spawnPosition, Quaternion.identity);
        droppedWeapon.GetComponent<Rigidbody>().velocity = Vector3.down;
        StartCoroutine(PlayDelayedAudio(weaponDropAudioClip, spawnPosition, 2f));
    }

    private void DropAmmo(Vector3 spawnPosition)
    {
        GameObject ammoToDrop = GetRandomAmmoDrop();
        GameObject droppedAmmo = Instantiate(ammoToDrop, spawnPosition, Quaternion.identity);
        droppedAmmo.GetComponent<Rigidbody>().velocity = Vector3.down;
        StartCoroutine(PlayDelayedAudio(ammoDropAudioClip, spawnPosition, 2f));
    }

    private GameObject GetRandomAmmoDrop()
    {
        int randomIndex = -1;
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < 0.7f) 
        {
            randomIndex = 0; 
        }
        else if (randomValue < 0.85f) 
        {
            randomIndex = 1; 
        }
        else 
        {
            randomIndex = 2; 
        }

        return ammoPrefabs[randomIndex];
    }

    private IEnumerator PlayDelayedAudio(AudioClip audioClip, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Playing drop sound");
        AudioSource.PlayClipAtPoint(audioClip, position);
    }

    private void BeginRound() 
    {
        if (!isRoundInProgress && hasGameStarted) 
        {
            currentRound++; 
            StartCoroutine(roundUI.ShowRoundText(currentRound)); 
            totalEnemiesToSpawn = 1 + (currentRound - 1) * 2; 
            enemiesLeft = totalEnemiesToSpawn; 
            StartCoroutine(SpawnEnemies()); 
            isRoundInProgress = true; 
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesLeft > 0) 
        {
            foreach (GameObject spawnPoint in enemySpawnPoints) 
            {
                
                spawnCounter++;

                
                GameObject enemyPrefabToSpawn;

                if (spawnCounter % 15 == 0 && spawnCounter != 0) 
                {
                    enemyPrefabToSpawn = enemyPrefabs[3]; // Boss
                }
                else if (spawnCounter % 10 == 0 && spawnCounter != 0) 
                {
                    enemyPrefabToSpawn = enemyPrefabs[2]; // Elite
                }
                else if (spawnCounter % 5 == 0 && spawnCounter != 0) 
                {
                    enemyPrefabToSpawn = enemyPrefabs[1]; // Minion
                }
                else
                {
                    enemyPrefabToSpawn = enemyPrefabs[0]; // Normal
                }

                
                Instantiate(enemyPrefabToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);

                enemiesLeft--; 

                if (enemiesLeft <= 0) break; 
            }
            yield return new WaitForSeconds(1); 
        }
        spawnCounter = 0; 
    }

    public void EnemyDefeated() 
    {
        totalEnemiesToSpawn--; 

        enemiesDefeatedCount++;
        if (enemiesDefeatedCount >= enemiesToDropSupply)
        {
            DropSupply();
            enemiesDefeatedCount = 0;
        }

        if (totalEnemiesToSpawn <= 0) 
        {
            isRoundInProgress = false; 
        }
    }

    private void Update() 
    {
        if (hasGameStarted && !isRoundInProgress) 
        {
            BeginRound(); 
        }
    }
}
