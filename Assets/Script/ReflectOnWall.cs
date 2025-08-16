using UnityEngine;

public class ReflectOnWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 velocity = rb.linearVelocity;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflected = Vector3.Reflect(velocity, normal);
            rb.linearVelocity = reflected;
        }
    }
}