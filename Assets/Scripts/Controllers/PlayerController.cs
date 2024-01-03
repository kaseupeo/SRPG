using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private MouseInput _mouseInput;
    private Vector3 _destination;
    
    public Tilemap map;

    [SerializeField] private float moveSpeed;
    private Grid _gridLayout;

    private void Awake()
    {
        _mouseInput = new MouseInput();
    }

    private void Start()
    {
        _mouseInput.Mouse.MouseClick.performed += _ => MouseClick();
        _destination = transform.position;
        _gridLayout = map.transform.parent.GetComponentInParent<Grid>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _destination) > 0.1f)
            transform.position = Vector3.MoveTowards(transform.position, _destination, moveSpeed * Time.deltaTime);
    }

    private void MouseClick()
    {
        Vector2 mousePosition = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        
        if (map.HasTile(gridPosition))
        {
            _destination = _gridLayout.CellToWorld(gridPosition);
        }

        IsometricGrid.WorldPosToIsoPos(mousePosition);
        
        Debug.Log($"{gridPosition} : {map.HasTile(gridPosition)}");
    }

    private void OnEnable()
    {
        _mouseInput.Enable();
    }

    private void OnDisable()
    {
        _mouseInput.Disable();
    }
}
