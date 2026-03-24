using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public List<Wave_SO> waves = new List<Wave_SO>();
    public Transform[] spawnPoints;
    public EraProgresUI eraUI;

    private int actualRound = 0;

    private int currentEraIndex = 0;
    private int totalEnemiesInCurrentEra = 0;
    private int enemiesSpawnedInCurrentEra = 0;

    private void Start()
    {
        actualRound = 0;
        currentEraIndex = 0;

        eraUI.cursor.position = eraUI.GetPoint(0);

        CalculateEnemiesForCurrentEra();
        StartCoroutine(SpawnRound());
    }

    void CalculateEnemiesForCurrentEra()
    {
        totalEnemiesInCurrentEra = 0;
        enemiesSpawnedInCurrentEra = 0;

        for (int i = actualRound; i < waves.Count; i++)
        {
            Wave_SO wave = waves[i];

            bool hasBoss = wave.enemiesInWave.Any(e => e.enemyType != null && e.enemyType.isBoss);

            foreach (var entry in wave.enemiesInWave)
            {
                if (entry.enemyType != null && !entry.enemyType.isBoss)
                {
                    totalEnemiesInCurrentEra += entry.count;
                }
            }

            if (hasBoss) break;
        }
    }

    bool IsBossAlive()
    {
        return GameObject.FindGameObjectsWithTag("Enemy")
            .Any(e => e.GetComponent<EnemyController>()?.data != null &&
                      e.GetComponent<EnemyController>().data.isBoss);
    }

    IEnumerator SpawnRound()
    {
        if (actualRound >= waves.Count) yield break;

        Wave_SO dataWave = waves[actualRound];
        bool hasBoss = false;

        Debug.Log("Iniciando Ronda: " + actualRound);

        foreach (var entry in dataWave.enemiesInWave)
        {
            if (entry.enemyType != null && entry.enemyType.isBoss)
            {
                hasBoss = true;
            }

            if (entry.initialDelay > 0f)
                yield return new WaitForSeconds(entry.initialDelay);

            for (int i = 0; i < entry.count; i++)
            {
                GameObject prefabToSpawn = entry.enemyType?.enemyPrefab;
                if (prefabToSpawn != null)
                {
                    GameObject newEnemy = Instantiate(
                        prefabToSpawn,
                        spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                        Quaternion.identity
                    );
                    newEnemy.SetActive(true);

                    if (newEnemy.TryGetComponent<EnemyController>(out var controller))
                    {
                        controller.data = entry.enemyType;
                    }
                }

                if (entry.enemyType != null && !entry.enemyType.isBoss)
                {
                    enemiesSpawnedInCurrentEra++;
                    UpdateUIProgress();
                }

                yield return new WaitForSeconds(entry.spawnInterval);
            }
        }

        if (hasBoss)
        {
            Debug.Log("boss spawning");
            UpdateUIProgress(forceMax: true);

            yield return new WaitUntil(() => !IsBossAlive());
            Debug.Log("boss negro muerto");

            currentEraIndex++;

            if (currentEraIndex >= eraUI.eraPoints.Length)
            {
                Debug.Log("nigger");
                //win
                yield break; 
            }

            actualRound++;
            CalculateEnemiesForCurrentEra();
        }
        else
        {
            actualRound++;
        }

        yield return new WaitForSeconds(dataWave.timeAfterWave);

        if (actualRound < waves.Count)
        {
            StartCoroutine(SpawnRound());
        }
    }

    void UpdateUIProgress(bool forceMax = false)
    {
        Vector3 startPos = eraUI.GetPoint(currentEraIndex);

        Vector3 endPos = (currentEraIndex + 1 < eraUI.eraPoints.Length)
                         ? eraUI.GetPoint(currentEraIndex + 1)
                         : eraUI.finalPoint.position;

        float progress = 0f;

        if (forceMax || totalEnemiesInCurrentEra == 0)
        {
            progress = 1f;
        }
        else
        {
            progress = (float)enemiesSpawnedInCurrentEra / totalEnemiesInCurrentEra;
        }

        Vector3 targetPos = Vector3.Lerp(startPos, endPos, progress);
        eraUI.MoveCursorToPosition(targetPos, 1); 
    }
}