using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //where is the enemy suppose to detect the player?
    [SerializeField]
    private float attentionDistance;
    [SerializeField]
    private float attentionAngle;
    //eyePosition to check visibility of the player
    [SerializeField]
    private Transform eyePosition;

    //Distance to always detect the player
    [SerializeField]
    private float minAttentionDistance;

    //Agent to move the enemy in case it detects the player or idle movement
    private NavMeshAgent agent;

    //how far should the enemy wander?
    [SerializeField]
    private float walkRadius;
    //[Tooltip()]
    [SerializeField]
    private float walkIntervall;
    //how long should the player be followed?
    [SerializeField]
    private float pursueRadius;

    //Engagement distance (large for ranged, small for melee)
    [SerializeField]
    public float maxEngagementDistance;

    //true if player is detected
    private bool inCombat=false;

    [SerializeField]
    public bool isRangedWeapon;

    private bool isRunning=false;

    /// <summary>
    /// general variables
    /// </summary>
    //timer to manage time
    private float timer = 0.0f;
    private Vector3 point;
    private GameObject player;

    //Assign agent
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        point = GetIdlePoint(0.0f);
    }

    //Update once per frame
    private void Update()
    {
        if(FindObjectOfType<CombatController>())
            player = FindObjectOfType<CombatController>().gameObject;
        if (player == null)
        {
            return;
        }
        //Debug.Log(agent.isStopped);
        agent.isStopped = false;
        isInRange();
        //Check if the player is detected and set status accordingly
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (isVisible() || isInRange()&&!inCombat)
        {
            inCombat = true;
        }

        if (inCombat && !isVisible()&&!isRunning)
        {
            if (distance > pursueRadius)
            {
                inCombat = false;
            }
            else
            {
                if (!isVisible()&&distance>=maxEngagementDistance)
                {
                    point = player.transform.position;
                    agent.SetDestination(point);
                }
                else
                {
                    point = GetNextPoint(distance);
                    agent.SetDestination(point);
                }
                
            }
        }

        else if (inCombat&& isVisible()&& !isRunning)
        {
            if (distance < maxEngagementDistance&&isRangedWeapon)
            {
                agent.isStopped = true;
            }
            else if(distance>= pursueRadius)
            {
                inCombat = false;
            }
            else if(distance>=maxEngagementDistance&&distance<pursueRadius)
            {
                point=GetNextPoint(distance);
                agent.SetDestination(point);
            }
        }
        

        //turn to the player if in combat and not moving
        if (inCombat && agent.velocity.magnitude <= 0.01f)
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = player.transform.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = 3.0f * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            //Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

        }


        //Timer to time the random idle movement right
        if (timer >= walkIntervall && !inCombat)
        {
            timer = 0.0f;
            point = GetIdlePoint(walkRadius);
            agent.SetDestination(point);
        }
        else
        {
            timer+=Time.deltaTime;
        }

        if (!readyToFire() && distance < minAttentionDistance*0.8f&&isRangedWeapon&&inCombat)
        {
            isRunning = true;
            agent.isStopped = false;
            point = transform.position - (player.transform.position-transform.position) * 1.3f;
            agent.SetDestination(point);
        }
        else if(distance < minAttentionDistance&&isRangedWeapon)
        {
            isRunning = false;
        }

        if (!isRangedWeapon&& inCombat&&distance>maxEngagementDistance)
        {
            Vector3 playerVector = player.transform.position - transform.position;
            playerVector -= playerVector.normalized * maxEngagementDistance * 0.5f;
            playerVector += transform.position;
            point = playerVector;
            agent.SetDestination(point);

        }

        

    }

    //Get the next point the enemy is about to move when in combat
    //this means engaging the player completely in case of a melee weapon and getting into gun range -margin when using a ranged weapon
    private Vector3 GetNextPoint(float distance)
    {
        Vector3 playerVector = player.transform.position - transform.position;
        if (isRangedWeapon&&distance>maxEngagementDistance)
        {
            playerVector = playerVector.normalized * maxEngagementDistance * 0.4f;
        }
        else if (isRangedWeapon && distance <= maxEngagementDistance)
        {
            playerVector = transform.position;
            
        }
        else if(!isRangedWeapon && distance > maxEngagementDistance)
        {
            playerVector -= playerVector.normalized*maxEngagementDistance*0.5f;
        }
        playerVector += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(playerVector, out hit, 0.0f, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;

    }

    //Get a point on the NavMesh to randomly move to when the player is not in sight
    private Vector3 GetIdlePoint(float m_walkRadius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * m_walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }

    //check if player is in range
    private bool isInRange()
    {
        if(Vector3.Distance(player.transform.position, gameObject.transform.position)< minAttentionDistance)
        {
            if (isRangedWeapon)
            {
                agent.isStopped = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //check if the enemy can see the player
    private bool isVisible()
    {
        Ray ray = new Ray(eyePosition.position,player.transform.position-eyePosition.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, attentionDistance))
        { 
            if (hit.collider.gameObject.GetComponent<CombatController>()&& Vector3.Distance(player.transform.position, gameObject.transform.position) <= attentionDistance&&Vector3.Angle(player.transform.position-transform.position,transform.forward)<= attentionAngle/2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    //method for checking if the enemy is ready to fire, called by enemy Combat script
    public bool readyToFire()
    {
        if (player)
        {
            //Debug.ClearDeveloperConsole();
            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            /*Debug.Log("Angle: " + (Vector3.Angle(player.transform.position - transform.position, transform.forward)<2.5f));
            Debug.Log("distance: "+ (distance < maxEngagementDistance));
            Debug.Log("Can see: " + isVisible());*/
            if (isVisible() && distance < maxEngagementDistance && Vector3.Angle(new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z), new Vector3(transform.forward.x, 0, transform.forward.z)) < 2.5f)
                return true;
            else
                return false;
        }
        else return false;
    }

    public bool readyToSlash()
    {
        if (player)
        {
            //Debug.ClearDeveloperConsole();
            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            /*Debug.Log("Angle: " + (Vector3.Angle(player.transform.position - transform.position, transform.forward)<2.5f));
            Debug.Log("distance: "+ (distance < maxEngagementDistance));
            Debug.Log("Can see: " + isVisible());*/
            if (distance < maxEngagementDistance)
                return true;
            else
                return false;
        }
        else return false;
       
    }

    /*void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, pursueRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, minAttentionDistance);
    }*/

}
