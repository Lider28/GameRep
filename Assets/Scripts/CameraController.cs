using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControl _cameraActions;
    private InputAction _movement;
    private Transform _cameraTransform;
    
    //movement
    [SerializeField] private float maxSpeed = 5f;
    private float _speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;
    
    //scroll
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 15f;
    [SerializeField] private float zoomSpeed = 2f;
    
    //screen edge
    [SerializeField] [Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
    [SerializeField] private bool useScreenEdge = true;

    private Vector3 _targetPosition;
    private float _zoomPosition;
    private float _zoomHeight;

    private Vector3 _horizontalVelocity;
    private Vector3 _lastPosition;

    private Vector3 _startDrag;


    private void Awake()
    {
        _cameraActions = new CameraControl();
        _cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        _zoomHeight = _cameraTransform.localPosition.y;
        _cameraTransform.LookAt(this.transform);
        _lastPosition = this.transform.position;
        _movement = _cameraActions.Camera.Movement;
        _cameraActions.Camera.Scroll.performed += ZoomCamera;
        _cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        _cameraActions.Camera.Scroll.performed -= ZoomCamera;
        _cameraActions.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();
        
        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    private void UpdateVelocity()
    {
        var position = this.transform.position;
        _horizontalVelocity = (position - _lastPosition) / Time.deltaTime;
        _horizontalVelocity.y = 0;
        _lastPosition = position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = _movement.ReadValue<Vector2>().x * GetCameraRight() +
                             _movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;
        if (inputValue.sqrMagnitude > 0.1f)
            _targetPosition += inputValue;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = _cameraTransform.right;
        right.y = 0;
        return right;
    }
    
    private Vector3 GetCameraForward()
    {
        Vector3 forward = _cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition()
    {
        if (_targetPosition.sqrMagnitude > 0.1f)
        {
            _speed = Mathf.Lerp(_speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += _targetPosition * (_speed * Time.deltaTime);
        }
        else
        {
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += _horizontalVelocity * Time.deltaTime;
        }
        
        _targetPosition = Vector3.zero;
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Math.Abs(value) > 0.1f)
        {
            _zoomHeight = _cameraTransform.localPosition.y + value * stepSize;
            if (_zoomHeight < minHeight)
                _zoomHeight = minHeight;
            else if (_zoomHeight > maxHeight)
                _zoomHeight = maxHeight;
        }
    }

    private void UpdateCameraPosition()
    {
        var localPosition = _cameraTransform.localPosition;
        Vector3 zoomTarget =
            new Vector3(localPosition.x, _zoomHeight, localPosition.z);
        zoomTarget -= zoomSpeed * (_zoomHeight - localPosition.y) * Vector3.forward;

        localPosition =
            Vector3.Lerp(localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        _cameraTransform.localPosition = localPosition;
        _cameraTransform.LookAt(this.transform);
    }
}
