using UnityEngine;
using UnityEngine.VFX;

public class SpaceshipVFX : MonoBehaviour
{
    [Header("Thruster")]
    [SerializeField] private Material _thrusterMaterial;
    [SerializeField] private Material _thrusterTrailMaterial;
    [SerializeField] private float _lowThrottleSpeed = 8f;
    [SerializeField] private float _moderateThrottleSpeed = 30f;
    [SerializeField] private float _highThrottleSpeed = 80f;
    [SerializeField] private float _lowThrustValue = 0.75f;
    [SerializeField] private float _moderateThrustValue = 0.85f;
    [SerializeField] private float _highThrustValue = 0.9f;

    [Header("Wind Zone")]
    [SerializeField] private ParticleSystem _windZone;
    [SerializeField] private float _lowParticleSpeed = 200f;
    [SerializeField] private float _moderateParticleSpeed = 400f;
    [SerializeField] private float _highParticleSpeed = 600f;
    [SerializeField] private float _lowParticleRate = 10f;
    [SerializeField] private float _moderateParticleRate = 50f;
    [SerializeField] private float _highParticleRate = 100f;

    [SerializeField] private VisualEffect _particles1VFX;
    [SerializeField] private VisualEffect _particles2VFX;
    [SerializeField] private TrailRenderer[] _trailRenderers;

    [ColorUsage(true, true)][SerializeField] private Color[] _color1 = new Color[3];
    [ColorUsage(true, true)][SerializeField] private Color[] _color2 = new Color[3];
    [ColorUsage(true, true)][SerializeField] private Color[] _color3 = new Color[3];

    private struct ThrottleSettings
    {
        public float maxSpeed;
        public float thrustValue;
        public float particleSpeed;
        public float particleRate;
        public Color color1, color2, color3;
        public float vfxRate;
    }

    private ThrottleSettings[] _throttleSettings;

    private float _thrustValue;
    private float _thrustExpectedValue;
    private float _speedToThrustConstant;
    private float _expectedParticleSpeed;
    private float _expectedParticleRate;
    private float _speedToParticleSpeedConstant;
    private float _speedToParticleRateConstant;

    private ParticleSystem.MainModule _windZoneMain;
    private ParticleSystem.EmissionModule _windZoneEmission;


    private float _spaceshipSpeed;
    private bool _particleBool;

    [SerializeField] private float _updateInterval = 0.5f;
    private float _updateTimer;

    private static readonly int _thrustPowerID = Shader.PropertyToID("_thrustPower");
    private static readonly int _thrustColour1ID = Shader.PropertyToID("_colour1");
    private static readonly int _thrustColour2ID = Shader.PropertyToID("_colour2");
    private static readonly int _thrustTrailColour1ID = Shader.PropertyToID("_Colour_1");
    private static readonly int _thrustTrailColour2ID = Shader.PropertyToID("_Colour_2");
    private static readonly int _thursterParticleRateID = Shader.PropertyToID("Rate");
    private static readonly int _thursterParticleColourID = Shader.PropertyToID("Colour");



    private void OnEnable()
    {
        SpaceshipMovement.SpeedAccess += SpaceshipSpeed;
        SpaceshipMovement.ThrottleChange += SelectThrottle;
    }

    private void OnDisable()
    {
        SpaceshipMovement.SpeedAccess -= SpaceshipSpeed;
        SpaceshipMovement.ThrottleChange -= SelectThrottle;
    }

