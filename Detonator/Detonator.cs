using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Places a Gameobject and populates it with an Audiosource, fetches the length of the clip and Destroys or pools it when it's done.
/// I use it for most one-shot sounds so they don't squash one another
/// worth setting it up for Object Pooling too. Creating and destroying entities can be pretty inefficient.
/// </summary>
public class Detonator : MonoBehaviour
{
    // Detonator manager child gameobjects
    [Header("Detonator Child Object")]
    public GameObject _detonatorPrefab;
    private GameObject _detonationEffect;

    [Header("Explosion")]
    [SerializeField] private List<GameObject> _explosionPrefab = new();
    [SerializeField][Range(0f, 150f)] private float _explosionRadius;
    private int randomExplosion = 0; // pick random item from list
    private float randomForce = 0;

    [Header("Impact")]
    [SerializeField] private List<GameObject> _shieldImpactPrefab = new();
    [SerializeField] private List<GameObject> _rockImpactPrefab = new();
    private int randomShieldImpact = 0;
    private int randomRockImpact = 0;
    private ParticleSystem _impactParticleEffect;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> _explosionAudio = new();
    [SerializeField] private List<AudioClip> _impactAudio = new();
    [SerializeField] private AudioSource _audioSource = new();

    private AudioClip _detonationAudio;
    private int randomExplosionAudio = 0; // pick random item from list
    private int randomImpactAudio = 0; // pick random item from list

    [Header("Camera")]
    public CameraManager _cameraManager;
    private bool _shake = false;

    private void Awake()
    {
        
    }
    private void RandomList()
    {
        // randomise lists
        randomExplosionAudio = Random.Range(0, _explosionAudio.Count);
        randomImpactAudio = Random.Range(0, _impactAudio.Count);
        randomShieldImpact = Random.Range(0, _shieldImpactPrefab.Count);
        randomRockImpact = Random.Range(0, _rockImpactPrefab.Count);
        randomExplosion = Random.Range(0, _explosionPrefab.Count);

        _impactParticleEffect = _rockImpactPrefab[randomRockImpact].transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
    }

    public void Boom(Vector3 hitPosition, Quaternion rotation, float fuse, string source, Transform transformObject)
    {
        RandomList();
        Debug.Log("explosion type " + randomExplosion);
        // create and name new object 
        GameObject _childDetonator = Instantiate(_detonatorPrefab, hitPosition, rotation, transformObject);
        _childDetonator.name = ("Detonator prefab " + source);
        _childDetonator.transform.parent = transform;

        // set up child components
        _childDetonator.AddComponent<AudioSource>();

        // set up audio component
    
        _audioSource = _childDetonator.GetComponent<AudioSource>();

        //Load AudioMixer
        AudioMixer audioMixer = Resources.Load<AudioMixer>("MainAudio");

        //Find AudioMixerGroup you want to load
        AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups("EffectsGeneral");

        //Assign the AudioMixerGroup to AudioSource (Use first index)
        _audioSource.outputAudioMixerGroup = audioMixGroup[0];

        // set up effect variances based on source
        GameObject impactGameOject;
        switch (source)
        {
            case "Shield":
                _detonationEffect = _shieldImpactPrefab[randomShieldImpact];
                _detonationAudio = _impactAudio[randomImpactAudio];
                _audioSource.maxDistance = 150;
                _shake = false;
                randomForce = 0;
                break;

            case "Enemy":
                _detonationEffect = _explosionPrefab[randomExplosion];
                _detonationAudio = _impactAudio[randomImpactAudio];
                _audioSource.maxDistance = 150;
                _shake = false;
                randomForce = 0;
                break;

            case "Asteroid":
                _detonationEffect = _explosionPrefab[randomExplosion];
                _detonationAudio = _explosionAudio[randomExplosionAudio];
                _audioSource.maxDistance = 1000;
                _shake = true;
                randomForce = Random.Range(20, _explosionRadius);
                impactGameOject = Instantiate(_rockImpactPrefab[randomRockImpact], hitPosition, rotation);
                // rock impact particles
                var impactSize = _impactParticleEffect.main;
                impactSize.startSize = new ParticleSystem.MinMaxCurve(0, transformObject.localScale.x * 5);
                Destroy(impactGameOject, fuse * 2);
                break;

            case null: // explosion defaults
                _detonationEffect = _explosionPrefab[randomExplosion];
                _detonationAudio = _explosionAudio[randomExplosionAudio];
                _audioSource.maxDistance = 1000;
                _shake = true;
                randomForce = Random.Range(1, _explosionRadius);
                break;
        }

        if (hitPosition != null)
        {
            AudioClip clip = _detonationAudio;
            _audioSource.clip = clip;
            _audioSource.spatialBlend = 1;
            _audioSource.rolloffMode = AudioRolloffMode.Custom;
            
            // play audio
            _audioSource.PlayOneShot(clip);
            
            // explosive force effects
            Collider[] colliders = Physics.OverlapSphere(hitPosition, _explosionRadius);
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    
                    // add physical force to nearby objects
                    rb.AddExplosionForce(randomForce, hitPosition, randomForce);

                    // add damage to sphere collider here - player / enemy ship damage

                    if (nearbyObject.gameObject.CompareTag("Player") && _shake)
                    {
                        // SHAKE CAMERA 
                        ImpactCamera();
                   
                    }
                }
            }
        }
        else return;

        // explosion 
        GameObject explosion = Instantiate(_detonationEffect, hitPosition, rotation);
               
        // randomise explosion size
        if (source == "Asteroid") 
        {
            Vector3 _explosionSize;
            _explosionSize.x = Random.Range(transformObject.localScale.x / 2f, 30f);
            _explosionSize.y = Random.Range(transformObject.localScale.y / 2f, 30f);
            _explosionSize.z = Random.Range(transformObject.localScale.z / 2f, 30f);
            explosion.transform.localScale = new Vector3(_explosionSize.x, _explosionSize.y, _explosionSize.z);
        }

        // Cleanup
        
        Destroy(_childDetonator, _audioSource.clip.length); // get audio clip legnth and remove
        Destroy(explosion, fuse); // destroy explosion effect

    }

    void ImpactCamera()
    {
        _cameraManager.ShakeActiveCamera(1f, 2f, "explosion");
    }

}


// account for varying sizes of asteroid - done mostly.
// split audio to own class
// add noises for camera shake
// add rumble for controller
// split camera shake to own class - done


// split detonator into manager that handles input from different events such as blaster impact, missiles or explosion.  done