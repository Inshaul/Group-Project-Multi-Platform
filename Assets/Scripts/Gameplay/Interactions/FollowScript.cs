using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowScript : MonoBehaviour
{

    public NavMeshAgent enemy;
    public Transform player;
    public float startFollowingDistance = 10f;
    public float stopFollowingDistance = 15f;


    private bool isFollowing = false; // check is the enemy is following the player



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if ( distanceToPlayer <= startFollowingDistance && !isFollowing)
        {
            isFollowing = true;

        }


        else if  (distanceToPlayer > stopFollowingDistance && isFollowing)
        {
            isFollowing = false;
        }

        if (isFollowing)
        {
            enemy.SetDestination(player.position);
        }

        else
        {
            enemy.ResetPath();
        }
    }
}
