using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{
    private const float m_MaxSize = 100f;

    public GameObject line;
    private GameObject m_Player;
    private SpriteRenderer m_Render;

    private PlayerMovement m_PlayerMovement;

    void Start()
    {
      
        m_Render = line.GetComponent<SpriteRenderer>();
        m_Render.enabled = false;

        m_Player = GameObject.FindWithTag("Player");
        m_PlayerMovement = m_Player.GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void Update()
    {

        if (m_PlayerMovement.m_Status == PlayerMovement.PlayerStatus.PS_Press)
        {
            Vector3 PlayerPos = m_Player.transform.position;
            this.transform.position = PlayerPos;
            m_Render.enabled = true;
        }
        else
        {
            this.transform.localScale = Vector3.zero;
            m_Render.enabled = false;
        }


        //Rot = playerScript.Rot;S
        //this.transform.Rotate(new Vector3(0 ,0, PlayerMovement.Rot));
        this.transform.rotation =  Quaternion.Euler(0, 0, m_PlayerMovement.m_Rot + 90);
        float distance = Mathf.Abs(Vector2.Distance(m_PlayerMovement.m_UpPos, m_Player.transform.position));

        if (distance  * m_Render.sprite.texture.width > m_MaxSize)
        { distance = m_MaxSize / m_Render.sprite.texture.width; }

        this.transform.localScale = new Vector3(1, distance, 1);
    }
}