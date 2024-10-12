using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotationWithMouseController : MonoBehaviour {
    public float m_sensitivity = 5.0f;
    public float m_minY = -80.0f;
    public float m_maxY = 90.0f;
    public Transform m_character;
    public bool Enabled { get; set; }
    float m_rotY = 0.0f;
    void Start()
    {
        Enabled = true;
    }
    void Update()
    {
        //just rotate camera of first view
        if (!Enabled)
            return;
        float m_rotX;
        m_rotX = Input.GetAxisRaw("Mouse X") * m_sensitivity;

        m_rotY += Input.GetAxisRaw("Mouse Y") * m_sensitivity;
        m_rotY = Mathf.Clamp(m_rotY, m_minY, m_maxY);

        transform.localEulerAngles = new Vector3(-m_rotY, 0.0f);
        m_character.transform.Rotate(m_character.transform.up, m_rotX);
    }
}
