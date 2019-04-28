using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierLineRenderer : MonoBehaviour
{
    public static BezierLineRenderer instance;

    public bool drawGizmos = false;
    public LineRenderer lineRenderer;
    public Transform point0, point1, point2, point3;

    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];

    public Vector3 tipAttachPoint;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = numPoints;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawSphere(point0.position, 0.1f);
            Gizmos.DrawSphere(point1.position, 0.2f);
            Gizmos.DrawSphere(point2.position, 0.3f);
            Gizmos.DrawSphere(point3.position, 0.4f);
        }
    }

    private void Update()
    {
        DrawCubicCurve();
    }

    private void DrawLinearCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateLinerBezierPoint(t, point0.position, point1.position);
        }

        lineRenderer.SetPositions(positions);
    }

    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.position, point1.position, point2.position);
        }

        lineRenderer.SetPositions(positions);
    }


    private void DrawCubicCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCubicBezierPoint(t, point0.position, point1.position, point2.position, point3.position);
        }

        lineRenderer.SetPositions(positions);

        tipAttachPoint = positions[2];
    }

    private Vector3 CalculateLinerBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // (1 - t)2 P0 + 2(1-t)tP1 + t2P2
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //  (1 - t)3 P0 + 3(1 - t)2 tP1 + 3(1 - t) t2 P2 + t3 P3
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
