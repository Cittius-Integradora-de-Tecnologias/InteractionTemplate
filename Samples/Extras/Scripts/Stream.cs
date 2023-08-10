using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer = null;

    private Coroutine pourRoutine = null;
    private Vector3 targetPosition = Vector3.zero;
    private Transform origin;
    private GameObject endObject;

    [SerializeField] private float rangeLiquid = 0.5f;

    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();

        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void Begin(Transform origin)
    {
        this.origin = origin;
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour()
    {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, origin.position);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }
    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    private IEnumerator EndPour()
    {
        while (!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, origin.position);
            AnimateToPosition(1, origin.position);
            gameObject.SetActive(false);

            yield return null;
        }
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(origin.position, Vector3.down);

        Physics.Raycast(ray, out hit, rangeLiquid);
        Vector3 endPoint = Vector3.zero;

        if(hit.collider)
        {
            endPoint = hit.point;
            if(endObject != hit.transform.gameObject)
            {
                endObject = hit.transform.gameObject;
                if (endObject.TryGetComponent(out LiquidControl control))
                {
                    control.StartFill(30, 300);
                }
            }
        }
        else
        {
            rangeLiquid++;
            endPoint = ray.GetPoint(rangeLiquid);
            if(endObject != null)
            {
                if (endObject.TryGetComponent(out LiquidControl control))
                {
                    control.StopFill();
                }
            }
        }

        return endPoint;
    }

    private void MoveToPosition(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }

    private void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }

    private bool HasReachedPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        return currentPosition == targetPosition;
    }
}
