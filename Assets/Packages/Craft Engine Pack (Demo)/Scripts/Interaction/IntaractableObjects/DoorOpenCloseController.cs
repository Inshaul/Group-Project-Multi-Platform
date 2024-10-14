using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenCloseController : InteractableObject 
{
    bool m_closed = true;
    public Transform m_door;
    Collider m_doorCollider;

    void Start()
    {
        m_doorCollider = m_door.gameObject.GetComponent<Collider>();
    }
    public override void Interact()
    {
        if (m_closed)
        {
            //open door
            m_door.Rotate(Vector3.up, 90.0f);
            m_doorCollider.isTrigger = true;
            m_closed = false;
        }
        else
        {
            //close door
            m_door.Rotate(Vector3.up, -90.0f);
            m_doorCollider.isTrigger = false;
            m_closed = true;
        }
    }
}
