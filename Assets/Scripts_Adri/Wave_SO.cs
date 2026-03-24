using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSpawnEntry
{
    public Enemy_SO enemyType;

    public int count = 1;

    public float spawnInterval = 0.5f;

    public float initialDelay = 0f;
}

[CreateAssetMenu(fileName = "New Wave", menuName = "New Wave/Wave_SO")]
public class Wave_SO : ScriptableObject
{
    public int waveNumber;
    public List<WaveSpawnEntry> enemiesInWave = new List<WaveSpawnEntry>();

    public float timeAfterWave = 2f;
}



