using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [SerializeField] private InputActionReference _mouseWheel;

    // Update is called once per frame
    void Update()
    {
        float angle = _mouseWheel.action.ReadValue<float>() * Time.deltaTime * 10.0f;

        transform.Rotate(Vector3.right * angle);
    }
}
