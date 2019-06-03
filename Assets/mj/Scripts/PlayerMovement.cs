using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed;
    [HideInInspector] public enum PlayerStatus : byte
    {
        PS_Idle,
        PS_Press,
        PS_Move
    }
    [HideInInspector] public float m_Rot;
    [HideInInspector] public PlayerStatus m_Status;


    [HideInInspector] public Vector2 m_Direction;
    [HideInInspector] public Vector2 m_UpPos;
    [HideInInspector] public Vector2 m_DownPos;
    private Camera m_Camera;

    private void Awake()
    {
        m_Rot = 0.0f;
        m_Direction = Vector2.zero;
        m_UpPos = Vector2.zero;
        m_DownPos = Vector2.zero;
        m_Status = PlayerStatus.PS_Idle;
    }

    void Start()
    {
        m_Camera    = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        //m_DownPos = transform.position
        switch (m_Status)
        {
            case PlayerStatus.PS_Idle:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_Status    = PlayerStatus.PS_Press;
                    m_DownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;
            case PlayerStatus.PS_Press:
                GameManager.g_GameSpeed = 0.025f;
                m_UpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_Direction = m_UpPos - m_DownPos;
                m_Direction.Normalize();
                m_Rot = Mathf.Atan2(-m_Direction.y, -m_Direction.x) * 180 / Mathf.PI;
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    m_UpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    m_Direction = m_UpPos - m_DownPos;
                    m_Direction.Normalize();

                    m_Rot = Mathf.Atan2(-m_Direction.y, -m_Direction.x) * 180.0f / Mathf.PI;
               
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    this.GetComponent<Rigidbody2D>().AddForce(m_Direction * MoveSpeed * Time.deltaTime * 10, ForceMode2D.Impulse);

                    m_Status = PlayerStatus.PS_Move;
                }
                break;
            case PlayerStatus.PS_Move:
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_DownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    m_Status = PlayerStatus.PS_Press;
                    this.GetComponent<Rigidbody2D>().velocity *= GameManager.g_GameSpeed;           
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