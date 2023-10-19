using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    [SerializeField] Transform pacman;
    [SerializeField] Transform surface;
    [SerializeField] Transform reactiveArea;

    [SerializeField] float chaseDistance;
    [SerializeField] float scatterTime;
    [SerializeField] float reactiveTime;

    [SerializeField] Material notActiveMaterial;
    Material material;

    NavMeshAgent agent;
    public Vector3 target;
    Bounds bounds;

    bool isScattering;
    float scatterTimer;

    public bool isActive = true;
    static float reactiveTimer = 0;
    

    private void Start()
    {
        isActive = true;
        agent = GetComponent<NavMeshAgent>();
        bounds = surface.GetComponent<MeshRenderer>().bounds;

        material = transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        float distanceToPacman = Vector3.Distance(transform.position, pacman.position);

        if (isActive)
        {
            if(distanceToPacman <= chaseDistance && !PacmanManager.isDie)
            {
                GoToPacman();
            }
            else if(!isScattering)
            {
                GoToRandomPoint();
            }
            else
            {
                scatterTimer -= Time.deltaTime;
                if(scatterTimer <= 0)
                {
                    GoToRandomPoint();
                }
            }

            //GetComponent<CinemachineCollisionImpulseSource>().enabled = true;
        }
        else
        {
            GoToReactiveArea();

            if(reactiveTimer <= 0)
            {
                reactiveTimer = reactiveTime;
            }
            else
            {
                reactiveTimer -= Time.deltaTime;
                isActive = reactiveTimer <= 0 ? true : false;

            }


            //GetComponent<CinemachineCollisionImpulseSource>().enabled = false;

        }
        //Go to the target
        agent.SetDestination(target);
    }

    void GoToPacman()
    {
        target = pacman.position;
        isScattering = false;
        ChangeMaterial(material);
    }
    void GoToRandomPoint()
    {
        isScattering = true;
        scatterTimer = scatterTime;
        target = GetRandomTargetPoint();
        ChangeMaterial(material);

    }
    void GoToReactiveArea()
    {
        target = reactiveArea.position;
        ChangeMaterial(notActiveMaterial);
    }

    void ChangeMaterial(Material mat)
    {
        transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = mat;
    }

    //Generate a random point
    public Vector3 GetRandomTargetPoint()
    {
        return new Vector3(Random.Range(bounds.min.x, bounds.max.x), transform.position.y, Random.Range(bounds.min.z, bounds.max.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RactvArea")
        {
            isActive = true;
        }
    }

    // Draw the chase bounds
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        
    }
}
