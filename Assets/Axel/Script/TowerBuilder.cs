using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] GameObject[] _towers;
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    [SerializeField] CanvasGroup _selecciondeTorretas;
    [SerializeField] Transform _posicion;
    Vector3 _scalaLocal;
    float time = 0.5f;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < _towers.Length; i++)
        {
            _scalaLocal = _towers[i].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Contacto();
            timer = time;
        }
    }
    void OnDrawGizmos()
    {
        //Gizmos para la caja de contacto
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, _tamanioCaja);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
    private void Contacto()
    {
        Vector3 centro = transform.TransformPoint(_offSet);

        Collider[] colliders = new Collider[1];

        Physics.OverlapBoxNonAlloc(centro, _tamanioCaja / 2, colliders, transform.rotation, _interacteables);
        if (colliders[0] != null)
        {
            _selecciondeTorretas.alpha = 1;
            _selecciondeTorretas.blocksRaycasts = true;
            _selecciondeTorretas.interactable = true;
        }
        else
        {
            _selecciondeTorretas.alpha = 0;
            _selecciondeTorretas.blocksRaycasts = false;
            _selecciondeTorretas.interactable = false;
        }
    }
    public void DesplegarTorre1()
    {
         _towers[0].transform.localScale = Vector3.zero;
        _towers[0].SetActive(true);
    }

}
