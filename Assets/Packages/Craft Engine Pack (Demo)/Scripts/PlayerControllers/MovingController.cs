using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoving
{
    void ClearValue();
    void MoveForward();
    void MoveBack();
    void MoveLeft();
    void MoveRight();
    void Jump();
    void Walk();
}

public class MovingController : MonoBehaviour, IMoving {
    public float m_speed = 10.0f;
    public float m_jumpStrength = 11.0f;

    Vector3 m_value;
    CharacterController m_player;
    float m_verticalVelocity = 0.0f;
    void Start()
    {
        m_player = GetComponent<CharacterController>();
        if (!m_player)
            throw new UnityException("Object needs a CharacterController");
    }


    public void ClearValue()
    {
        m_value = new Vector3();
    }

    public void MoveForward()
    {
        m_value += transform.forward * m_speed;
    }

    public void MoveBack()
    {
        m_value -= transform.forward * m_speed;
    }

    public void MoveLeft()
    {
        m_value -= transform.right * m_speed;
    }

    public void MoveRight()
    {
        m_value += transform.right * m_speed;
    }

    public void Jump()
    {
        if (m_player.isGrounded)
            m_verticalVelocity = m_jumpStrength;
    }
    void FixedUpdate()
    {
        if (!m_player.isGrounded)
            m_verticalVelocity += Physics.gravity.y * Time.deltaTime * 2.0f;
    }

    public void Walk()
    {
        m_player.Move((m_value + Physics.gravity + m_verticalVelocity * transform.up) * Time.deltaTime);
    }
}
