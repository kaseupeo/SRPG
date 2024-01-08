using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;

    private Tilemap[] _tileMaps;
    private Tilemap _startTile;
    
    private void Start()
    {
        _tileMaps = mapPrefab.GetComponentsInChildren<Tilemap>();
        Managers.Game.Init();
        Managers.Map.GenerateOverlayTile(_tileMaps);
    }

    private void Update()
    {
        
    }

    public void BattleStart()
    {
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        Managers.Game.GenerateRandomMonster();
    }

    public void FinishedPlayerTurn()
    {
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;
    }

    public void FinishMonsterTurn()
    {
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        
        Managers.Game.ResetTurn();
    }
}
