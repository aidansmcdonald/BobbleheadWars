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

    //Upgrade/pickup
    public GameObject upgradePrefab; 
    //Ref to gun script
    public Gun gun;
    //Max time before upgrade spawns
    public float upgradeMaxTimeSpawn = 7.5f;
    //Tracks if pickup spawned, only one can spawn
    private bool spawnedUpgrade = false;
    //Tracks the current time until upgrade spawns
    private float actualUpgradeTime = 0;
   
    private float currentUpgradeTime = 0;

    public GameObject DeathFloor;

    public Animator arenaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Determine upgrade time
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //If there is no player, return
        if (player == null)
        {
            return;
        }

        //Increase current upgrade time each frame
        currentUpgradeTime += Time.deltaTime;

        if (currentUpgradeTime > actualUpgradeTime)
        {
            // After random time passes, check if pickup is spawned
            if (!spawnedUpgrade)
            {
                // Spawn at spawn point
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // Associates gun with upgrade
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // Upgrade spawn is now true
                spawnedUpgrade = true;

                //Sound effect for pickup
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

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

                        //Call addlistener on event, AlienDestroyed is called when event occurs
                        alienScript.OnDestroy.AddListener(AlienDestroyed);

                        alienScript.GetDeathParticles().SetDeathFloor(DeathFloor);
                    }
                }
            }
        }
    }

    public void AlienDestroyed()
    {
        //Decreases aliens on screen
        aliensOnScreen -= 1;
        //Decreases aliens needed to win
        totalAliens -= 1;

        //End game once aliens are killed
        if (totalAliens == 0)
        {
            Invoke("endGame", 2.0f);
        }
    }

    private void endGame()
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.
        elevatorArrived);
        arenaAnimator.SetTrigger("PlayerWon");
    }
}
