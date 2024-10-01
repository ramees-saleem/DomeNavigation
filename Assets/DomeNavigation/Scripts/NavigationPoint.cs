using UnityEngine;

public class NavigationPoint : MonoBehaviour
{
    [SerializeField] new Renderer renderer;

    public Coordinate Coordinate { get; private set; }


    public void Initialize(Coordinate coordinate)
    {
        Coordinate = coordinate;
    }

    public void SetColor(Color color)
    {
        var property = new MaterialPropertyBlock();
        property.SetColor("_BaseColor", color);
        renderer.SetPropertyBlock(property);
    }

    public void Trigger()
    {
        Navigation.instance.LoadToDome(Coordinate);
    }
}
