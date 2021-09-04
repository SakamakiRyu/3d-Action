using UnityEngine;

public class SwordTest : MonoBehaviour
{
    [SerializeField, Header("剣のPrefab")]
    GameObject m_sword = default;

    [SerializeField, Header("剣の向きを設定する")]
    Quaternion m_swordRotation = default;

    GameObject[] m_swords = new GameObject[10];
    int m_inPossessionWeponCount = 1;

    public void ChengeWepon(int id)
    {
        foreach (var item in m_swords)
        {
            if (item.GetInstanceID() == id)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    public void AddWepon(GameObject sword)
    {
        m_swords[m_inPossessionWeponCount] = sword;
        m_inPossessionWeponCount++;
    }

    private void OnValidate()
    {
        transform.rotation = m_swordRotation;
    }
}