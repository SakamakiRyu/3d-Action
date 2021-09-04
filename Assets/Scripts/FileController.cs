using System.IO;
using UnityEngine;

public class FileController : MonoBehaviour
{
    [SerializeField, Header("Player")]
    Player m_player;

    public struct PlayerDate
    {
        public int hp;
        public int ep;

        public PlayerDate(int hp, int ep)
        {
            this.hp = hp;
            this.ep = ep;
        }
    }

    public void CreateFile()
    {
        string filePath = GetFilePath();
        Debug.Log($"{filePath}");
        File.Create(filePath);
    }

    public void DisplayPlayerInfo()
    {
        Debug.Log($"{m_player.CurrentHP} + {m_player.CurrentEP}");
    }

    public void Save()
    {
        using (var writer = new StreamWriter(GetFilePath()))
        {
            PlayerDate date = new PlayerDate(m_player.CurrentHP, m_player.CurrentEP);
            string json = JsonUtility.ToJson(date);
            writer.Write(json);
            Debug.Log(json);
        }
    }

    public void Load()
    {
        using (var reader = new StreamReader(GetFilePath()))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var my = JsonUtility.FromJson<PlayerDate>(line);
                m_player.SetDate(my.hp, my.ep);
            }
        }
        Debug.Log($"{m_player.CurrentHP} + {m_player.CurrentEP}");
    }

    string GetFilePath()
    {
        string filePath = Application.persistentDataPath + "\\" + "PlayerDate.txt";
        return filePath;
    }
}