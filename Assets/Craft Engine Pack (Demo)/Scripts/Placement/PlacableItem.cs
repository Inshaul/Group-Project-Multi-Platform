using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableItem : MonoBehaviour {
    public bool m_isIntersect
    {
        get { return m_intersectObjects.Count != 0; }
    }
    List<Collider> m_intersectObjects;

    // all code above is about count colliders that are intersect our collider
    void Awake()
    {
        m_intersectObjects = new List<Collider>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "PlacementTerrain" && collider.gameObject != gameObject)
            m_intersectObjects.Add(collider);
    }
    void OnTriggerExit(Collider collider)
    {
        m_intersectObjects.Remove(collider);
    }
}
