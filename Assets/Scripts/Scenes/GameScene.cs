using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameScene : MonoBehaviour
{
    private Tilemap _startTile;
    
    private void Awake()
    {
        Managers.Game.Init();
        Managers.Map.Init();
    }
    
    public void BattleStart()
    {
        if (Managers.Game.PlayerCharacters.Count == 0)
            return;
        
        Managers.Game.SelectedCharacter = null;
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        Managers.Game.GenerateMonster();
    }

    public void FinishedPlayerTurn()
    {
        foreach (PlayerCharacter playerCharacter in Managers.Game.PlayerCharacters) 
            playerCharacter.State = Define.State.Idle;
        
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;
        StartCoroutine(Managers.Game.CoMovePath(Managers.Game.Monsters, Managers.Game.PlayerCharacters));
    }
}
