using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform targetObj;

    public float moveSpeed = 10f;

    public float stoppingDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTopPlayer = Vector3.Distance(transform.position, targetObj.position);


        if (distanceTopPlayer > stoppingDistance)
        {

            transform.position = Vector3.MoveTowards(this.transform.position, targetObj.position, 10 * Time.deltaTime);

        }
    }

}
