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

    public Transform objetivo;
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
            if (other.TryGetComponent(out EnemyHealth enemyHealth))
            {
                Debug.Log("Impacto enemigo");
                enemyHealth.TakeDamage(_damage);
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
        OnInitialize?.Invoke();
        _collider.enabled = true;
        _rb.isKinematic = false;
        if (objetivo != null)
        {
            Vector3 dir = (objetivo.position - transform.position).normalized;
            if (dir.sqrMagnitude > 0.0001f) transform.forward = dir;
        }
        _rb.linearVelocity = transform.forward * _velocidad;
        _lifeTimer = Time.time + _lifeTime;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        _collider.enabled = false;
        _rb.isKinematic = true;
        objetivo = null;
        OnInitialize = null;
        OnImpact = null;
    }
    #endregion



}
