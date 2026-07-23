using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro.EditorUtilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/*
RPC (Remote Procedure Call) == RMI (Remote Method Invoke) 
 
 
 
*/



public class TankController : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions _inputAction;
    [SerializeField] private Transform _firePos;

    private float v, h;
    private PhotonView _pv;
    private Rigidbody _rb;
    private CinemachineCamera _camera;
    private GameObject _cannonPrefab;

    private MeshRenderer[] _renderers;

    //[SerializeField] private InputActionReference _attackAction;

    private void Awake()
    {
        _inputAction = new InputSystem_Actions();
        _rb = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();
        _camera = GameObject.FindAnyObjectByType<CinemachineCamera>();

        _cannonPrefab = Resources.Load<GameObject>("Cannon");
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }


    private void Start()
    {
        if (_pv.IsMine)
        {
            // Cinemachine Camera 연결
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
        _inputAction.Player.Attack.started += OnFire;
    }

    private void OnDisable()
    {
        if (!_pv.IsMine) return;

        _inputAction.Disable();
        _inputAction.Player.Move.performed -= OnMove;
        _inputAction.Player.Move.canceled -= OnMove;
        _inputAction.Player.Attack.started -= OnFire;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        // RPC 호출
        _pv.RPC(nameof(Fire), RpcTarget.AllViaServer, _pv.Owner.ActorNumber);
    }

    [PunRPC]
    private void Fire(int actorNumber)
    {
        var obj = Instantiate(_cannonPrefab, _firePos.position, _firePos.rotation);

        obj.GetComponent<Cannon>().shooterId = actorNumber; 

        Destroy(obj, 10.0f);
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

        /*
        // Legacy InputManager
        if (Input.GetMouseButtonDown(0))
        {
            // FireCannon();
        }
        // New InputSystem
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // FireCannon();
        }
        */


        Move();
    }

    private float _currHp = 100.0f;
    private float _maxHp = 100.0f;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("CANNON"))
        {
            _currHp -= 20.0f;

            // ActorNumber(ShooterID) > NickName
            int shooterId = coll.gameObject.GetComponent<Cannon>().shooterId;
            Player shooter = PhotonNetwork.CurrentRoom.GetPlayer(shooterId);

            if (_currHp <= 0.0f)
            {
                string msg = $"<color=green>[{_pv.Owner.NickName}]</color>님은 <color=red>[{shooter.NickName}]</color>에게 살해당했습니다.";
                Debug.Log(msg);
                
                TankDestroy();
            }
        }    
    }

    private void TankDestroy()
    {
        // Tank Invisible
        SetVisible(false);

        
        Vector3 newPos = new Vector3(Random.Range(-100, 100), 5.0f, Random.Range(-100, 100));
        transform.position = newPos;

        Invoke(nameof(RespawnTank), 3.0f);
    }

    private void RespawnTank()
    {
        _currHp = _maxHp;

        SetVisible(true);
    }

    private void SetVisible(bool isVisible)
    {
        for (int i=0; i<_renderers.Length; i++)
        {
            _renderers[i].enabled = isVisible;
        }
    }
}
