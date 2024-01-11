using System;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private void Awake()
    {
        
    }

    public void StartGame()
    {
        Managers.Scene.LoadScene(Define.SceneType.GameScene);
    }
}