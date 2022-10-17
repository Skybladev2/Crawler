using UnityEngine;
using System.Collections;

public class SpringController : MonoBehaviour
{
    public SpringJoint2D SpringJoint;
    private LineRenderer lineRenderer;
    private Vector2 startPoint = Vector2.zero;
    private Vector2 hitPoint = Vector2.zero;
    public Transform ColliderTransform;

    // Use this for initialization
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void EjectSpring(Vector2 hitPoint)
    {
        this.hitPoint = hitPoint;

        SpringJoint.connectedAnchor = hitPoint;
        SpringJoint.anchor = Vector2.zero;

        SetRendererStartPoint();
        SetRendererHitPoint(hitPoint);

        SpringJoint.enabled = true;
        lineRenderer.enabled = true;
    }

    private void SetRendererHitPoint(Vector2 hitPoint)
    {
        lineRenderer.SetPosition(0, hitPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.enabled)
        {
            SetRendererStartPoint();
            Vector2 fromBodyToWall = hitPoint - startPoint;

            ColliderTransform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(fromBodyToWall.y, fromBodyToWall.x));
            ColliderTransform.localScale = new Vector3(fromBodyToWall.magnitude, 1, 1);
            ColliderTransform.position = startPoint;
        }
    }

    private void SetRendererStartPoint()
    {
        startPoint = transform.TransformPoint(SpringJoint.anchor);
        lineRenderer.SetPosition(1, startPoint);
    }

    public void RetractSpring()
    {
        SpringJoint.enabled = false;
        lineRenderer.enabled = false;
    }
}
