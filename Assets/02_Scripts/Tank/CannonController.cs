using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [SerializeField] private InputActionReference _mouseWheel;

    private void Start()
    {
        this.enabled = transform.root.GetComponent<PhotonView>().IsMine;
    }

    void Update()
    {
        float angle = _mouseWheel.action.ReadValue<float>() * Time.deltaTime * 200.0f;

        transform.Rotate(Vector3.right * angle);
    }
}
