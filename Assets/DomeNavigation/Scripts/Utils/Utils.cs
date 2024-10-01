using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

static class Utils
{

    public static Vector3 CoordinateToPosition(Vector2 coordinate)
    {

        var latitude = coordinate.x;
        var longitude = coordinate.y;

        var radius = 4000;

        float latRad = Mathf.Deg2Rad * latitude;
        float lonRad = Mathf.Deg2Rad * longitude;

        // float y = radius * Mathf.Sin(latRad);

        float x = radius * Mathf.Cos(latRad) * Mathf.Cos(lonRad); 
        float z = radius * Mathf.Cos(latRad) * Mathf.Sin(lonRad); 

        return new Vector3(x, 0, z);
    }
    
    public static async Task<Texture2D> DownloadAndApplyEquirectangularTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Awaitable.EndOfFrameAsync();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to download equirectangular texture: {request.error}");
            return null;
        }

        Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request) as Texture2D;

        if (downloadedTexture == null)
        {
            Debug.LogError("Downloaded texture is not valid or is not a Texture2D.");
            return null;
        }

        return downloadedTexture;
    }
}