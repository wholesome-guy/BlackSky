using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Rendering;
public class UIEffects : MonoBehaviour
{
    [SerializeField] private Volume _globalVolume;

    private DepthOfField _depthOfField;
    [SerializeField] private float _focusDistance = 1.75f;
    [SerializeField] private int _focalLength = 130;
    [SerializeField] private int _aperture = 32;

    public static Action<bool> SlowMotionEffectEvent;

    private void Start()
    {
        if(_globalVolume.profile.TryGet<DepthOfField>(out _depthOfField))
        {
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
            Time.timeScale = 0.25f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            _depthOfField.active = false;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;
        }

    }
}
