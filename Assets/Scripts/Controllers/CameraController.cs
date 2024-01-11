using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraMoveArea;

    private void Start()
    {
        _camera = Camera.main;
    }
    
    private void LateUpdate()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        var mousePosition = Input.mousePosition;

        if (!(mousePosition.x > 0) || !(mousePosition.y > 0) || !(mousePosition.x < Screen.width) ||
            !(mousePosition.y < Screen.height))
            return;

        float xRate = mousePosition.x / Screen.width;
        float yRate = mousePosition.y / Screen.height;

        if (xRate < cameraMoveArea)
            _camera.transform.Translate(Vector2.left * (Time.deltaTime * cameraMoveSpeed));
        if (xRate > 1 - cameraMoveArea) 
            _camera.transform.Translate(Vector2.right * (Time.deltaTime * cameraMoveSpeed));

        if (yRate < cameraMoveArea)
            _camera.transform.Translate(Vector2.down * (Time.deltaTime * cameraMoveSpeed));
        if (yRate > 1 - cameraMoveArea) 
            _camera.transform.Translate(Vector2.up * (Time.deltaTime * cameraMoveSpeed));
    }
}