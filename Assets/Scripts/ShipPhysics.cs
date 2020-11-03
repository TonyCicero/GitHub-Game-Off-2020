using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPhysics : MonoBehaviour
{
    public List<MassBody> masses;
    List<Vector3> Forces;

    const float G = 667f;

    [Header("Ship Characteristics")]
    float mass;
    public float thrust = 100; //thrust at 100% throttle
    public float Yaw = 10; // yaw speed modifier
    public float Pitch = 10; //pitch speed modifier
    public float Roll = 10; //roll speed modifier

    [Header("Ship Controls / Stats")]
    [Range(0, 100)]
    public float throttle = 0f;
    [SerializeField]
    float speed;

    [Header("Particle Effects")]
    [SerializeField]
    ParticleSystem[] booster_fx;
    float boosterLife = 5; //max liftime

    Rigidbody rb;

    Vector3 GravDir; //direction of the gravitational force
    Vector3 GravForce; //gravitational force to be applied to Ship

    private void Awake()
    {
        masses = new List<MassBody>(); // initiate list before anything can be added to it
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
    }

    List<Vector3> calcForces() // calculate all gravitational forces on the ship
    {
        List<Vector3> forces = new List<Vector3>();
        for (int i = 0; i < masses.Count; i++)
        {
            float dist = Vector3.Distance(masses[i].transform.position, transform.position);
            float M = masses[i].mass;
            float forceG = (G * M * mass) / (dist * dist);
            //Debug.Log(forceG);
            Vector3 dir = (masses[i].transform.position - transform.position).normalized;
            //Debug.Log(dir*forceG);
            forces.Add(dir * forceG);
        }
        //Debug.Log(forces[0]);
        return forces;
    }

    Vector3 addForces(List<Vector3> fs)
    {
        Vector3 ff = new Vector3(0,0,0); //final force
        for (int i = 0; i < fs.Count; i++)
        {
            ff += fs[i];
        }
        return ff;
    }

    void chgBoosterSpeed()
    {
        for(int i = 0; i < booster_fx.Length; i++)
        {
            var main = booster_fx[i].main;
            main.startLifetime = boosterLife * (throttle / 100);
        }
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetAxis("Throttle") > 0 && throttle < 100)
        {
            throttle++;
            chgBoosterSpeed();
        }else if (Input.GetAxis("Throttle") < 0 && throttle > 0)
        {
            throttle--;
            chgBoosterSpeed();
        }
            transform.Rotate(new Vector3(Input.GetAxis("Vertical") * Pitch * Time.deltaTime, Input.GetAxis("Horizontal") * Yaw * Time.deltaTime, Input.GetAxis("Roll") * Roll * Time.deltaTime), Space.Self); //yaw & pitch controls
    }

    void FixedUpdate()
    {
        GravForce = addForces(calcForces());
        GravDir = GravForce.normalized;
        rb.AddForce(GravForce); //gravitational forces
        rb.AddRelativeForce(Vector3.forward * (throttle * thrust)); // forward thrust
    }
}
