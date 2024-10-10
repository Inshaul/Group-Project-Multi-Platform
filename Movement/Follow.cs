using UnityEngine;

public class Follow : MonoBehaviour 
{
    void start()
    {

    }


    void update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, targetObj.Position, 10 * Time.deltaTime);
    }
}