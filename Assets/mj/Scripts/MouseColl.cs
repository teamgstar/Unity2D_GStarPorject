using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseColl : MonoBehaviour
{

    private Vector3 m_MousePosition;
    private Camera m_Camera;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {

            m_MousePosition = Input.mousePosition;
            m_MousePosition = m_Camera.ScreenToWorldPoint(m_MousePosition);
            transform.position = m_MousePosition;
        }
        else
            transform.position = new Vector3(999, 999, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("Hello");
        }
    }
}
