using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTester : MonoBehaviour
{
    public Transform target;
    public Vector3 firstVector;
    public Vector3 rightVector;
    private Vector3 alpha; // orthogonal vector
    public float sParam;
    public float tParam;

    // Start is called before the first frame update
    void Start()
    {
        alpha = target.position - this.transform.position;

        firstVector = new Vector3(0 , 1, 0).normalized;
        rightVector = Vector3.Cross(firstVector, alpha ).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
        alpha = target.position - this.transform.position;  // orthogonal vector
        this.transform.forward = alpha.normalized;

        Vector3 s = target.rotation * new Vector3(0, 1, 0);

        //firstVector = target.rotation * firstVector;
        rightVector = Vector3.Cross(firstVector, alpha ).normalized;

        Vector3 planeVector = GeneratePointOnPlane(sParam, tParam);

        Debug.DrawRay(planeVector, alpha.normalized, Color.black, 0.01f);

        Debug.DrawRay(this.transform.position, alpha, Color.cyan, 0.01f);
        Debug.DrawRay(this.transform.position, this.transform.up, Color.blue, 0.01f);
        //Debug.DrawRay(target.position, s, Color.magenta, 0.01f);

        Debug.DrawRay(target.position, this.transform.up, Color.red, 0.01f);
        Debug.DrawRay(target.position, rightVector, Color.green, 0.01f);
    }

    private float ImplicitD(Vector3 ortho, Vector3 point)
    {
        // gets the d value in the implicit formula of a plane
        // takes parameters of an orthogonal vector and a point on the plane

        return (-ortho.x * point.x - ortho.y * point.y - ortho.z * point.z);
    }

    // private Vector3 GetOrthogonal()
    // {

    // }

    public Vector3 GeneratePointOnPlane(float s, float t)
    {
        return target.transform.position + (s * this.transform.up) + (t * rightVector);
    }
}
