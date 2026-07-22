using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions _inputAction;

    private float v, h;

    private void Awake()
    {
        _inputAction = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputAction.Enable();
        _inputAction.Player.Move.performed += OnMove;
        _inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
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
        Move();
    }


}
