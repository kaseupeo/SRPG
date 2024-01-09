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
        Managers.Game.GenerateRandomMonster();
    }

    public void FinishedPlayerTurn()
    {
        foreach (PlayerCharacter playerCharacter in Managers.Game.PlayerCharacters) 
            playerCharacter.State = Define.State.Idle;
        
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;
        
        // foreach (Monster monster in Managers.Game.Monsters)
        // {
        //     StartCoroutine(Managers.Game.CoMovePath(monster, Managers.Game.PlayerCharacters));
        //
        // }

        int i = 0;
        while (true)
        {
            // bool isIdle = true;
            // foreach (Monster monster in Managers.Game.Monsters)
            // {
            //     if (monster.State != Define.State.Move)
            //         continue;
            //     isIdle = false;
            //     break;
            // }
            
            // var monster = Managers.Game.Monsters[i];
            if (Managers.Game.MonsterState == Define.State.Idle)
            {
                StartCoroutine(Managers.Game.CoMovePath(Managers.Game.Monsters[i], Managers.Game.PlayerCharacters));
                i++;
            }
        
            if (i == Managers.Game.Monsters.Count)
                break;
        }
    }

    public void FinishMonsterTurn()
    {
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        
        Managers.Game.ResetTurn();
    }
}
