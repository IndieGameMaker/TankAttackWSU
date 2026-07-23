using Photon.Pun;
using System;
using TMPro.EditorUtilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions _inputAction;

    private float v, h;
    private PhotonView _pv;
    private Rigidbody _rb;
    private CinemachineCamera _camera;
    public Transform turret;

    private void Awake()
    {
        _inputAction = new InputSystem_Actions();
        _rb = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();
        _camera = GameObject.FindAnyObjectByType<CinemachineCamera>();
    }

    private void Start()
    {
        if (_pv.IsMine)
        {
            // Cinemachine Camera ¿¬°á
            _camera.Follow = transform;
            _camera.LookAt = transform;
            _camera.PreviousStateIsValid = false;
        }
        else
        {
            _rb.isKinematic = true;
        }
    }

    private void OnEnable()
    {
        if (!_pv.IsMine) return;

        _inputAction.Enable();
        _inputAction.Player.Move.performed += OnMove;
        _inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        if (!_pv.IsMine) return;

        _inputAction.Disable();
        _inputAction.Player.Move.performed -= OnMove;
        _inputAction.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            var move = ctx.ReadValue<Vector2>();
            v = move.y;
            h = move.x;
        }
        else
        {
            v = h = 0f;
        }
    }

    [SerializeField] private float speed = 10.0f;

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed * v);
        transform.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);
    }

    private void Update()
    {
        if (!_pv.IsMine) return;

        Move();
    }


}
