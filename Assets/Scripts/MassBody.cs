using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassBody : MonoBehaviour
{
    public float mass;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ShipPhysics>().masses.Add(this); //add this to the ShipPhysics list 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
