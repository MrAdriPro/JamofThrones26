using System.Collections;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject[] _towers;
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    [SerializeField] CanvasGroup _selecciondeTorretas;
    [SerializeField] Vector3 _posicion;
    [SerializeField] float _velocidadCrecimiento;
    [SerializeField] bool _torretaActivada = false;
    int _nTorreta;
    bool _isAtive = false;
    #endregion




    #region Funciones de Unity
    void Start()
    {
        _posicion.x = transform.position.x;
        _posicion.z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAtive)
        {
            _selecciondeTorretas.alpha = 0;
            _selecciondeTorretas.blocksRaycasts = false;
            _selecciondeTorretas.interactable = false;
            return;
        }
        if (_torretaActivada)
        {
            StartCoroutine(CrecerTorre(_nTorreta));

        }

        Contacto();
    }
    void OnDrawGizmos()
    {
        //Gizmos para la caja de contacto
        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.TransformPoint(_offSet), transform.rotation, _tamanioCaja);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
    #endregion




    #region Funciones
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
        _nTorreta = 0;
        _torretaActivada = true;
    }
    public void DesplegarTorre2()
    {
        _nTorreta = 1;
        _torretaActivada = true;
    }
    public void DesplegarTorre3()
    {
        _nTorreta = 2;
        _torretaActivada = true;
    }

    #endregion




    #region Coroutine
    private IEnumerator CrecerTorre(int nTorre)
    {
        _towers[nTorre].transform.localScale = Vector3.zero;
        _towers[nTorre].SetActive(true);
        _towers[nTorre].transform.position = _posicion;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * _velocidadCrecimiento;

            _towers[nTorre].transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        _towers[nTorre].transform.localScale = Vector3.one;
        _torretaActivada = true;
        _isAtive = true;
    }
    #endregion
}






