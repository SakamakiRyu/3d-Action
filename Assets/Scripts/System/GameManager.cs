using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager m_instance;
    public static GameManager Instance => m_instance; 
    private GameManager() { }

    private void Awake()
    {
        var go = GameObject.FindObjectOfType<GameManager>();
        if (go)
        {
            m_instance = go;
            Destroy(gameObject);
        }
    }
}
