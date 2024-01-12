using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InputController : MonoBehaviour
{
    private PlayerActions _playerActions;

    [SerializeField] private GameObject menu;

    private void Awake()
    {
        _playerActions = new PlayerActions();
    }

    private void Start()
    {
        _playerActions.Keyboard.SettingsMenu.performed += _ => OpenMenu();
    }

    private void OpenMenu()
    {
        Debug.Log("setting");
        
        menu.SetActive(!menu.activeSelf);
    }
    
    private void OnEnable()
    {
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
    }
    
}