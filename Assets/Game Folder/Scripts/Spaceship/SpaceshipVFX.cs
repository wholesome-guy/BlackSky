using UnityEngine;
using UnityEngine.VFX;

public class SpaceshipVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect Particles_VFX;
    [SerializeField] private ParticleSystem Wind_Zone_VFX;
    private ParticleSystem.MainModule Wind_Zone_Main;
    private ParticleSystem.EmissionModule Wind_Zone_Emission;

    [SerializeField] private Material Thruster_Material;
    [SerializeField] private Material Flame_Trail;

    private float Thrust_Value;
    [SerializeField] private float Low_Throttle_Velocity = 59f;
    [SerializeField] private float Moderate_Throttle_Velocity = 127f;
    [SerializeField] private float High_Throttle_Velocity = 245f;

    private float Expected_Thrust_Value;
    [SerializeField] private float Expected_Thrust_Value_Low = 0.75f;
    [SerializeField] private float Expected_Thrust_Value_Moderate = 0.85f;
    [SerializeField] private float Expected_Thrust_Value_High = 0.9f;

    private float Expected_Particle_Speed_Value;
    [SerializeField] private float Particle_Speed_Value_Low = 200f;
    [SerializeField] private float Particle_Speed_Value_Moderate = 400f;
    [SerializeField] private float Particle_Speed_Value_High = 600;

    private float Velocity_To_Particle_Speed_Constant;
    private float Low_Velocity_To_Particle_Speed_Constant;
    private float Moderate_Velocity_To_Particle_Speed_Constant;
    private float High_Velocity_To_Particle_Speed_Constant;

    private float Expected_Particle_Rate_Value;
    [SerializeField] private float Particle_Rate_Value_Low = 10f;
    [SerializeField] private float Particle_Rate_Value_Moderate = 50f;
    [SerializeField] private float Particle_Rate_Value_High = 100;

    private float Velocity_To_Particle_Rate_Constant;
    private float Low_Velocity_To_Particle_Rate_Constant;
    private float Moderate_Velocity_To_Particle_Rate_Constant;
    private float High_Velocity_To_Particle_Rate_Constant;


    private float Velocity_To_Thurst_Constant;
    private float Low_Velocity_To_Thurst_Constant;
    private float Moderate_Velocity_To_Thurst_Constant;
    private float High_Velocity_To_Thurst_Constant;

    [ColorUsage(true, true)]
    [SerializeField] private Color[] Color1 = new Color[3];
    [ColorUsage(true, true)]
    [SerializeField] private Color[] Color2 = new Color[3];
    [ColorUsage(true, true)]
    [SerializeField] private Color[] Color3 = new Color[3];

    private float Particle_Rate;

    public bool Particle_Switch = false;

    private float Player_Velocity;


    private void Start()
    {

        Low_Velocity_To_Thurst_Constant = Expected_Thrust_Value_Low / Low_Throttle_Velocity;
        Moderate_Velocity_To_Thurst_Constant = Expected_Thrust_Value_Moderate / Moderate_Throttle_Velocity;
        High_Velocity_To_Thurst_Constant = Expected_Thrust_Value_High / High_Throttle_Velocity;

        Low_Velocity_To_Particle_Speed_Constant = Particle_Speed_Value_Low / Low_Throttle_Velocity;
        Moderate_Velocity_To_Particle_Speed_Constant = Particle_Speed_Value_Moderate / Moderate_Throttle_Velocity;
        High_Velocity_To_Particle_Speed_Constant = Particle_Speed_Value_High / High_Throttle_Velocity;

        Low_Velocity_To_Particle_Rate_Constant = Particle_Rate_Value_Low / Low_Throttle_Velocity;
        Moderate_Velocity_To_Particle_Rate_Constant = Particle_Rate_Value_Moderate / Moderate_Throttle_Velocity;
        High_Velocity_To_Particle_Rate_Constant = Particle_Rate_Value_High / High_Throttle_Velocity;


        Wind_Zone_Main = Wind_Zone_VFX.main;
        Wind_Zone_Emission = Wind_Zone_VFX.emission;

        Low_Throttle();
    }

    private void Update()
    {
        Thrust_Value = Mathf.Clamp(Player_Velocity * Velocity_To_Thurst_Constant, 0, Expected_Thrust_Value);

        Thruster_Material.SetFloat("_ThrustPower", Thrust_Value);

        float Particle_Speed = Mathf.Clamp(Player_Velocity * Velocity_To_Particle_Speed_Constant, 0, Expected_Particle_Speed_Value);
        Wind_Zone_Main.startSpeed = Particle_Speed;

        float Particle_Rate = Mathf.Clamp(Player_Velocity * Velocity_To_Particle_Rate_Constant, 0, Expected_Particle_Rate_Value);
        Wind_Zone_Emission.rateOverTime = Particle_Rate;


        bool isMoving = Player_Velocity > 0.01f;

        if (isMoving && !Particle_Switch)
        {
            Particles_Play();
            Particle_Switch = true;
        }
        else if (!isMoving && Particle_Switch)
        {
            Particles_Stop();
            Particle_Switch = false;
        }


    }

    private void Particles_Play()
    {
        Particles_VFX.Play();
        Wind_Zone_VFX.Play();
    }
    private void Particles_Stop()
    {
        Particles_VFX.Stop();
        Wind_Zone_VFX.Stop();

    }
    public void Low_Throttle()
    {


        Velocity_To_Thurst_Constant = Low_Velocity_To_Thurst_Constant;
        Expected_Thrust_Value = Expected_Thrust_Value_Low;
        Particle_Rate = 5;
        Velocity_To_Particle_Speed_Constant = Low_Velocity_To_Particle_Speed_Constant;
        Velocity_To_Particle_Rate_Constant = Low_Velocity_To_Particle_Rate_Constant;
        Expected_Particle_Speed_Value = Particle_Speed_Value_Low;
        Expected_Particle_Rate_Value = Particle_Rate_Value_Low;


        Thruster_Material.SetColor("_Colour_1", Color1[0]);
        Thruster_Material.SetColor("_Colour_2", Color2[0]);

        Flame_Trail.SetColor("_Colour_1", Color1[0]);
        Flame_Trail.SetColor("_Colour_2", Color2[0]);


        Particles_VFX.SetFloat("Rate", Particle_Rate);
        Particles_VFX.SetVector4("Colour", Color3[0]);

    }
    public void Moderate_Throttle()
    {
        Velocity_To_Thurst_Constant = Moderate_Velocity_To_Thurst_Constant;
        Expected_Thrust_Value = Expected_Thrust_Value_Moderate;
        Particle_Rate = 10;
        Velocity_To_Particle_Speed_Constant = Moderate_Velocity_To_Particle_Speed_Constant;
        Velocity_To_Particle_Rate_Constant = Moderate_Velocity_To_Particle_Rate_Constant;
        Expected_Particle_Speed_Value = Particle_Speed_Value_Moderate;
        Expected_Particle_Rate_Value = Particle_Rate_Value_Moderate;



        Thruster_Material.SetColor("_Colour_1", Color1[1]);
        Thruster_Material.SetColor("_Colour_2", Color2[1]);

        Flame_Trail.SetColor("_Colour_1", Color1[1]);
        Flame_Trail.SetColor("_Colour_2", Color2[1]);

        Particles_VFX.SetFloat("Rate", Particle_Rate);
        Particles_VFX.SetVector4("Colour", Color3[1]);


    }
    public void High_Throttle()
    {
        Velocity_To_Thurst_Constant = High_Velocity_To_Thurst_Constant;
        Expected_Thrust_Value = Expected_Thrust_Value_High;
        Particle_Rate = 15;
        Velocity_To_Particle_Speed_Constant = High_Velocity_To_Particle_Speed_Constant;
        Velocity_To_Particle_Rate_Constant = High_Velocity_To_Particle_Rate_Constant;
        Expected_Particle_Speed_Value = Particle_Speed_Value_High;
        Expected_Particle_Rate_Value = Particle_Rate_Value_High;



        Thruster_Material.SetColor("_Colour_1", Color1[2]);
        Thruster_Material.SetColor("_Colour_2", Color2[2]);


        Flame_Trail.SetColor("_Colour_1", Color1[2]);
        Flame_Trail.SetColor("_Colour_2", Color2[2]);

        Particles_VFX.SetFloat("Rate", Particle_Rate);
        Particles_VFX.SetVector4("Colour", Color3[2]);


    }
}
