using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    public Dictionary<Vector2Int, OverlayTile> MapTiles = new Dictionary<Vector2Int, OverlayTile>();
    
    public void Init()
    {
        
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
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    if (tileMap.HasTile(tileLocation) && !MapTiles.ContainsKey(tileKey))
                    {
                        OverlayTile overlayTile = GameObject.Instantiate(overlayTilePrefab, overlayContainer.transform).GetComponent<OverlayTile>();
                        Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1f);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.Node.GridLocation = tileLocation;
                        MapTiles.Add(tileKey, overlayTile.GetComponent<OverlayTile>());
                    }
                }
            }
        }
    }
}
