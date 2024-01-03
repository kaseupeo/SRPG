using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;

    private Tilemap _tileMap;
    
    private void Awake()
    {
        _tileMap = mapPrefab.GetComponentInChildren<Tilemap>();
        
        Managers.Map.GenerateOverlayTile(_tileMap);
    }
}
