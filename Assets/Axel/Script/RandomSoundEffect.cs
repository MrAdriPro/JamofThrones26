
using UnityEngine;

public class RandoSoundEffecs : MonoBehaviour
{
    [SerializeField] AudioClip[] _attackClips;
    [SerializeField] AudioClip[] _dieClips;
    [SerializeField] AudioClip[] _contructionClip;
    [SerializeField] AudioSource _audioSource;
    float minPitch = 0.85f;
    float maxPith = 1.15f;
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
}









