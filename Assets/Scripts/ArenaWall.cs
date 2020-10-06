using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWall : MonoBehaviour
{

    private Animator arenaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Gets arena object
        GameObject arena = transform.parent.gameObject;
        //References the arena's animator component
        arenaAnimator = arena.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Set IsLowered to true when trigger is activated
    void OnTriggerEnter(Collider other)
    {
        arenaAnimator.SetBool("IsLowered", true);
    }

    //When trigger not activated, islowered is false (walls raise)
    void OnTriggerExit(Collider other)
    {
        arenaAnimator.SetBool("IsLowered", false);
    }

 
}
