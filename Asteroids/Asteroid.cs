using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    [SerializeField] private FracturedAsteroid _fracturedAsteroidPrefab;
    //[SerializeField] private Detonator _explosionPrefab;
    [SerializeField][Range(0, 5)] float tumble = 1f;
    private Transform _transform;
    private Detonator _hiteffect;


    private void Awake()
    {
        _transform = transform;
        _hiteffect = GameObject.Find("DetonatorManager").GetComponent<Detonator>();
    }
    private void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(0, tumble);
    }
    public void TakeDamage(int damage, Vector3 hitPosition)
    {
        // scale damage depending on size of asteroid?
        FracturedAsteroid(hitPosition);
    }

    private void FracturedAsteroid(Vector3 hitPosition)
    {
       if (_fracturedAsteroidPrefab != null)
       {

            // match parent asteroid transform for fracture
            FracturedAsteroid _  =  Instantiate(_fracturedAsteroidPrefab, _transform.position, _transform.rotation);
            _.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

            _hiteffect.Boom(hitPosition, Quaternion.identity, 5f, "Asteroid", transform);
             Destroy(gameObject);
            // Call detonator class
        }
      //if (_explosionPrefab != null)
      //{
      //     //Instantiate(_explosionPrefab, hitPosition, Quaternion.identity);
      //     
      //}
      //
      //Destroy(gameObject);
    }
}


