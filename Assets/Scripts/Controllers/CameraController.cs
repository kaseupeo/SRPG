using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float cameraSpeed = 0.5f;

    private Vector3 _lastMouse;
    private float _totalRun;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.y < 0)
            _camera.transform.Translate(Input.mousePosition.normalized * Time.deltaTime * 0.5f);
        if (Input.mousePosition.x < 0)
            _camera.transform.Translate(Input.mousePosition.x * Time.deltaTime * cameraSpeed, 0, 0);
        else if (Input.mousePosition.y < 0)
            _camera.transform.Translate(0, Input.mousePosition.y * Time.deltaTime * cameraSpeed, 0);
        // TODO : 카메라 이동 


    }
}