using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Setting")]
    public float distance = 0.5f;
    public float speed = 1.5f;

    //protected Rigidbody2D m_rb;
    protected Animator m_anim;
    protected SpriteRenderer m_visual;

    private Player player;
    private int m_index;

    private void Awake()
    {
        //m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
        m_visual = GetComponentInChildren<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (direction.magnitude > distance * m_index) {
            transform.position = player.transform.position - direction.normalized * distance * m_index;
            m_anim.SetFloat("Walk", 1);
            if (direction.x > 0) {
                m_visual.flipX = false;
                m_anim.SetFloat("Direction", 1);
            }
            else if (direction.x < 0) {
                m_visual.flipX = true;
                m_anim.SetFloat("Direction", -1);
            }
        }
        else {
            m_anim.SetFloat("Walk", 0);
        }
        //if (direction.magnitude > distance * (m_index + 1)) {
        //    m_rb.velocity = direction.normalized * speed * Time.deltaTime;
        //    m_anim.SetFloat("Walk", 1);
        //    if (direction.x > 0) {
        //        m_visual.flipX = false;
        //        m_anim.SetFloat("Direction", 1);
        //    }               
        //    else if (direction.x < 0) {
        //        m_visual.flipX = true;
        //        m_anim.SetFloat("Direction", -1);
        //    }
        //}
        //else {
        //    m_anim.SetFloat("Walk", 0);
        //}
    }

    public void RegisterGhost(Player p, int index)
    {
        player = p;
        m_index = index;
    }
}
