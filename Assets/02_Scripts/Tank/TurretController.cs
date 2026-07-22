using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    [SerializeField] private InputActionReference _mousePos;

    private void Update()
    {
        var mousePos = _mousePos.action.ReadValue<Vector2>();
        // 마우스커서의 위치로 투사되는 광선을 생성
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));

        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
    }
}
