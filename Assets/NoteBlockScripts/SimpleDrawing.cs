using UnityEngine;

public class SimpleDrawing : MonoBehaviour
{
    public Camera drawingCamera;
    public Texture2D drawingTexture;
    public Color drawColor = Color.black;
    public int brushSize = 5;
    private Vector2 previousDrawPoint;
    private bool isDrawing = false;

    private void Start()
    {
        drawingTexture = new Texture2D(1024, 1024);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(drawingTexture, new Rect(0, 0, 1024, 1024), new Vector2(0.5f, 0.5f));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousDrawPoint = GetTextureCoordinates(Input.mousePosition);
            isDrawing = true;
            SetPixel(previousDrawPoint, drawColor); 
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 drawPoint = GetTextureCoordinates(Input.mousePosition);
            Draw(previousDrawPoint, drawPoint);
            previousDrawPoint = drawPoint;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    private Vector2 GetTextureCoordinates(Vector2 screenPoint)
    {
        Vector2 worldPoint = drawingCamera.ScreenToWorldPoint(screenPoint);
        Renderer renderer = GetComponent<Renderer>();
        Vector2 texturePoint = worldPoint - (Vector2)renderer.bounds.min;
        texturePoint.x *= drawingTexture.width / renderer.bounds.size.x;
        texturePoint.y *= drawingTexture.height / renderer.bounds.size.y;
        return texturePoint;
    }

    private void Draw(Vector2 from, Vector2 to)
    {
        DrawLine((int)from.x, (int)from.y, (int)to.x, (int)to.y, drawColor);
        drawingTexture.Apply();
    }

    private void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        while (true)
        {
            SetPixel(new Vector2(x0, y0), color);
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }

    private void SetPixel(Vector2 point, Color color)
    {
        for (int x = -brushSize; x <= brushSize; x++)
        {
            for (int y = -brushSize; y <= brushSize; y++)
            {
                if (x * x + y * y <= brushSize * brushSize)
                {
                    int px = (int)point.x + x;
                    int py = (int)point.y + y;
                    if (px >= 0 && px < drawingTexture.width && py >= 0 && py < drawingTexture.height)
                    {
                        drawingTexture.SetPixel(px, py, color);
                    }
                }
            }
        }
    }
}