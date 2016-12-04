using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField]
    private GameObject m_Prefab;

    [SerializeField]
    private float m_Range;

    [SerializeField]
    private int m_Amount;

    private List<Boid> m_BoidList;
    public List<Boid> BoidList { get { return m_BoidList; } }

    private GameObject m_Parent;

    [SerializeField]
    private float m_MovementSpeed = 1f;

    private Vector3 m_CurrentDirection, m_NewDirection;

    private float m_Module;

    private float m_Rnd;
    private Light m_Light;

    private Color m_LightColor, m_BoidColor;

    void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_Light = GetComponent<Light>();
    }

    void Start()
    {
        m_MovementSpeed /= 100;

        m_BoidList = new List<Boid>();

        m_Parent = new GameObject();
        m_Parent.name = "Group";

        m_Rnd = Random.value * 100;

        //for (int i = 0; i < m_Amount; i++)
        //{
        //    Spawn(this.transform.position + Random.insideUnitSphere * m_Range);
        //}

        //AssignRandomLeader();

        m_NewDirection = Random.insideUnitSphere;
        m_CurrentDirection = m_NewDirection;

        m_Module = 1;

        m_BoidColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    void Update()
    {
        if (m_GameManager.Tick % 300 == 0) m_BoidColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

        if (m_GameManager.Tick % (int)m_Module == 0)
        {
            Spawn(this.transform.position);
            if (m_Module < 60) m_Module *= 1.1f;
            if (Random.value < .1f) m_Module = 20;
        }

        RandomizeLightIntensity();
        //RandomizeLightColor();

        //HandleMovement();
    }

    private void AssignRandomLeader()
    {
        Boid leader = m_BoidList[Random.Range(0, m_BoidList.Count)];
        leader.IsLeader = true;
        leader.name += "Leader";
    }

    private void HandleMovement()
    {
        if (Random.value < .3f) m_NewDirection = Random.insideUnitSphere;

        m_CurrentDirection = Vector3.Slerp(m_CurrentDirection, m_NewDirection, Time.deltaTime);

        this.transform.Translate(m_CurrentDirection * m_MovementSpeed);
    }

    public GameObject Spawn( Vector3 pos )
    {
        Boid b = ((GameObject)Instantiate(m_Prefab, pos, Quaternion.identity)).GetComponent<Boid>();
        b.transform.SetParent(m_Parent.transform);
        m_BoidList.Add(b);
        b.Init(this, m_BoidList.Count, m_BoidColor);

        return b.gameObject;
    }

    #region Light Effects
    private void RandomizeLightIntensity()
    {
        m_Light.intensity = 2 * Mathf.PerlinNoise(m_Rnd + Time.time, m_Rnd + 1 + Time.time * 1);
    }

    private void RandomizeLightColor()
    {
        m_LightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        m_Light.color = Color.Lerp(m_Light.color, m_LightColor, Time.deltaTime);
    }
    #endregion
}
