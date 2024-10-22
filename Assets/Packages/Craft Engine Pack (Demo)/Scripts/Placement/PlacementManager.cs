using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour {
    public Material m_ghostPossible;// color of ghost when it is possible to instantiate object at this place
    public Material m_ghostImpossible;//color of ghost when it is impossible to instatniate object there

    public float m_maxPlacementDistance = 2.0f;//maximum distance to place object

    public float m_rotationSpeed = 100.0f;// rotation object in placement mode

    public Camera m_camera;

    bool m_placable; // is it possible to place object there
    float m_height; // height of prefab posted on

    GameObject m_object;
    GameObject m_ghost;

    public GameObject m_objectToPlace
    {
        get
        {
            return m_object;
        }
        set
        {
            m_placable = false; // after change it is impossible to place
            m_object = value;
            if (value == null)// disable placement mode (when value is null) -> destroy ghost
            {
                if (m_ghost)
                    Destroy(m_ghost);
                m_ghost = null;
                return;
            }
            m_ghost = Instantiate(value);
            m_height = m_ghost.transform.position.y;

            //disable all that can disturb us

            foreach (Rigidbody r in m_ghost.GetComponentsInChildren(typeof(Rigidbody), true))
                Destroy(r);

            if (m_ghost.GetComponent<HitableObject>())
                Destroy(m_ghost.GetComponent<HitableObject>());

            if (m_ghost.GetComponent<InteractableObject>())
                Destroy(m_ghost.GetComponent<InteractableObject>());

            foreach (Collider c in m_ghost.GetComponentsInChildren<Collider>())
                c.isTrigger = true;

            PaintMesh(m_ghostImpossible);//set ghost impossible
        }
    }
    void FixedUpdate()
    {
        if (m_ghost)
        {
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_maxPlacementDistance, 1 << 9))//raycast terrain
            {
                m_ghost.transform.position = new Vector3(hit.point.x, hit.point.y + m_height, hit.point.z);
                if (m_ghost.GetComponent<PlacableItem>().m_isIntersect)
                {
                    m_placable = false;
                    PaintMesh(m_ghostImpossible);
                }
                else
                {
                    m_placable = true;
                    PaintMesh(m_ghostPossible);
                }
            }
        }
    }
    void PaintMesh(Material mat)
    {
        foreach (Renderer r in m_ghost.GetComponentsInChildren(typeof(Renderer), true))
            r.material = mat;
    }
    public bool PlaceObject()
    {
        if (!m_placable)
            return false;
        Instantiate(m_objectToPlace, m_ghost.transform.position, m_ghost.transform.rotation);
        m_objectToPlace = null;
        return true;
    }
    public void RotateObject()
    {
        if(m_ghost)
            m_ghost.transform.Rotate(m_ghost.transform.up, m_rotationSpeed * Time.deltaTime);
    }
}
