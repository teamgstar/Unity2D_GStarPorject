using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{
    private const float m_MaxSize = 100f;

    private Vector2 m_DownPos;
    private Vector2 m_UpPos;

    public GameObject line;
    public GameObject ClickLine;

    private GameObject m_Player;
    private SpriteRenderer m_Render;

    private PlayerMovement m_PlayerMovement;
    private LineRenderer m_LineRenderer;

    // Use this for initialization

    private void Awake()
    {

        //라인렌더러 설정
 
    }
    void Start()
    {
        m_LineRenderer = ClickLine.GetComponent<LineRenderer>();       
        m_LineRenderer.startColor = Color.red;
        m_LineRenderer.endColor = Color.yellow;
        m_LineRenderer.startWidth = 0.1f;
        m_LineRenderer.endWidth = 0.0f;
        m_LineRenderer.enabled = false;
        m_Render = line.GetComponent<SpriteRenderer>();
        m_Player = GameObject.FindWithTag("Player");
        m_PlayerMovement = m_Player.GetComponent<PlayerMovement>();
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, transform.position);

        m_Render.enabled = false;


        m_DownPos = Vector2.zero;
        m_UpPos = Vector2.zero;
    }


    // Update is called once per frame
    void Update()
    {
        switch(m_PlayerMovement.m_Status)
        {
            case PlayerMovement.PlayerStatus.PS_Press:
                this.transform.position = m_Player.transform.position;
                m_LineRenderer.enabled = true;
                m_Render.enabled = true;
                break;

            
        }

        m_DownPos = m_PlayerMovement.m_DownPos;
        m_UpPos = m_PlayerMovement.m_UpPos;
        m_LineRenderer.SetPosition(0, new Vector3(m_DownPos.x, m_DownPos.y, -1));
        m_LineRenderer.SetPosition(1, new Vector3(m_UpPos.x, m_UpPos.y, -1));

        if (m_PlayerMovement.m_Status != PlayerMovement.PlayerStatus.PS_Press)
        {
            this.transform.localScale = Vector3.zero;
            m_LineRenderer.enabled = false;
            m_Render.enabled = false;
        }


        //Rot = playerScript.Rot;S
        //this.transform.Rotate(new Vector3(0 ,0, PlayerMovement.Rot));
        this.transform.rotation =  Quaternion.Euler(0, 0, m_PlayerMovement.m_Rot + 90);
        float distance = Mathf.Abs(Vector2.Distance(m_PlayerMovement.m_UpPos, m_PlayerMovement.m_DownPos));

        if (distance  * m_Render.sprite.texture.width > m_MaxSize)
        { distance = m_MaxSize / m_Render.sprite.texture.width; }

        this.transform.localScale = new Vector3(1, distance, 1);
    }
}