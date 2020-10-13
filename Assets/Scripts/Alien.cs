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

    public Rigidbody head;
    public bool isAlive = true;

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
            if (isAlive)
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
    }
    //Make collision the trigger to destroy the alien
    void OnTriggerEnter(Collider other)
    {
        //if statement to only call die once
        if (isAlive)
        {
            Die();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        }
    }

    //Destroy the alien and cleanup properties
    public void Die()
    {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);

        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);

        head.GetComponent<SelfDestruct>().Initiate();

        Destroy(gameObject);
    }
}
