using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //The player
    public GameObject player;
    //An array of spawn points
    public GameObject[] spawnPoints;
    //The alien
    public GameObject alien;

    //Max value of aliens allowed to display
    public int maxAliensOnScreen;
    //Number of aliens player must kill
    public int totalAliens;
    //Min and max spawn rate values
    public float minSpawnTime;
    public float maxSpawnTime;
    //The amount of aliens which spawn per spawn event
    public int aliensPerSpawn;

    //Current aliens
    private int aliensOnScreen = 0;
    //Time between spawn events
    private float generatedSpawnTime = 0;
    //Tracks time since last spawn
    private float currentSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Accumulates the amount of time that has passed
        currentSpawnTime += Time.deltaTime;


        if (currentSpawnTime > generatedSpawnTime)
        {
            //Reset timer after spawn
            currentSpawnTime = 0;
            //Randomize spawn time between min and max ranges
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            //If aliens per spawn is greater than 0 and current aliens are less than aliens needed
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                //Array that documents spawn locations
                List<int> previousSpawnLocations = new List<int>();

                //Limit number of spawnable aliens by number of spawn points
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                //Limit aliens spawned to total aliens needed
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                    aliensPerSpawn - totalAliens : aliensPerSpawn;

                // Loop through spawned aliens
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    //If there are less aliens on the screen than the max, add another alien
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        aliensOnScreen += 1;
                    }
                   
                    //Spawn Location (-1 for not set)
                    int spawnPoint = -1;
                    //Loop until a spawn point if found
                    while (spawnPoint == -1)
                    {
                        //Produces a random number which is the spawn point
                        int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                        
                        //Checks to see is generated number is being used by another alien,
                        //if true, rerolls a new spawn point
                        if (!previousSpawnLocations.Contains(randomNumber))
                        {
                            previousSpawnLocations.Add(randomNumber);
                            spawnPoint = randomNumber;
                        }
                    }
                    //Takes spawn point from code above
                    GameObject spawnLocation = spawnPoints[spawnPoint];

                    //Spawns a new alien
                    GameObject newAlien = Instantiate(alien) as GameObject;

                    //Positions the alien at it's spawn point
                    newAlien.transform.position = spawnLocation.transform.position;

                    //Gives reference to alien script
                    Alien alienScript = newAlien.GetComponent<Alien>();

                    //alien target is space marine
                    alienScript.target = player.transform;

                    //Rotate towards player
                    Vector3 targetRotation = new Vector3(player.transform.position.x,
                        newAlien.transform.position.y, player.transform.position.z);
                    newAlien.transform.LookAt(targetRotation);
                }
            }
        }
    }
}
