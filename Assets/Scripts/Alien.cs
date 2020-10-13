using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class Alien : MonoBehaviour
{
    // Alien target location
    public Transform target;
    private NavMeshAgent agent;

    //Time in ms when alien will update path
    public float navigationUpdate;
    //Tracks time since previous update
    private float navigationTime = 0;
    //Unity custom event type
    public UnityEvent OnDestroy;

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
        Die();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
    }

    //Destroy the alien when it dies
    public void Die()
    {
        Destroy(gameObject);
        //Notifies GM of death
        OnDestroy.Invoke();
        //Removes the listener
        OnDestroy.RemoveAllListeners();
    }
}
