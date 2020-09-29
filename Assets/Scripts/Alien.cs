using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    // Alien target location
    public Transform target;
    private NavMeshAgent agent;

    //Time in ms when alien will update path
    public float navigationUpdate;
    //Tracks time since previous update
    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //Checks to see if specific time has passed then updates path
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                agent.destination = target.position;
                navigationTime = 0;
            }
        }
    }
    //Make collision the trigger to destroy the alien
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
