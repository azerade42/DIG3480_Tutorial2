using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Transform node1;
    public Transform node2;

    private Transform currentNode;

    public float swayTime;
    private float currentLerpTime = 0;

    private void Start()
    {
        currentNode = node1;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, currentNode.position) < 0.01f)
        {
            if (currentNode == node1) currentNode = node2;
            else if (currentNode == node2) currentNode = node1;

            currentLerpTime = 0;

        }

        currentLerpTime += Time.deltaTime;

        float percolator = currentLerpTime / swayTime;

        transform.position = Vector3.Lerp(transform.position, currentNode.position, percolator * percolator * (3f - (2f * percolator)));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(node1.position, .2f);
        Gizmos.DrawSphere(node2.position, .2f);
    }
}
