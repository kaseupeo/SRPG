using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    private Dictionary<Vector2Int, OverlayTile> _mapTiles = new Dictionary<Vector2Int, OverlayTile>();
    public Dictionary<Vector2Int, OverlayTile> MapTiles => _mapTiles;

    public void Init()
    {
        
    }
    
    // TODO : TileMap을 여러개로 받을때도 가능하게 만들기
    public void GenerateOverlayTile(List<Tilemap> tileMaps)
    {
        foreach (Tilemap tile in tileMaps)
        {
            GenerateOverlayTile(tile);
        }
    }
    
    public void GenerateOverlayTile(Tilemap tileMap)
    {
        BoundsInt bounds = tileMap.cellBounds;

        GameObject overlayTilePrefab = Resources.Load("Prefabs/OverlayTile") as GameObject;
        GameObject overlayContainer = new GameObject("OverlayContainer");

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
                        GameObject overlayTile = GameObject.Instantiate(overlayTilePrefab, overlayContainer.transform);
                        Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1f);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.GetComponent<OverlayTile>().GridLocation = new Vector3Int(x, y, z);
                            
                        _mapTiles.Add(tileKey, overlayTile.GetComponent<OverlayTile>());
                    }
                }
            }
        }
    }
}
