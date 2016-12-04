using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int m_Tick = 0;
    public int Tick { get { return m_Tick; } }

    void Awake()
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
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
