﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed;

    private enum PlayerStatus : byte
    {
        PS_Idle,
        PS_Press,
        PS_Move
    }

    private PlayerStatus m_Status;

    private Vector2 m_Direction;
    private Vector2 m_UpPos;
    private Vector2 m_DownPos;

    private Camera m_Camera;

    private float m_Rot = 0;
    void Start()
    {
        m_Rot       = 0.0f;
        m_Direction = Vector2.zero;
        m_UpPos     = Vector2.zero;
        m_DownPos   = Vector2.zero;
        m_Status    = PlayerStatus.PS_Idle;
        m_Camera    = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        switch (m_Status)
        {
            case PlayerStatus.PS_Idle:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_Status    = PlayerStatus.PS_Press;
                    m_DownPos   = Input.mousePosition;
                }
                break;
            case PlayerStatus.PS_Press:
                GameManager.g_GameSpeed = 0.5f;

                m_UpPos = Input.mousePosition;
                m_Direction = m_UpPos - m_DownPos;
                m_Direction.Normalize();
                m_Rot = Mathf.Atan2(-m_Direction.y, -m_Direction.x) * 180 / Mathf.PI;
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    m_UpPos = Input.mousePosition;
                    m_Direction = m_DownPos - m_UpPos;
                    m_Direction.Normalize();
                    m_Rot = Mathf.Atan2(-m_Direction.y, -m_Direction.x) * 180.0f / Mathf.PI;
                    this.GetComponent<Rigidbody2D>().AddForce(m_Direction * MoveSpeed, ForceMode2D.Impulse);
                    m_Status = PlayerStatus.PS_Move;
                }
                break;
            case PlayerStatus.PS_Move:
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_Status = PlayerStatus.PS_Press;
                    this.GetComponent<Rigidbody2D>().velocity *= 0.5f;
                }
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            m_Status = PlayerStatus.PS_Idle;
        }
    }
}