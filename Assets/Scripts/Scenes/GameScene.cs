using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

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

    public void BattleStart()
    {
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        Managers.Game.GenerateRandomMonster();
    }

    public void FinishedPlayerTurn()
    {
        Managers.Game.PlayerState = Define.State.Idle;
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;

        StartCoroutine(Managers.Game.CoMovePath(Managers.Game.Monsters[0], Managers.Game.PlayerCharacters[0]));
    }

    public void FinishMonsterTurn()
    {
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        
        Managers.Game.ResetTurn();
    }
}
