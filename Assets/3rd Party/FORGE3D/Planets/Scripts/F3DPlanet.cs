using UnityEngine;
using System.Collections;

public class F3DPlanet : MonoBehaviour
{
    public float RotationRate;
    public float OrbitRate;
    public Transform OrbitPoint;

    public bool ShowOrbit;

    float distToOrbitPoint;
    Vector3 orbitAxis;
    Vector3 pointToPlanetDir;

    // Use this for initialization
    void Start ()
    {
        if (OrbitPoint)
        {
            distToOrbitPoint = Vector3.Distance(transform.position, OrbitPoint.position);
            orbitAxis = transform.up;
            pointToPlanetDir = Vector3.Normalize(transform.position - OrbitPoint.position);
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        transform.rotation *= Quaternion.Euler(0f, RotationRate * Time.deltaTime, 0f);

        if (OrbitPoint)
        {
            pointToPlanetDir = Quaternion.AngleAxis(OrbitRate * Time.deltaTime, orbitAxis) * pointToPlanetDir;
            transform.position = OrbitPoint.position + pointToPlanetDir * distToOrbitPoint;
        }
    }
}
