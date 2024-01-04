using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;

    private Tilemap[] _tileMaps;
    
    private void Start()
    {
        _tileMaps = mapPrefab.GetComponentsInChildren<Tilemap>();
        
        Managers.Map.GenerateOverlayTile(_tileMaps);
    }

    private void Update()
    {
        
    }
}
