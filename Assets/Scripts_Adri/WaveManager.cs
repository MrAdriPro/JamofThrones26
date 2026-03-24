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
    private bool spawning = false;

    private List<int> eraChangeRounds = new List<int>();
    private int currentPointIndex = 0;

    private void Start()
    {
        actualRound = 0;
        currentPointIndex = 0;

        for (int i = 0; i < waves.Count; i++)
        {
            if (waves[i].enemiesInWave.Any(e => e.enemyType != null && e.enemyType.isBoss))
            {
                eraChangeRounds.Add(i);
            }
        }

        StartCoroutine(SpawnRound());
    }

    bool IsBossAlive()
    {
        return GameObject.FindGameObjectsWithTag("Enemy")
            .Any(e => e.GetComponent<EnemyController>()?.data != null &&
                      e.GetComponent<EnemyController>().data.isBoss);
    }

    IEnumerator SpawnRound()
    {
        spawning = true;

        Wave_SO dataWave = waves[actualRound];

        bool hasBoss = dataWave.enemiesInWave.Any(e =>
            e.enemyType != null && e.enemyType.isBoss
        );

        Debug.Log("Ronda: " + actualRound);

        foreach (var entry in dataWave.enemiesInWave)
        {
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

                yield return new WaitForSeconds(entry.spawnInterval);
            }
        }

        if (hasBoss)
        {
            Debug.Log("Boss activo, esperando...");
            yield return new WaitUntil(() => !IsBossAlive());
            Debug.Log("Boss muerto, continuar...");
        }

        int nextPoint = currentPointIndex + 1;

        Vector3 startPos = eraUI.GetPoint(currentPointIndex);
        Vector3 endPos = eraUI.GetPoint(nextPoint);

        int startRound = (currentPointIndex == 0) ? 0 : eraChangeRounds[currentPointIndex - 1] + 1;
        int endRound = (currentPointIndex < eraChangeRounds.Count) ? eraChangeRounds[currentPointIndex] : waves.Count - 1;

        float t = (float)(actualRound - startRound + 1) / (endRound - startRound + 1);
        t = Mathf.Clamp01(t);

        Vector3 targetPos = Vector3.Lerp(startPos, endPos, t);

        eraUI.MoveCursorToPosition(targetPos, 0.3f);

        if (hasBoss)
        {
            currentPointIndex++;

            if (currentPointIndex > eraChangeRounds.Count)
            {
                Debug.Log("FIN DEL JUEGO");
            }
        }

        actualRound++;
        spawning = false;

        if (actualRound < waves.Count)
        {
            StartCoroutine(SpawnRound());
        }
    }
}