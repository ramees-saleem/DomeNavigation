using UnityEngine;

public class Interaction : MonoBehaviour
{
    private NavigationPoint currentPoint;
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NavigationPoint>(out var point))
        {
            point.SetColor(Color.green);
            currentPoint = point;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NavigationPoint>(out var point))
        {
            point.SetColor(Color.white);
            currentPoint = null;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentPoint != null)
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        Navigation.instance.LoadToDome(currentPoint.Coordinate);
    }
}
