using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphTextureRenderer : MonoBehaviour
{
    public RecordedTrailData trailData;       // ScriptableObject con la lista de posiciones Y
    public RawImage graphImage;               // RawImage en el Content del Scroll View
    public float pointSpacing = 2f;           // Espacio horizontal entre puntos
    public int textureHeight = 500;           // Altura en píxeles de la textura
    public Color graphColor = Color.green;    // Color del gráfico

    void Start()
    {
        GenerateGraphTexture();
    }

    void GenerateGraphTexture()
    {
        List<float> yPositions = trailData.recordedYPositions;

        int width = Mathf.CeilToInt(yPositions.Count * pointSpacing);
        Texture2D texture = new Texture2D(width, textureHeight, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;

        // Fondo transparente
        Color32[] pixels = new Color32[width * textureHeight];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.clear;
        texture.SetPixels32(pixels);

        float minY = Mathf.Min(yPositions.ToArray());
        float maxY = Mathf.Max(yPositions.ToArray());

        // Dibujar puntos
        for (int i = 0; i < yPositions.Count; i++)
        {
            int x = Mathf.RoundToInt(i * pointSpacing);

            // Normalizamos Y para que encaje en la textura
            float normalizedY = Mathf.InverseLerp(minY, maxY, yPositions[i]);
            int y = Mathf.RoundToInt(normalizedY * (textureHeight - 1));

            texture.SetPixel(x, y, graphColor);

            // (opcional) grosor de línea vertical
            if (y + 1 < textureHeight) texture.SetPixel(x, y + 1, graphColor);
            if (y - 1 >= 0) texture.SetPixel(x, y - 1, graphColor);
        }

        texture.Apply();

        // Asignamos al RawImage
        graphImage.texture = texture;

        // Ajustamos tamaño del RawImage y Content
        RectTransform rt = graphImage.rectTransform;
        rt.sizeDelta = new Vector2(width, textureHeight);
    }
}
