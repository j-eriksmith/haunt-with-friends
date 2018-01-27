using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintCanvas : MonoBehaviour {
    public static Texture2D Texture
    {
        get;
        private set;
    }

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
