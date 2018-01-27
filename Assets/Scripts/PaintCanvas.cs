/*
 * Code modified from https://unity3d.college/2017/07/22/build-unity-multiplayer-drawing-game-using-unet-unity3d/
 * Originally written by Jason Weimann
 */

using UnityEngine;

public class PaintCanvas : MonoBehaviour
{
    public static Texture2D Texture { get; private set; }

    public static byte[] GetAllTextureData()
    {
        return Texture.GetRawTextureData();
    }

    private void Start()
    {
        PrepareTemporaryTexture();
    }

    private void PrepareTemporaryTexture()
    {
        Texture = (Texture2D)GameObject.Instantiate(GetComponent<Renderer>().material.mainTexture);
        GetComponent<Renderer>().material.mainTexture = Texture;
    }

    internal static void SetAllTextureData(byte[] textureData)
    {
        Texture.LoadRawTextureData(textureData);
        Texture.Apply();
    }
}