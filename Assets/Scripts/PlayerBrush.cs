using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBrush : NetworkBehaviour {
    public Color brushColor;
    public const int BRUSH_SIZE = 8;

    [Server]
    private void Start()
    {
        var data = PaintCanvas.GetAllTextureData();
        var zippeddata = data.Compress();

        RpcSendFullTexture(zippeddata);
    }

    [ClientRpc]
    private void RpcSendFullTexture(byte[] textureData)
    {
        PaintCanvas.SetAllTextureData(textureData.Decompress());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var pallet = hit.collider.GetComponent<PaintCanvas>();
                if (pallet != null)
                {
                    Debug.Log(hit.textureCoord);
                    Debug.Log(hit.point);

                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (rend == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                    {
                        return;
                    }

                    Texture2D tex = rend.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;

                    CmdBrushAreaWithColorOnServer(pixelUV, brushColor);
                    BrushAreaWithColor(pixelUV, brushColor);
                }
            }
        }
    }

    [Command]
    private void CmdBrushAreaWithColorOnServer(Vector2 pixelUV, Color color)
    {
        RpcBrushAreaWithColorOnClients(pixelUV, color);
        BrushAreaWithColor(pixelUV, color);
    }

    [ClientRpc]
    private void RpcBrushAreaWithColorOnClients(Vector2 pixelUV, Color color)
    {
        BrushAreaWithColor(pixelUV, color);
    }

    private void BrushAreaWithColor(Vector2 pixelUV, Color color)
    {
        for(int x = -BRUSH_SIZE; x < BRUSH_SIZE; x++)
        {
            for(int y = -BRUSH_SIZE; y < BRUSH_SIZE; y++)
            {
                PaintCanvas.Texture.SetPixel((int)pixelUV.x + x, (int)pixelUV.y + y, color);
            }
        }

        PaintCanvas.Texture.Apply();
    }
}
