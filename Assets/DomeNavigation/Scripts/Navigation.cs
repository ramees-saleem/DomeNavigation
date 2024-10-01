using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


[Serializable]
public class Coordinate
{

    public Texture2D texture;

    public Vector2 coordinate;

    public Vector3 position { get; private set; }
    public int id { get; private set; }

    public void Initialize()
    {
        id = GetHashCode();
        position = Utils.CoordinateToPosition(coordinate);

        Debug.Log("ID: " + id);
    }
}


public class Navigation : MonoBehaviour
{
    public static Navigation instance;
    [SerializeField] List<Coordinate> coordinates;
    [SerializeField] NavigationPoint pointPrefab;
    [SerializeField] Dome dome;

    private List<NavigationPoint> currentPoints = new List<NavigationPoint>();

    new Camera camera;

    void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        camera = Camera.main;

        var api = new Api();

        Init(api.GetCoordinates());
    }

    public async void Init(List<RemoteCoordinateData> items)
    {
        coordinates.Clear();
        foreach (var item in items)
        {
            Texture2D texture = await Utils.DownloadAndApplyEquirectangularTexture(item.image_path);

            var coordinate = new Coordinate
            {
                coordinate = new Vector2(item.latitude, item.longitude),
                texture = texture 
            };
            coordinate.Initialize();
            coordinates.Add(coordinate);

        }

        LoadToDome(coordinates.First());
    }


    public void LoadToDome(Coordinate coordinate)
    {
        dome.ApplyEquirectangularTextureToSkybox(coordinate.texture);

        foreach (var point in currentPoints)
        {
            Destroy(point.gameObject);
        }

        currentPoints.Clear();

        foreach (var c in coordinates)
        {
            if (c.id != coordinate.id)
            {
                var point = Instantiate(pointPrefab);

                var direction = coordinate.position - c.position;

                point.transform.position = direction.normalized * 15;

                point.Initialize(c);

                currentPoints.Add(point);
            }
        }

    }
}
