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
    }
    void GoToRandomPoint()
    {
        isScattering = true;
        scatterTimer = scatterTime;
        target = GetRandomTargetPoint();
        
    }
    void GoToReactiveArea()
    {
        target = reactiveArea.position;
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

}
