using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int m_Tick = 0;
    public int Tick { get { return m_Tick; } }

    [SerializeField]
    private bool m_DrawGroupCenters = false;
    public bool DrawGroupCenters { get { return m_DrawGroupCenters; } }

    [SerializeField]
    private bool m_Dictatorship = false;
    public bool Dictatorship { get { return m_Dictatorship; } }

    void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    void Start()
    {
        m_Tick = 0;
    }

    void Update()
    {
        m_Tick++;

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
