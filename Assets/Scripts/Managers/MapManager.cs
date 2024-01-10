using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    private GameObject _overlayTilePrefab;
    
    private Dictionary<Vector2Int, Tile> _mapTiles = new Dictionary<Vector2Int, Tile>();
    private Dictionary<Vector2Int, Tile> _updateMapTiles = new Dictionary<Vector2Int, Tile>();
    public Dictionary<Vector2Int, Tile> MapTiles => _mapTiles;
    public Dictionary<Vector2Int, Tile> UpdateMapTiles
    {
        get
        {
            _updateMapTiles.Clear();
            
            foreach (var tile in _mapTiles.Values.Where(tile => !tile.IsBlocked))
            {
                _updateMapTiles.Add(tile.Grid2DLocation, tile);
            }
            
            return _updateMapTiles;
        }
    }

    public void Init()
    {
        
    }
    
    public void GenerateOverlayTile(Tilemap[] tileMaps)
    {
        _overlayTilePrefab = Resources.Load("Prefabs/UI/OverlayTile") as GameObject;
        
        foreach (Tilemap tile in tileMaps)
        {
            GenerateOverlayTile(tile);
        }
    }
    
    // 움직일수 있는 타일 생성
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
                        overlayTile.name = $"{overlayTile.GetComponent<Tile>().Grid2DLocation}";

                        _mapTiles.Add(tileKey, overlayTile.GetComponent<Tile>());
                    }
                }
            }
        }
    }

    public void ShowTile(List<Tile> rangeTile, Color color)
    {
        foreach (Tile tile in rangeTile)
            tile.ShowTile(color);
    }
    
    // 중심 타일에서 한 칸 간격으로 이동 가능한 타일 리스트(십자가모양)
    public List<Tile> GetSurroundingTiles(Vector2Int originTile)
    {
        List<Tile> surroundingTiles = new List<Tile>();
        
        Vector2Int tileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
        if (_mapTiles.ContainsKey(tileToCheck) && Mathf.Abs(_mapTiles[tileToCheck].transform.position.z - _mapTiles[originTile].transform.position.z) <= 1)
            surroundingTiles.Add(_mapTiles[tileToCheck]);

        tileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
        if (_mapTiles.ContainsKey(tileToCheck) && Mathf.Abs(_mapTiles[tileToCheck].transform.position.z - _mapTiles[originTile].transform.position.z) <= 1)
            surroundingTiles.Add(_mapTiles[tileToCheck]);

        tileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
        if (_mapTiles.ContainsKey(tileToCheck) && Mathf.Abs(_mapTiles[tileToCheck].transform.position.z - _mapTiles[originTile].transform.position.z) <= 1)
            surroundingTiles.Add(_mapTiles[tileToCheck]);

        tileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
        if (_mapTiles.ContainsKey(tileToCheck) && Mathf.Abs(_mapTiles[tileToCheck].transform.position.z - _mapTiles[originTile].transform.position.z) <= 1)
            surroundingTiles.Add(_mapTiles[tileToCheck]);

        return surroundingTiles;
    }

    public void UpdateTileInfo(Tile tile)
    {
        _mapTiles[tile.Grid2DLocation].GridLocation = tile.GridLocation;
    }
    
    public void Clear()
    {
        _mapTiles.Clear();
    }
}
