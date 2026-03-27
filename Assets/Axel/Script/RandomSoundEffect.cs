
using UnityEngine;

public class RandoSoundEffecs : MonoBehaviour
{
    [SerializeField] AudioClip[] _attackClips;
    [SerializeField] AudioClip[] _dieClips;
    [SerializeField] AudioClip[] _contructionClip;
    [SerializeField] AudioClip[] _reparacionClip;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] private float _tiempoEntreSonidos = 0.18f;
    private float _timerSonido;
    float minPitch = 0.85f;
    float maxPith = 1.15f;
    private void Update()
{
    if (_timerSonido > 0)
    {
        _timerSonido -= Time.deltaTime;
    }
}
    //Funciona para reproducir un clip de Ataque aleatorio 
    public void PlayRandomAttackClip()
    {
        // Seguro de que si algo falta, salte el mensaje para colocarlo
        if (_attackClips.Length == 0 || _audioSource == null)
        {
            Debug.LogWarning("No audio clips or audio source assigned.");
            return;
        }
        //Elige un sonido aleatorio dentro de la lista y lo reproduce, cambiando el Pith para quitar sensacion de monoteidad
        int randomIndex = Random.Range(0, _attackClips.Length);
        float randomPitch = Random.Range(minPitch, maxPith);
        AudioClip randomClip = _attackClips[randomIndex];
        _audioSource.pitch = randomPitch;
        _audioSource.PlayOneShot(randomClip);
    }
    //Funciona para reproducir un clip de muerte aleatorio 
    public void PlayRandomDieClip()
    {
        // Seguro de que si algo falta, salte el mensaje para colocarlo
        if (_dieClips.Length == 0 || _audioSource == null)
        {
            Debug.LogWarning("No audio clips or audio source assigned.");
            return;
        }
        //Elige un sonido aleatorio dentro de la lista y lo reproduce, cambiando el Pith para quitar sensacion de monoteidad
        int randomIndex = Random.Range(0, _dieClips.Length);
        float randomPitch = Random.Range(minPitch, maxPith);
        AudioClip randomClip = _dieClips[randomIndex];
        _audioSource.pitch = randomPitch;
        _audioSource.PlayOneShot(randomClip);
    }
    public void PlayRandomContructionClip()
    {
        // Seguro de que si algo falta, salte el mensaje para colocarlo
        if (_contructionClip.Length == 0 || _audioSource == null)
        {
            Debug.LogWarning("No audio clips or audio source assigned.");
            return;
        }
        //Elige un sonido aleatorio dentro de la lista y lo reproduce, cambiando el Pith para quitar sensacion de monoteidad
        int randomIndex = Random.Range(0, _contructionClip.Length);
        float randomPitch = Random.Range(minPitch, maxPith);
        AudioClip randomClip = _contructionClip[randomIndex];
        _audioSource.pitch = randomPitch;
        _audioSource.PlayOneShot(randomClip);
    }
    public void PlayRandomReparacionClip()
    {
        if (_timerSonido > 0) return;
        // Seguro de que si algo falta, salte el mensaje para colocarlo
        if (_reparacionClip.Length == 0 || _audioSource == null)
        {
            Debug.LogWarning("No audio clips or audio source assigned.");
            return;
        }
        //Elige un sonido aleatorio dentro de la lista y lo reproduce, cambiando el Pith para quitar sensacion de monoteidad
        int randomIndex = Random.Range(0, _reparacionClip.Length);
        AudioClip randomClip = _reparacionClip[randomIndex];
        _audioSource.PlayOneShot(randomClip);
        _timerSonido = _tiempoEntreSonidos;
    }
}









