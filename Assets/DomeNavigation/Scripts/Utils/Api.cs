


using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RemoteCoordinateData
{
    public float latitude;
    public float longitude;
    public string image_path;
}
[System.Serializable]
public class ApiResponse
{
    public List<RemoteCoordinateData> message;
}

class Api
{
    private string json = @"
    [
        {
          ""latitude"": ""11.736253"",
          ""longitude"": ""75.498771"",
          ""image_path"": ""https://bookpropertyvisit.com/files/office-2.1.jpg""
        },
        {
          ""latitude"": ""11.736173"",
          ""longitude"": ""75.498765"",
          ""image_path"": ""https://bookpropertyvisit.com/files/office-2.2.jpg""
        },
        {
          ""latitude"": ""11.748166"",
          ""longitude"": ""75.486841"",
          ""image_path"": ""https://bookpropertyvisit.com/files/office-2.3.jpg""
        },
        {
          ""latitude"": ""11.748250"",
          ""longitude"": ""75.498750"",
          ""image_path"": ""https://bookpropertyvisit.com/files/office-2.4.jpg""
        }
    ]";

    private string apiUrl = "https://bookpropertyvisit.com/api/method/bookpropertyvisit.bookpropertyvisit.Items_360_images.find_360_images";

    private string authToken = "e738acd2413199e:52d43fb69e0e37c";

    private string itemCode = "P-00169";

    private string boundary = "-----011000010111000001101001";

    public List<RemoteCoordinateData> GetCoordinates()
    {
        List<RemoteCoordinateData> coordinateItems = JsonUtility.FromJson<ApiResponse>("{\"message\":" + json + "}").message;
        return coordinateItems;
    }

    IEnumerator FetchCoordinates()
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        string multipartData = $"{boundary}\r\n" +
                               $"Content-Disposition: form-data; name=\"item_code\"\r\n\r\n" +
                               $"{itemCode}\r\n" +
                               $"{boundary}--\r\n\r\n";

        byte[] formBytes = Encoding.UTF8.GetBytes(multipartData);

        request.uploadHandler = new UploadHandlerRaw(formBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", $"token {authToken}");
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error fetching coordinates: {request.result}");
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;

            Debug.Log("Response: " + jsonResponse);

            // TODO: Deserialize and process the response
        }
    }
}