using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region 싱글톤

    private static Managers _instance;

    public static Managers Instance
    {
        get
        {
            Init();

            return _instance;
        }
    }

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("Managers");

            if (go == null)
            {
                go = new GameObject("Managers");
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();
        }
    }

    #endregion

    private GameManager _game = new GameManager();
    private PoolManager _pool = new PoolManager();
    
    private static GameManager Game => Instance._game;
    public static PoolManager Pool => Instance._pool;
    
    
    public static void Clear()
    {
        
    }
}
