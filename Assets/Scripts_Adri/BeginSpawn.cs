using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BeginSpawn : MonoBehaviour
{
    public Enemy_SO enemyType;
    public float delay = 0.5f;

}
[CreateAssetMenu(fileName = "New Wave", menuName = "New Wave/Wave_SO")]
public class Wave_SO : ScriptableObject
{
    public int waveNumber;
    public List<BeginSpawn> enemiesInWave;

}



