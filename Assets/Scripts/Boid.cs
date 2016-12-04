using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    private int m_ID;
    public int ID { get { return m_ID; } }

    private Spawner m_Spawner;

    [SerializeField]
    private float m_MovementSpeed;

    [SerializeField]
    private float m_InfluenceRange = 5f, m_AttractionEdge = .66f, m_AlignmentEdge = .33f;

    private Vector3 m_CurrentDirection, m_NewDirection;

    [SerializeField]
    private bool m_IsLeader = false;
    public bool IsLeader { get { return m_IsLeader; } set { m_IsLeader = value; } }

    private float m_Age;

    [SerializeField]
    private float m_AgeMax = 60;

    private TrailRenderer m_TrailRenderer;

    [SerializeField]
    private Light m_Light;

    private float m_StartRange;

    private Color m_LightColor;

    void Awake()
    {
        m_TrailRenderer = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        m_MovementSpeed /= 100;

        m_IsLeader = (Random.value < .05f);

        m_StartRange = m_Light.range;
        m_Light.range = .1f;

        m_Age = .0f;

        //m_LightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

        m_NewDirection = Random.insideUnitSphere;
        m_CurrentDirection = m_NewDirection;
    }

    public void Init( Spawner spawner, int id, Color color )
    {
        m_Spawner = spawner;
        m_ID = id;
        m_LightColor = color;
    }

    void Update()
    {
        m_Light.color = m_LightColor;

        m_Age += Time.deltaTime;

        if (m_Age >= m_AgeMax)
        {
            if (this.transform.localScale.x > .0f)
            {
                Vector3 scale = this.transform.localScale;
                scale.x -= .001f;
                scale.y -= .001f;
                scale.z -= .001f;
                this.transform.localScale = scale;
            }

            m_TrailRenderer.startWidth = this.transform.localScale.x;
            m_Light.range -= .002f;

            if (m_Light.range <= .01f) Destroy(gameObject);
        }
        else
        {
            if (m_Light.range < m_StartRange) m_Light.range += .01f;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!m_IsLeader) m_NewDirection = (m_NewDirection + CalculateNewDirection()/* + ((m_Spawner.transform.position - this.transform.position) * .5f)*/).normalized;

        if (Random.value < .3f) m_NewDirection = Random.insideUnitSphere;

        m_CurrentDirection = Vector3.Slerp(m_CurrentDirection, m_NewDirection, Time.deltaTime);

        //Quaternion lookRot = Quaternion.LookRotation(m_CurrentDirection);
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRot, Time.deltaTime * 5);

        this.transform.Translate(m_CurrentDirection * m_MovementSpeed);
    }

    private Vector3 CalculateNewDirection()
    {
        Vector3 directionNew = new Vector3();

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, m_InfluenceRange);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            Boid other = hitColliders[i].transform.parent.GetComponent<Boid>();

            float dist = Vector3.Distance(this.transform.position, other.transform.position);

            Vector3 dirToOther = other.transform.position - this.transform.position;

            // apply the flocking rules
            // attraction
            if (dist > (m_InfluenceRange * m_AttractionEdge))
            {
                directionNew += dirToOther * ((other.IsLeader) ? 10f : .01f);
            }

            // alignment
            if (dist < (m_InfluenceRange * m_AttractionEdge) && dist > (m_InfluenceRange * m_AlignmentEdge))
            {
                directionNew += other.GetCurrentDirection() * ((other.IsLeader) ? 1f : .01f);
            }

            // repulsion
            if (dist < (m_InfluenceRange * m_AlignmentEdge))
            {
                directionNew -= dirToOther * .01f;
            }
        }

        return directionNew;
    }

    public Vector3 GetCurrentDirection()
    {
        return m_CurrentDirection;
    }
}
