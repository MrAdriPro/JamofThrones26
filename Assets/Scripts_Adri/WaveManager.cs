using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave_SO> waves = new List<Wave_SO>();
    public Transform[] spawnPoints;

    [Header("Configuración")]
    public bool autoStart = false;
    public float autoStartDelay = 2f;

    private int actualRound = 0;
    private bool spawning = false;

    private void Start()
    {
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spawning && actualRound < waves.Count)
        {
            StartCoroutine(SpawnRound());
        }
    }
    /// <summary>
    /// Here we handle the spawning of enemies for each round. We iterate through the list of enemies defined in the current wave, spawn them at random spawn points, and wait for the specified intervals between spawns. 
    /// After all enemies are spawned, we wait until all enemies are defeated before proceeding to the next round.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnRound()
    {
        spawning = true;
        Wave_SO dataWave = waves[actualRound];
        print($"Iniciando Ronda: {actualRound + 1}");

        foreach (var entry in dataWave.enemiesInWave)
        {
            if (entry.initialDelay > 0f) yield return new WaitForSeconds(entry.initialDelay);

            for (int i = 0; i < entry.count; i++)
            {
                GameObject prefabToSpawn = entry.enemyType?.enemyPrefab;

                if (prefabToSpawn != null)
                {
                    GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

                    newEnemy.SetActive(true);

                    if (newEnemy.TryGetComponent<EnemyController>(out var controller))
                    {
                        controller.data = entry.enemyType;
                        controller.Initialize();
                    }
                }

                yield return new WaitForSeconds(entry.spawnInterval);
            }
        }

        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return new WaitForSeconds(1f); 
        }

        print($"Ronda {actualRound + 1} despejada.");

        if (dataWave.timeAfterWave > 0f) yield return new WaitForSeconds(dataWave.timeAfterWave);

        actualRound++;
        spawning = false;
        if(actualRound < waves.Count)
        {
            StartCoroutine(SpawnRound());
            print($"Preparando Ronda: {actualRound + 1}");
        }
    }
}

