using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidHandler : MonoBehaviour
{
    public float Lookradius;
    void Update()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, Lookradius);
        Vector3 center = Vector3.zero;
        foreach (Collider col in Cols)
        {
            center += col.transform.position;
        }
        center /= Cols.Length;
        transform.forward = ((center - transform.position)+transform.forward).normalized;
        transform.position += transform.forward * Time.deltaTime;
    }
}