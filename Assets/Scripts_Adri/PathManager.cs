using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager instance;
    public Transform[] pathPoints;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
        pathPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pathPoints[i] = transform.GetChild(i);
        }
    }

    
}
