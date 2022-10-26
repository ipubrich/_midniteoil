using System.Collections.Generic;
using UnityEngine;

public class AnimateCockpitControls : MonoBehaviour
{

    [SerializeField]  Transform _joystick;
    public  Vector3 _joystickRange = Vector3.zero;
    public List<Transform> _throttles;
    public float _throttleRange = 35;


    // import movement to mirror controls
    IMovementControls _movementInput;

    public void Init(IMovementControls movementControls)
    {
        _movementInput = movementControls;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementInput == null) return;
        // Animate joystick using inputs
        _joystick.localRotation = Quaternion.Euler(
            x: _movementInput.PitchAmount * _joystickRange.x,
            y: _movementInput.YawAmount * _joystickRange.y,
            z:_movementInput.RollAmount * _joystickRange.z
            );




        Vector3 throttleRotation = _throttles[0].localRotation.eulerAngles;
        throttleRotation.x = _movementInput.ThrustAmount * _throttleRange;

        foreach (Transform throttle in _throttles)
        {
            throttle.localRotation = Quaternion.Euler(throttleRotation);
        }
    }

}