    private void Start()
    {
        _throttleSettings = new ThrottleSettings[]
        {
            new() { maxSpeed = _lowThrottleSpeed,      thrustValue = _lowThrustValue,
                    particleSpeed = _lowParticleSpeed,  particleRate = _lowParticleRate,
                    color1 = _color1[0], color2 = _color2[0], color3 = _color3[0], vfxRate = 5f },

            new() { maxSpeed = _moderateThrottleSpeed,  thrustValue = _moderateThrustValue,
                    particleSpeed = _moderateParticleSpeed, particleRate = _moderateParticleRate,
                    color1 = _color1[1], color2 = _color2[1], color3 = _color3[1], vfxRate = 10f },

            new() { maxSpeed = _highThrottleSpeed,      thrustValue = _highThrustValue,
                    particleSpeed = _highParticleSpeed,  particleRate = _highParticleRate,
                    color1 = _color1[2], color2 = _color2[2], color3 = _color3[2], vfxRate = 15f },
        };

        _windZoneMain = _windZone.main;
        _windZoneEmission = _windZone.emission;
        _updateTimer = 0;

        ApplyThrottle(0);
    }

    private void Update()
    {
        _updateTimer += Time.deltaTime;
        if( _updateTimer > _updateInterval)
        {
            TimedUpdate();
            _updateTimer = 0;
        }
    }
    private void TimedUpdate()
    {

        _thrustValue = Mathf.Clamp(_spaceshipSpeed * _speedToThrustConstant, 0f, _thrustExpectedValue);
        _thrusterMaterial.SetFloat(_thrustPowerID, _thrustValue);

        _windZoneMain.startSpeed = Mathf.Clamp(_spaceshipSpeed * _speedToParticleSpeedConstant, 0f, _expectedParticleSpeed);
        _windZoneEmission.rateOverTime = Mathf.Clamp(_spaceshipSpeed * _speedToParticleRateConstant, 0f, _expectedParticleRate);

        bool isMoving = _spaceshipSpeed > 0.01f;
        if (isMoving && !_particleBool)
        {
            ParticlesPlay();
            _particleBool = true;
        }
        else if (!isMoving && _particleBool)
        {
            ParticlesStop();
            _particleBool = false;
        }
    }


    private void SpaceshipSpeed(float speed)
    { 
        _spaceshipSpeed = speed; 
    } 

    private void ParticlesPlay()
    {
        _particles1VFX.Play();
        _particles2VFX.Play();
        _windZone.Play();
    }

    private void ParticlesStop()
    {
        _particles1VFX.Stop();
        _particles2VFX.Stop();
        _windZone.Stop();
    }
    private void TrailRendererSwitch(bool boolean)
    {
        int count = _trailRenderers.Length;
        for (int i = 0; i < count; i++)
        {
            _trailRenderers[i].enabled = boolean;
        }
    }

    private void SelectThrottle(int i)
    {
        if (i == 0) 
        {
            _speedToThrustConstant       = 0f;
            _speedToParticleSpeedConstant = 0f;
            _speedToParticleRateConstant  = 0f;
            ParticlesStop();
            TrailRendererSwitch(false);
            return;
        }
        TrailRendererSwitch(true);
        ApplyThrottle(Mathf.Clamp(i - 1, 0, _throttleSettings.Length - 1));
    }

    private void ApplyThrottle(int index)
    {
        var s = _throttleSettings[index];

        _speedToThrustConstant = s.thrustValue / s.maxSpeed;
        _thrustExpectedValue = s.thrustValue;
        _speedToParticleSpeedConstant = s.particleSpeed / s.maxSpeed;
        _speedToParticleRateConstant = s.particleRate / s.maxSpeed;
        _expectedParticleSpeed = s.particleSpeed;
        _expectedParticleRate = s.particleRate;

        _thrusterMaterial.SetColor(_thrustColour1ID, s.color1);
        _thrusterMaterial.SetColor(_thrustColour2ID, s.color2);
        _thrusterTrailMaterial.SetColor(_thrustTrailColour1ID, s.color1);
        _thrusterTrailMaterial.SetColor(_thrustTrailColour2ID, s.color2);

        _particles1VFX.SetFloat(_thursterParticleRateID, s.vfxRate);
        _particles1VFX.SetVector4(_thursterParticleColourID, s.color3);
        _particles2VFX.SetFloat(_thursterParticleRateID, s.vfxRate);
        _particles2VFX.SetVector4(_thursterParticleColourID, s.color3);
    }
}