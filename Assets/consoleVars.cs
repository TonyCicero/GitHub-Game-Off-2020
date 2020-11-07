using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class consoleVars : MonoBehaviour
{
    public float Throttle; //ship throttle value 0-100
    public float Health =100; //ship health value
    public float gForce =9.8f; //gravitational force on ship
    public float Fuel =100; //ship fuel level
    public float Battery =100; //ship battery level
    public bool FuelPump; //Fuel Pump status
    public bool Engine; //Engine System
    public bool Navigation; //Navigation system status
    public bool Solar; //Solar system status
    public bool Communication; //Communication system status
}
