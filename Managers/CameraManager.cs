using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    enum VirtualCameras
    {
        NoCamera = -1,
        CockPitCamera = 0,
        FollowCamera,
        ChaseCamera
    }

    [SerializeField]
    List<GameObject> _virtualCameras;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _vcam;
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    private bool isShaking;
    private float currentAmplitude;



    VirtualCameras CameraKeyPressed
    {
        get
        {
            for (int i = 0; i < _virtualCameras.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i)) return (VirtualCameras)i;
            }
            return VirtualCameras.NoCamera;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetActiveCamera(VirtualCameras.CockPitCamera);
        isShaking = false;
        shakeTimer = 0;
    }

    private void FixedUpdate()
    {
        brain = CinemachineCore.Instance.GetActiveBrain(0);
        _vcam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        
    }
    private void Update()
    {
        // cycle cameras
        SetActiveCamera(CameraKeyPressed);
        currentAmplitude = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;

        //shake cameras
        if (shakeTimer > 0)
        {
            isShaking = true;
            shakeTimer -= Time.deltaTime;
            _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                Mathf.Lerp(startingIntensity, 0f, (1 - shakeTimer / shakeTimerTotal) * Time.deltaTime);
            
        }
        else if (shakeTimer <= 0)
        { 
    
            _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            isShaking = false;
            shakeTimer = 0;
        }

       // // check if object in view, take actions on cleanup of debris
       // Vector2 screenPosition = _vcam.WorldToScreenPoint(transform.position);
       // if (screenPosition.x < widthThresold.x || screenPosition.x > widthThresold.y || screenPosition.y < heightThresold.x || screenPosition.y > heightThresold.y)
       //

    }

    void SetActiveCamera(VirtualCameras activeCamera)
    {
        if (activeCamera == VirtualCameras.NoCamera)
        {
            return;
        }

        foreach (GameObject camera in _virtualCameras)
        {
            camera.SetActive(camera.CompareTag(activeCamera.ToString()));
        }
    }

    public void ShakeActiveCamera(float intensity, float timer, string source)
    {
        // Increase shake intensity for external cams as less notable otherwise
        if ((_vcam.tag == "FollowCamera") || (_vcam.tag == "ChaseCamera"))
        {
            intensity = intensity * 3f; 
        }

        if (!isShaking || source == "explosion" || source == "blaster")
        {
            // shake cam here
            _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            shakeTimer = timer;
            shakeTimerTotal = timer;
            startingIntensity = intensity;
        }
    }

}
