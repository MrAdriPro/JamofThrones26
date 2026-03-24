using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform bulletTarget;
    public float bulletSpeed;

    void Update()
    {
        if (bulletTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, bulletTarget.position, bulletSpeed * Time.deltaTime);

        transform.LookAt(bulletTarget.position);

        if (Vector3.Distance(transform.position, bulletTarget.position) < 0.2f)
        {
            Destroy(gameObject);
        }
    }
}
