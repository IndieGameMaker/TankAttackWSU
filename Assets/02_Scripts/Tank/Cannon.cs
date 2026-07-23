using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _expEffect;
    [SerializeField] private float _speed = 2000.0f;

    private void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * _speed);
    }

    private void OnCollisionEnter(Collision coll)
    {
        var obj = Instantiate(_expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 5.0f);

        // Cannon Destory
        Destroy(this.gameObject);
    }
}
