using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 targetLookOffset;
    public Vector2 distanceFromTargetZY = new Vector2(50, 10);
    public float rotateSpeed = 1;

    private Vector2 lookAtPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position - Vector3.forward * distanceFromTargetZY.x + Vector3.up * distanceFromTargetZY.y;
    }

    // Update is called once per frame
    void Update()
    {
        lookAtPos = target.position + targetLookOffset;
        transform.LookAt(lookAtPos);

        transform.RotateAround(target.position, Vector3.up, -rotateSpeed * Time.deltaTime);
    }
}
