using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] [Range(100f, 1000f)] private int _asteroidCount = 500;
    [SerializeField] [Range(100f, 1000f)] private int _asteroidRadius = 300;
    [SerializeField] [Range(1f, 50f)] private float _minScale = 1;
    [SerializeField] [Range(50f, 100f)] private float _maxScale = 20;
    [SerializeField] private List<GameObject> _asteroidPrefabs;

    private Transform _transform;
    
    private void Awake()
    {

        _transform = GameObject.Find("Escort Fighter").GetComponent<Transform>();
    }
    private void OnEnable()
    {
        SpawnAsteroids();
    }
    void SpawnAsteroids()
    {
        for (int i = 0; i < _asteroidCount; i++)
        {
            GameObject asteroid = Instantiate(_asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Count)], _transform.position, Quaternion.identity);
            
            float scale = Random.Range(_minScale, _maxScale);
            asteroid.transform.localScale = new Vector3(scale, scale, scale);
            asteroid.transform.position += Random.insideUnitSphere * _asteroidRadius;
            asteroid.GetComponent<Rigidbody>().AddTorque(Random.insideUnitCircle * Random.Range(0.1f, 50f));
        }
    }


}
