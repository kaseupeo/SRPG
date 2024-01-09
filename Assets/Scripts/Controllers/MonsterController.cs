using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArrowDirection = Define.ArrowDirection;

public class MonsterController : MonoBehaviour
{
    private List<Monster> _monsters;
    private List<PlayerCharacter> _playerCharacters;
    
    
    public void MonsterMoveCheck()
    {
        if (_playerCharacters == null || _monsters == null)
            return;

        foreach (Monster monster in _monsters)
        {
            
        }
        
    }
}
