using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private GameObject startTile;

    private Tilemap[] _tileMaps;
    private Tilemap _startTile;
    
    private void Start()
    {
        _tileMaps = mapPrefab.GetComponentsInChildren<Tilemap>();
        // _startTile = 
        Managers.Game.Init();
        Managers.Map.GenerateOverlayTile(_tileMaps);
    }

    private void Update()
    {
        
    }

    public void GameModeChange()
    {
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
    }

    public void FinishedPlayerTurn()
    {
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;
    }
}
