/*
 * Code modified from https://unity3d.college/2017/07/22/build-unity-multiplayer-drawing-game-using-unet-unity3d/
 * Originally written by Jason Weimann
 */

using UnityEngine;
using UnityEngine.Networking;

public class PlayerBrush : NetworkBehaviour
{
    public Color brushColor = Color.cyan;
    public int brushSize = 3;
    private Vector2 prevCoords;
    private bool prevMouse = false;
    private int res = 100;

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
        //print(prevMouse);
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var pallet = hit.collider.GetComponent<PaintCanvas>();
                if (pallet != null)
                {
                    //Debug.Log(hit.textureCoord);
                    //Debug.Log(hit.point);

                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                        return;

                    Texture2D tex = rend.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;
                    if (!prevMouse)
                    {
                        prevCoords = pixelUV;
                    }
                    //print(prevCoords);
                    //print(pixelUV);
                    CmdBrushAreaWithColorOnServer(prevCoords, pixelUV, brushColor, brushSize);
                    BrushAreaWithColor(prevCoords, pixelUV, brushColor, brushSize);
                    prevCoords = pixelUV;
                    prevMouse = true;
                }
            }
        }
        else
        {
            prevMouse = false;
        }
    }

    [Command]
    private void CmdBrushAreaWithColorOnServer(Vector2 start, Vector2 end, Color color, int size)
    {
        RpcBrushAreaWithColorOnClients(start, end, color, size);
        BrushAreaWithColor(start, end, color, size);
    }

    [ClientRpc]
    private void RpcBrushAreaWithColorOnClients(Vector2 start, Vector2 end, Color color, int size)
    {
        BrushAreaWithColor(start, end, color, size);
    }

    private void BrushAreaWithColor(Vector2 start, Vector2 end, Color color, int size)
    {
        for (int t = 0; t < res; t++)
        {
            //print(t);
            Vector2 pixelUV = Vector2.Lerp(start, end, (float)t/res);
            //print(pixelUV);
            for (int x = -size; x < size; x++)
            {
                for (int y = -size; y < size; y++)
                {
                    PaintCanvas.Texture.SetPixel((int)pixelUV.x + x, (int)pixelUV.y + y, color);
                }
            }
        }
        PaintCanvas.Texture.Apply();
    }
}