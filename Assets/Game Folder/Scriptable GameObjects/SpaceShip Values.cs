using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpaceShip Values" , menuName ="ScriptableObjects/SpaceShip Values")]
public class SpaceShipValues : ScriptableObject
{
    // ============================
    // Roll Settings (Rotation around Z-axis)
    // ============================

     public float Min_Low_Roll;                 // Minimum roll speed at low throttle
     public float Min_Moderate_Roll;            // Minimum roll speed at moderate throttle
     public float Min_High_Roll;                // Minimum roll speed at high throttle
     public float Max_Low_Roll;                 // Maximum roll speed at low throttle
     public float Max_Moderate_Roll;            // Maximum roll speed at moderate throttle
     public float Max_High_Roll;                // Maximum roll speed at high throttle
     public float Min_To_Max_Duartion_Roll;     // Time to interpolate from min to max roll speed

    // ============================
    // Pitch Settings (Rotation around X-axis)
    // ============================

     public float Min_Low_Pitch;                    // Minimum pitch speed at low throttle
     public float Min_Moderate_Pitch;               // Minimum pitch speed at moderate throttle
     public float Min_High_Pitch;                   // Minimum pitch speed at high throttle
     public float Max_Low_Pitch;                    // Maximum pitch speed at low throttle
     public float Max_Moderate_Pitch;               // Maximum pitch speed at moderate throttle
     public float Max_High_Pitch;                   // Maximum pitch speed at high throttle
     public float Min_To_Max_Duartion_Pitch;        // Time to interpolate from min to max pitch speed

    // ============================
    // Yaw Settings (Rotation around Y-axis)
    // ============================

     public float Min_Low_Yaw;                          // Minimum yaw speed at low throttle
     public float Min_Moderate_Yaw;                     // Minimum yaw speed at moderate throttle
     public float Min_High_Yaw;                         // Minimum yaw speed at high throttle
     public float Max_Low_Yaw;                          // Maximum yaw speed at low throttle
     public float Max_Moderate_Yaw;                     // Maximum yaw speed at moderate throttle
     public float Max_High_Yaw;                         // Maximum yaw speed at high throttle
     public float Min_To_Max_Duartion_Yaw;              // Time to interpolate from min to max yaw speed


    // ============================
    // Throttle Settings (Forward speed control)
    // ============================

     public float Max_Low_Throttle;                        // Max speed at low throttle
     public float Max_Moderate_Throttle;                   // Max speed at moderate throttle
     public float Max_High_Throttle;                       // Max speed at high throttle
     public float Min_To_Max_Duartion_Throttle;            // Time to accelerate from min to max throttle


    // ============================
    // Physical Properties
    // ============================

     public float Mass;                                 // Ship's mass, affects inertia and physics

    // ============================
    // Drag Settings (Resistance to movement and rotation)
    // ============================


     public float Low_Linear_Drag;                      // Drag at low throttle
     public float Moderate_Linear_Drag;                 // Drag at moderate throttle
     public float High_Linear_Drag;                     // Drag at high throttle

     public float Low_Angular_Drag;                     // Rotational drag at low throttle 
     public float Moderate_Angular_Drag;                // Rotational drag at moderate Steering
     public float High_Angular_Drag;                    // Rotational drag at high Steering

    // ============================
    // Fuel System
    // ============================

     public float Max_Fuel;                             // Maximum fuel capacity
     public float Low_Throttle_Fuel_Consumption;        // Fuel consumption rate at low throttle
     public float Moderate_Throttle_Fuel_Consumption;   // Fuel consumption rate at moderate throttle
     public float High_Throttle_Fuel_Consumption;       // Fuel consumption rate at high throttle
     public float Refuel_Amount;                        // Refueling rate (units per second)

    public float Nitro;
    public float Max_Nitro;
    public float Nitro_Comsumption;
    public float Renitro_Amount;

    // ============================
    // Health
    // ============================

    public float Max_Health;                            // Maximum health of the spaceship
    public float Regenerate_Health_Amount;


    public float Payload;

}


