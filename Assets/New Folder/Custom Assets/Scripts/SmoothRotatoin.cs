using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{

    public float minDelay = 0f; // Minimum delay between rotations
    public float maxDelay = 0f; // Maximum delay between rotations
    public float rotationSpeed = 180f; // Speed of rotation

    private Quaternion targetRotation;
    private bool isRotating = false;

    private void Start()
    {
        StartCoroutine(ChangeRotation());
    }

    private IEnumerator ChangeRotation()
    {
        while (true)
        {
            if (!isRotating)
            {
                // Generate a random delay between rotations
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);

                // Generate a random rotation
                targetRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

                // Start rotating towards the target rotation
                StartCoroutine(RotateTowardsTarget());
            }

            yield return null;
        }
    }

    private IEnumerator RotateTowardsTarget()
    {
        isRotating = true;

        while (transform.rotation != targetRotation)
        {
            // Rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        isRotating = false;
    }
}
