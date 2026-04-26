using UnityEngine;

public class AutoMoveAvoid : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 120f;
    public float detectionDistance = 2f;

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Raycast to detect obstacle ahead
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            // If obstacle detected, rotate randomly
            float randomTurn = Random.Range(-1f, 1f);
            transform.Rotate(Vector3.up * randomTurn * rotationSpeed * Time.deltaTime);
        }
    }

    // Optional: visualize ray in editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
    }
}