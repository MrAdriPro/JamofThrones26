using System;
using UnityEngine;

public class BulletController : PoolEntity
{
    #region Variables
    public Action OnInitialize;
    public Action<Vector3> OnImpact;
    [Header("Componentes")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] Collider _collider;

    [Header("Componentes")]
    [SerializeField] float _damage;
    [SerializeField] float _velocidad;
    [SerializeField] float _lifeTime;
    float _lifeTimer;
    [SerializeField] LayerMask _disparables;
    #endregion




    #region Funciones Unity
    void Update()
    {
        if (!IsActive) return;

        if (_lifeTimer < Time.time)
        {
            ReturnToPool();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!IsActive) return;

        if ((_disparables & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out EnemyController enemyController))
            {
                Debug.Log("Impacto enemigo");
            }
            OnImpact?.Invoke(transform.position);
            ReturnToPool();
        }
    }
    #endregion

    #region Pool Entity
    public override void Initialize()
    {
        base.Initialize();
        _collider.enabled = true;
        _rb.isKinematic = false;
        _rb.linearVelocity = transform.forward * _velocidad;
        _lifeTimer = Time.time + _lifeTime;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        _collider.enabled = false;
        _rb.isKinematic = true;
    }
    #endregion



}
