using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    private GameObject _overlayTilePrefab;
    
    private Dictionary<Vector2Int, Tile> _mapTiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> MapTiles => _mapTiles;
    
    public void Init()
    {
        
    }
    
    public void GenerateOverlayTile(Tilemap[] tileMaps)
    {
        _overlayTilePrefab = Resources.Load("Prefabs/OverlayTile") as GameObject;
        
        foreach (Tilemap tile in tileMaps)
        {
            GenerateOverlayTile(tile);
        }
    }
    
    private void GenerateOverlayTile(Tilemap tileMap)
    {
        BoundsInt bounds = tileMap.cellBounds;
        
        // 모든 타일 맵 읽기 + 오버레이 타일 그리기
        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    Vector3Int tileLocation = new Vector3Int(x, y, z);
                    Vector2Int tileKey = new Vector2Int(x, y);
                    
                    if (tileMap.HasTile(tileLocation) && !_mapTiles.ContainsKey(tileKey))
                    {
                        GameObject overlayTile = GameObject.Instantiate(_overlayTilePrefab, tileMap.transform);
                        Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.GetComponent<Tile>().GridLocation = new Vector3Int(x, y, z);
                            
                        _mapTiles.Add(tileKey, overlayTile.GetComponent<Tile>());
                    }
                }
            }
        }
    }

    public void Clear()
    {
        _mapTiles.Clear();
    }
}
