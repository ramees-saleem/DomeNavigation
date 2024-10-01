using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

class Dome : MonoBehaviour
{

    [SerializeField]
    private new Renderer renderer;

    public void ApplyEquirectangularTextureToSkybox(Texture2D equirectangularTexture)
    {
        if (renderer != null && equirectangularTexture != null)
        {
            var property = new MaterialPropertyBlock();

            property.SetTexture("_BaseMap", equirectangularTexture);

            renderer.SetPropertyBlock(property);
        }
        else
        {
            Debug.LogWarning("Skybox Material or Equirectangular Texture is missing!");
        }
    }
}