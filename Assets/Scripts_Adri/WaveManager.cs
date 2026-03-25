using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
<<<<<<< HEAD
=======
using UnityEngine.UI;
>>>>>>> parent of b2b8bd1 (Merge branch 'Adrian_DEV' into Sergio)

public class WaveManager : MonoBehaviour
{
    public List<Wave_SO> waves = new List<Wave_SO>();
    public Transform[] spawnPoints;
    public EraProgresUI eraUI;

    private int actualRound = 0;

<<<<<<< HEAD
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
=======
    [Header("UI")]
    public EraUIController eraUIController;
    public List<string> eras = new List<string>() { "Prehistoria", "Medievo", "SteamPunk", "Futurista" };

    private void Start()
    {
        if (eraUIController != null)
        {
            eraUIController.SetupEras(eras, waves.Count);
            eraUIController.UpdateForRound(actualRound);
        }
        if (autoStart) StartCoroutine(DelayedStart(autoStartDelay));
    }
    /// <summary>
    /// Just a simple method to delay the start of the first round if autoStart is enabled. It waits for the specified delay and then starts the first round if we are not already spawning.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!spawning) StartCoroutine(SpawnRound());
>>>>>>> parent of b2b8bd1 (Merge branch 'Adrian_DEV' into Sergio)
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

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned.");
            spawning = false;
            yield break;
        }

        List<GameObject> spawnedThisRound = new List<GameObject>();

        foreach (var entry in dataWave.enemiesInWave)
        {
<<<<<<< HEAD
            if (entry.enemyType != null && entry.enemyType.isBoss)
            {
                hasBoss = true;
            }
=======
            if (entry.initialDelay > 0f) yield return new WaitForSeconds(entry.initialDelay);
>>>>>>> parent of b2b8bd1 (Merge branch 'Adrian_DEV' into Sergio)

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
            eraUI.SetShaking(true);
            eraUI.StartBossFlash(currentEraIndex + 1);
            UpdateUIProgress(forceMax: true);

            yield return new WaitUntil(() => !IsBossAlive());
            Debug.Log("boss negro muerto");
            eraUI.SetShaking(false);
            eraUI.StopFlash();

            eraUI.SetEraCompleted(currentEraIndex);

            if (currentEraIndex + 1 < eraUI.eraTexts.Length)
            {
                eraUI.eraTexts[currentEraIndex + 1].color = eraUI.normalColor;
            }

            currentEraIndex++;

            if (currentEraIndex >= eraUI.eraPoints.Length)
            {
                Debug.Log("nigger");
                eraUI.SetEraCompleted(currentEraIndex);

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

<<<<<<< HEAD
        if (actualRound < waves.Count)
=======
        if (dataWave.timeAfterWave > 0f) yield return new WaitForSeconds(dataWave.timeAfterWave);

        actualRound++;
        if (eraUIController != null)
        {
            eraUIController.UpdateForRound(actualRound);
        }
        spawning = false;
        if(actualRound < waves.Count)
>>>>>>> parent of b2b8bd1 (Merge branch 'Adrian_DEV' into Sergio)
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