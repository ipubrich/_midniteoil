using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))]

public class SkyBoxSetter : MonoBehaviour
{
    [SerializeField] List<Material> _skyboxMaterials;
    Skybox _skybox;

    private void Awake()
    {
        _skybox = GetComponent<Skybox>();
    }
    private void OnEnable()
    {
        ChangeSkybox(0);
    }

    private void ChangeSkybox(int skyBox)
    {
        if (_skybox != null && skyBox >= 0 && skyBox <= _skyboxMaterials.Count)
        {
            _skybox.material = _skyboxMaterials[skyBox];
        }
    }
}
