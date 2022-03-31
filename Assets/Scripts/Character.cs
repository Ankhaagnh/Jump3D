using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] Vector3 m_bodyVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] Rigidbody m_body = null;
    [SerializeField] Animator m_anim = null;
    [SerializeField] bool m_landing = true;
    [SerializeField] float m_rotationRate = 12f;

    [SerializeField] RaycastHit m_hit;
    [SerializeField] bool m_hittingTrampline;
    [SerializeField] bool m_fallingDown;

    public delegate void DelegateJump();
    public DelegateJump onJump;
    [SerializeField] Transform m_lastTrampline = null;
    [SerializeField] Transform m_NextTrampline = null;

    public Rigidbody Body { get { return m_body; } set { m_body = value; } }
    public RaycastHit Hit { get { return m_hit; } set { m_hit = value; } }
    public Transform LastTrampline { get { return m_lastTrampline; } set { m_lastTrampline = value; } }
    public Transform NextTrampline { get { return m_NextTrampline; } set { m_NextTrampline = value; } }
    public float RotationRate { get { return m_rotationRate; } set { m_rotationRate = value; } }
    public bool Landing { get { return m_landing; } set { m_landing = value; } }
    public bool FallingDown { get { return m_fallingDown; } set { m_fallingDown = value; } }
    public bool Hitting { get { return m_hittingTrampline; } set { m_hittingTrampline = value; } }
    public Animator Anim { get { return m_anim; } set { m_anim = value; } }
    public Vector3 BodyVelocity { get { return m_bodyVelocity; } set { m_bodyVelocity = value; } }

    public bool isLeadingTherace;
    public int lastTramplineIndex;

    public void SetFlipID() {
        int rand = Random.Range(1, 3);
        m_anim.SetInteger("FlipID", rand);
    }
    public void RemoveFlipID()
    {
        m_anim.SetInteger("FlipID", 0);
    }
    public void AddForce(Vector3 force, Transform trampline) {
        this.m_lastTrampline = trampline;
        m_body.AddForce(force);
        SetFlipID();

        if(!trampline.GetComponent<Trampline>().isBottomTrampoline)
        {
            lastTramplineIndex = trampline.GetComponent<Trampline>().index;
        }
    }
    public void Jump(Transform platform, float upForce)
    {
        var next = PlatformManager.instance.GetNextTrampline(platform);
        if (next == null)
        {
            next = PlatformManager.instance.GetNearestTrampline(platform);
        }
        m_NextTrampline = next;
        AddForce(new Vector3(0, upForce, 0), platform);
        if(onJump!=null)
            onJump();
    }
    public void Finish() {
        Debug.Log("Finish: "+gameObject.name);
    }
    public void AddForce(Vector3 force)
    {
        m_body.AddForce(force);
        SetFlipID();
    }
    public virtual void FixedUpdate() {
        m_bodyVelocity = m_body.velocity;
        m_anim.SetFloat("YVelocity", m_bodyVelocity.y);
        if (Physics.Raycast(transform.position, -transform.up, 1.2f) && m_bodyVelocity.y < 0)
        {
            m_landing = true;
        }
        else
            m_landing = false;
        m_anim.SetBool("Landing", m_landing);
        if (Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out m_hit)) {
            m_hittingTrampline = true;
        }else
            m_hittingTrampline = false;
        if (m_lastTrampline) {
       
            if (m_NextTrampline.position.y > transform.position.y&& m_bodyVelocity.y<0)
                m_fallingDown = true;
            else
                m_fallingDown = false;
        }
    }
}
