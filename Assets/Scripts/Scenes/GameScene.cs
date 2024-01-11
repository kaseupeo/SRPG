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
    
    private void Awake()
    {
        Managers.Game.Init();
        Managers.Map.Init();
    }

    public void Preparation()
    {
        Managers.Map.GenerateOverlayTile(Managers.Game.Map);
    }
    
    public void BattleStart()
    {
        Managers.Game.SelectedCharacter = null;
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        Managers.Game.GenerateRandomMonster();
    }

    public void FinishedPlayerTurn()
    {
        foreach (PlayerCharacter playerCharacter in Managers.Game.PlayerCharacters) 
            playerCharacter.State = Define.State.Idle;
        
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;
        StartCoroutine(Managers.Game.CoMovePath(Managers.Game.Monsters, Managers.Game.PlayerCharacters));
    }
}
