using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessViaRayCast : MonoBehaviour {

    public Camera m_camera;
    public float m_maxDistance = 1.0f; // max distance to see interactable objects
    public Text m_interactionHintText;// text to print hint
    public InputManager m_inputManager;

    InputUnit m_interactionKey;// for print hint
    public InteractableObject m_currentObject { get; private set; }// object we see at the moment
    public HitableObject m_objectToHit { get; private set; }// hitable object we see at the moment
    Inventory m_inventory;
    void Start()
    {
        m_currentObject = null;
        m_objectToHit = null;
        m_interactionKey = m_inputManager.GetInputUnit(InputManager.InteractGroup, InputManager.Interaction);
        m_interactionHintText.text = "";
        m_inventory = GetComponent<Inventory>();
    }
    void Update()
    {
        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_maxDistance, 1 << 8)) // raycast forward from camera (only objects with "Visible" layer set)
        {
            m_objectToHit = hit.collider.gameObject.GetComponent<HitableObject>();
            if (m_currentObject = hit.collider.gameObject.GetComponent<InteractableObject>())
            {
                //print interaction key and hint to interact
                m_interactionHintText.text = m_interactionKey.Key.ToString() + " to " + (m_currentObject.m_itemDescription.m_isLiftable ? "pick up" : "interact with") + " a " + m_currentObject.m_itemDescription.m_name;
            }
            else
            {
                m_interactionHintText.text = "";
            }
        }
        else
        {
            m_interactionHintText.text = "";
            m_currentObject = null;
            m_objectToHit = null;
        }
    }

    void PickUp()//pick up liftable object
    {
        if (m_inventory.Put(m_currentObject.m_itemDescription))// if inventory records this object -> destroy it from scene
            DestroyObject(m_currentObject.gameObject);
        else
        {
            //if not -> drop it!
            m_currentObject.transform.position = transform.position + transform.forward * 2 + transform.up;
            m_currentObject.transform.rotation = Quaternion.Euler(Random.insideUnitSphere * 100);
        }
    }
    public void Interact()
    {
        if(m_currentObject)
        {
            if (m_currentObject.m_itemDescription.m_isLiftable)
                PickUp();
            else
                m_currentObject.Interact();
        }
    }
}
