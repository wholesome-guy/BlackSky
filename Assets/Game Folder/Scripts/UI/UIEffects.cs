using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Rendering;
public class UIEffects : MonoBehaviour
{
    [SerializeField] private Volume _globalVolume;

    private DepthOfField _depthOfField;
    [SerializeField] private float _focusDistance = 1.75f;
    [SerializeField] private float _focalLength = 130;
    [SerializeField] private float _aperture = 32;

    public static Action<bool> SlowMotionEffectEvent;

    private float _slowedTime = 0.25f;
    private float _normalTime = 1.0f;
    private float _fixedTimeStep = 0.02f;

    private void Start()
    {
        if(_globalVolume.profile.TryGet<DepthOfField>(out _depthOfField))
        {
            _depthOfField.active = true;
            _depthOfField.active = false;
            _depthOfField.focusDistance.value = _focusDistance;
            _depthOfField.focalLength.value = _focalLength;
            _depthOfField.aperture.value = _aperture;
        }
    }

    private void OnEnable()
    {
        SlowMotionEffectEvent += SlowMotion;
    }
    private void OnDisable()
    {
        SlowMotionEffectEvent -= SlowMotion;
    }

    private void SlowMotion(bool boolean)
    {
        if(boolean)
        {
            _depthOfField.active = true;
            Time.timeScale = _slowedTime;
        }
        else
        {
            _depthOfField.active = false;
            Time.timeScale = _normalTime;
        }
        Time.fixedDeltaTime = _fixedTimeStep * Time.timeScale;
    }
}
