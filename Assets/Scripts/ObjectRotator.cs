using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}