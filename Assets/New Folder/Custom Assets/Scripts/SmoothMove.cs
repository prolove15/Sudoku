using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    public float moveRangeMax = 30f, moveRangeMin = 1f; // Maximum range of movement
    public float moveSpeed = 8f; // Speed of movement

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        GenerateRandomTargetPosition();
    }

    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // If the target position is reached, generate a new random target position
        if (transform.position == targetPosition)
        {
            GenerateRandomTargetPosition();
        }
    }

    void GenerateRandomTargetPosition()
    {
        // Generate a random position within the move range
        targetPosition = initialPosition + new Vector3(Random.Range(moveRangeMin, moveRangeMax),
            Random.Range(moveRangeMin, moveRangeMax), Random.Range(moveRangeMin, moveRangeMax));
    }
}
