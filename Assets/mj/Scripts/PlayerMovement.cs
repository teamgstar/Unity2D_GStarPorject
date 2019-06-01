using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Camera Camera;
    private Vector2 m_vDir = Vector2.zero;

    public float MoveSpeed;
    public  enum PlayerStatus : int
    {
        Start,
        Idle,
        Slide_On,
        Slide_Move
    }
    static public bool b_Move;

    static public PlayerStatus m_Status;
    static public Vector2 m_vSlideStartPos = Vector2.zero;
    static public Vector2 m_vSlideEndPos = Vector2.zero;
    
    static public float Rot = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_Status = PlayerStatus.Start;
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    //bool re;
    // Update is called once per frame
    private void Update()
    {
        switch (m_Status)
        {
            case PlayerStatus.Start:
            case PlayerStatus.Idle:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_Status = PlayerStatus.Slide_On;
                    m_vSlideStartPos = Input.mousePosition;
                    b_Move = false;
                }

                break;
            case PlayerStatus.Slide_On:

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    b_Move = true;
                    //m_Status = PlayerStatus.Slide_Move;
                    m_vSlideEndPos = Input.mousePosition;
                    m_vDir = m_vSlideEndPos - m_vSlideStartPos;
                    m_vDir.Normalize();
                    Rot = Mathf.Atan2(-m_vDir.y, -m_vDir.x) * 180 / Mathf.PI;

                    m_Status = PlayerStatus.Idle;
                }
                else 
                {
                    m_vSlideEndPos = Input.mousePosition;
                    m_vDir = m_vSlideEndPos - m_vSlideStartPos;
                    
                    m_vDir.Normalize();
                    Rot = Mathf.Atan2(-m_vDir.y, -m_vDir.x) * 180 / Mathf.PI;

                    //if (Input.GetKeyDown(KeyCode.Mouse0))
                    //{
                    //    m_vSlideStartPos = Input.mousePosition;
                    //    
                    //}
                }
                break;

        }
    }

    void FixedUpdate()
    {
        if (b_Move)
        {
            this.transform.Translate((m_vDir) * Time.deltaTime * MoveSpeed);
            m_Status = PlayerStatus.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            b_Move = false;
            m_Status = PlayerStatus.Idle;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            b_Move = false;
            m_Status = PlayerStatus.Idle;
        }
    }
}