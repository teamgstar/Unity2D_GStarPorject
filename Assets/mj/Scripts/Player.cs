using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Camera Camera;


    private enum PlayerStatus : int
    {
        Start,
        Idle,
        Slide_On,
        Slide_Out,
        Slide_Move
    }

    PlayerStatus m_Status;

    private Vector2 m_vSlideStartPos = Vector2.zero;
    private Vector2 m_vSlideEndPos = Vector2.zero;
    private Vector2 m_vDir = Vector2.zero;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_Status = PlayerStatus.Start;
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        lineRenderer = GetComponent<LineRenderer>();
        // lineRenderer.SetColors(Color.red, Color.yellow);
        lineRenderer.startWidth = 1.0f;
        lineRenderer.endWidth = 1.0f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        //라인렌더러 처음위치 나중위치



    }

    // Update is called once per frame
    void Update()
    {
        switch (m_Status)
        {
            case PlayerStatus.Start:
            case PlayerStatus.Idle:
//#if UNITY_STANDALONE_WIN //윈도우
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {

                    Debug.Log("Down");
                    m_Status = PlayerStatus.Slide_On;
                    m_vSlideStartPos = Input.mousePosition;
               //     m_vSlideStartPos = Camera.ScreenToWorldPoint(m_vSlideStartPos);
                }
//#elif UNITY_ANDROID // 안드로이드
//        if (Input.GetTouch(0).phase == TouchPhase.Began)
//        {
//                    m_Status = PlayerStatus.Slide_On;
//                    m_vSlideStartPos = Input.mousePosition;        }
//#endif
                break;
            case PlayerStatus.Slide_On:
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    Debug.Log("Up");
                    m_Status = PlayerStatus.Slide_Out;
                    m_vSlideEndPos = Input.mousePosition;
            //        m_vSlideEndPos = Camera.ScreenToWorldPoint(m_vSlideStartPos);

                    m_vDir = m_vSlideEndPos - m_vSlideStartPos;
             
                    m_vDir.Normalize();
                    float Rot = Mathf.Atan2(-m_vDir.y, -m_vDir.x) * 180 / Mathf.PI;
                    //transform.Rotate(0.0f, 0.0f,Quaternion.FromToRotation(Vector3.up, m_vSlideEndPos - m_vSlideStartPos).z - 180);
                    //transform.rotation.z = 0;
                    Debug.Log(Rot);
    
                }
                lineRenderer.SetPosition(0, m_vSlideStartPos);
                lineRenderer.SetPosition(1, Input.mousePosition);
                 
                break;
            case PlayerStatus.Slide_Out:
                this.transform.Translate((m_vDir) * Time.deltaTime * 10.0f);

                break;
            case PlayerStatus.Slide_Move:
                break;
        }

    }
}