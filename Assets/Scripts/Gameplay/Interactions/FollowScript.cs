using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowScript : MonoBehaviour
{

    public NavMeshAgent enemy;
    public Transform player;
    public float startFollowingDistance = 10f; 
    public float stopFollowingDistance = 10f;

    private bool isFollowing = false; 

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        
        if (distanceToPlayer <= startFollowingDistance && !isFollowing)
        {
            isFollowing = true; // Start following
        }
        
        else if (distanceToPlayer > stopFollowingDistance && isFollowing)
        {
            isFollowing = false; // Stop following
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