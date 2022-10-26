using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.VFX;


public class Shield : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 500;
    [SerializeField] private int _health;

    public SpawnShieldRipples m_shieldVFX;


    private void Awake()
    {
        //_renderer = GetComponent<Renderer>();
        _health = _maxHealth;
        //_baseColor = _renderer.material.color;
    }
    private void Start()
    {
       // GetComponent<MeshCollider>().enabled = true;
       // GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<VisualEffect>().enabled = true;

    }

    public void TakeDamage(int damage, Vector3 hitPosition)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DestroyShields();
            return;
        }
    }

    private void DestroyShields()
    {
        StopAllCoroutines();
        // disable instead of destroy
        GetComponent<VisualEffect>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        m_shieldVFX.ShieldsDown();

    }
}

