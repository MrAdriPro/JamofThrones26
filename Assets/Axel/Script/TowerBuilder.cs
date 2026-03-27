using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject[] _towers;
    [SerializeField] ShopManager _shopManager;
    [SerializeField] RandoSoundEffecs _randomSoundEffect;
    [SerializeField] LayerMask _interacteables;
    [SerializeField] Vector3 _tamanioCaja;
    [SerializeField] Vector3 _offSet;
    [SerializeField] CanvasGroup _selecciondeTorretas;
    [SerializeField] Vector3 _posicion;
    [SerializeField] float _velocidadCrecimiento;
    [SerializeField] bool _torretaActivada = false;
    [SerializeField] Vector3 _localScale;
    [SerializeField] int _dineroTorreta1;
    [SerializeField] Button _buttonTorre1;
    [SerializeField] int _dineroTorreta2;
    [SerializeField] Button _buttonTorre2;
    int _nTorreta;
    bool _isActive = false;
    #endregion




    #region Funciones de Unity
    void Start()
    {
        _posicion.x = transform.position.x;
        _posicion.z = transform.position.z;
        _localScale = _towers[0].transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
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
            Torreta1();
            Torreta2();
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
        _randomSoundEffect.PlayRandomContructionClip();
    }
    public void DesplegarTorre2()
    {
        _nTorreta = 1;
        _torretaActivada = true;
        _randomSoundEffect.PlayRandomContructionClip();
    }
    public void DesplegarTorre3()
    {
        _nTorreta = 2;
        _torretaActivada = true;
        _randomSoundEffect.PlayRandomContructionClip();
    }
    #endregion




    #region Funciones Funcionales
    private void Torreta1()
    {
        if (_shopManager.actualCoins < _dineroTorreta1)
        {
            _buttonTorre1.interactable = false;
            return;
        }
        else
        {
            _buttonTorre1.interactable = true;
        }
    }
    private void Torreta2()
    {
        if (_shopManager.actualCoins < _dineroTorreta2)
        {
            _buttonTorre2.interactable = false;
            return;
        }
        else
        {
            _buttonTorre2.interactable = true;
        }
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

            _towers[nTorre].transform.localScale = Vector3.Lerp(Vector3.zero, _localScale, t);
            yield return null;
        }
        _towers[nTorre].transform.localScale = _localScale;
        _torretaActivada = true;
        _isActive = true;
    }
    #endregion
}






