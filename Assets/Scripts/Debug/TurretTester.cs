using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTester : MonoBehaviour
{
    public Transform target;
    private Vector3 rightVector;    // from player towards right of turret
    private Vector3 alpha; // orthogonal vector
    public float sParam;
    public float tParam;
    public float radius;
    public int subdivisions;
    public float degrees;

    // Start is called before the first frame update
    void Start()
    {
        alpha = target.position - this.transform.position;
        rightVector = Vector3.Cross(Vector3.up, alpha ).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
        alpha = target.position - this.transform.position;  // orthogonal vector
        this.transform.forward = alpha.normalized;          // point the turret to the player

        rightVector = Vector3.Cross(Vector3.up, alpha ).normalized;     // get one vector to scale parametrically

        Vector3 planeVector = GeneratePointOnPlane(sParam, tParam);


        //DrawRingOnPlane(radius, subdivisions);
        if(Input.GetKeyDown(KeyCode.P))
        {
            DrawRingOnPlane(radius, subdivisions);
        }

        //Vector3 testingPoint = GeneratePointOnPlane(Mathf.Sin(degrees * Mathf.Deg2Rad), Mathf.Cos(degrees * Mathf.Deg2Rad));
        //Debug.DrawRay(testingPoint, alpha, Color.yellow, 5f);

        //Debug.Log(Mathf.Sin(90 * Mathf.Deg2Rad) * Mathf.Rad2Deg);
        //Debug.Log(Mathf.Sin(90 * Mathf.Deg2Rad));


        Debug.DrawRay(planeVector, alpha.normalized, Color.black, 0.01f);

        Debug.DrawRay(this.transform.position, alpha, Color.cyan, 0.01f);
        Debug.DrawRay(this.transform.position, this.transform.up, Color.blue, 0.01f);
        //Debug.DrawRay(target.position, s, Color.magenta, 0.01f);

        Debug.DrawRay(target.position, this.transform.up, Color.red, 0.01f);
        Debug.DrawRay(target.position, rightVector, Color.green, 0.01f);
    }

    public void DrawRingOnPlane(float radius, int subdivisions)
    {
        float theta = 360f / subdivisions;
        Vector3[] points = new Vector3[subdivisions];

        for(int i = 0; i < subdivisions; i++)
        {
            points[i] = GeneratePointOnPlane(radius * Mathf.Sin(theta * i * Mathf.Deg2Rad), radius * Mathf.Cos(theta * i * Mathf.Deg2Rad) );
            Debug.DrawRay(points[i], alpha, Color.yellow, 5f);
        }

        //Vector3 testingPoint = GeneratePointOnPlane(Mathf.Sin(degrees * Mathf.Deg2Rad), Mathf.Cos(degrees * Mathf.Deg2Rad));
    }

    // private Vector3 GetOrthogonal()
    // {

    // }

    public Vector3 GeneratePointOnPlane(float s, float t)   // up then over
    {
        return target.transform.position + (s * this.transform.up) + (t * rightVector);
    }
}
