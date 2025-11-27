using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Mouse ScrollWheel");

        rb.linearVelocity = new Vector3(h * 10f, z * 100f, v * 10f);
    }
}
