using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{
    public  float MaxRange;

    public GameObject line;
    public GameObject ClickLine;
    private LineRenderer m_LineRenderer;

    private GameObject m_Player;
    private SpriteRenderer m_Render;

    private PlayerMovement m_PlayerMovement;

    // Use this for initialization

    private void Awake()
    {
        m_LineRenderer      = ClickLine.GetComponent<LineRenderer>();
        m_Render            = line.GetComponent<SpriteRenderer>();
        m_Player = GameObject.FindWithTag("Player");
        m_PlayerMovement = m_Player.GetComponent<PlayerMovement>();

        //라인렌더러 설정

    }
    void Start()
    {
        //시작 컬러값과 나중 컬러값 설정, 이러면 서서히 색이 바뀌게 된다.
        m_LineRenderer.startColor = Color.red;
        m_LineRenderer.endColor = Color.yellow;

        //시작 넓이와 나중 넓이 설정, 이러면 서서히 넓이가 바뀌게 된다.
        m_LineRenderer.startWidth = 0.25f;
        m_LineRenderer.endWidth = 0.0f;
        
        //터치 스크롤시에만 렌더링 될 것 이므로 일단은 기능 꺼놓기
        m_LineRenderer.enabled = false;
        m_Render.enabled = false;

        //플레이어 오브젝트를 찾는다

    }


    // Update is called once per frame
    void Update()
    {
        //Press 상태일경우
        if (m_PlayerMovement.m_Status == PlayerMovement.PlayerStatus.PS_Press)
        {
            //플레이어 기준 라인이므로 좌표값을 플레이어의 좌표값으로 설정해줘야한다.
            this.transform.position = m_Player.transform.position;

            //터지 기준 라인이므로 처음 마우스 좌표값 에서 나중 마우스 좌표값 까지 선을 이어준다.
            m_LineRenderer.SetPosition(1, new Vector3(m_PlayerMovement.m_StartPos.x, m_PlayerMovement.m_StartPos.y, -1));
            m_LineRenderer.SetPosition(0, new Vector3(m_PlayerMovement.m_EndPos.x, m_PlayerMovement.m_EndPos.y, -1));

            //렌더러 기능들 모두 켜준다.
            m_LineRenderer.enabled = true;
            m_Render.enabled = true;

        }
        else
        {
            //이어진 선을 모두 풀어주고
            m_LineRenderer.SetPosition(1, Vector3.zero);
            m_LineRenderer.SetPosition(0, Vector3.zero);

            //렌더러 기능들 모두 꺼준다.
            m_LineRenderer.enabled = false;
            m_Render.enabled = false;
        }

        //처음좌표값과 나중좌표값의 각도 (아크탄젠트)를 구하고
        float Rot = Mathf.Atan2(-m_PlayerMovement.m_Direction.y, -m_PlayerMovement.m_Direction.x) * 180 / Mathf.PI;

        //플레이어 기준 라인을 회전시켜준다.
        this.transform.rotation =  Quaternion.Euler(0, 0, Rot+ 90);

        //두 점사이의 거리를 구해서
        float distance = Mathf.Abs(Vector2.Distance(m_PlayerMovement.m_EndPos, m_PlayerMovement.m_StartPos));


        //최대 거리보다 길 경우 설정해준다.
        if (distance   > MaxRange)
        { distance = MaxRange; }

        //선의 길이도 늘려준다.
        this.transform.localScale = new Vector3(1, distance, 1);
    }
}