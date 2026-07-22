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

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, 1 << 8))
        {
            // 레이캐스팅으로 추출한 좌료(월드좌표)를 터렛 기준의 로컬좌표로 변환
            Vector3 pos = transform.InverseTransformPoint(hit.point);

            // 사잇각도 구하는 로직 Atan2
            float angle = Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg;

            transform.Rotate(Vector3.up * Time.deltaTime * 10.0f * angle);
        }
    }
}
