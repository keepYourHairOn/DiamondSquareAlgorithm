using UnityEngine;
using System.Collections;

public class DiamondSquareGenerator : MonoBehaviour {

    // Width of the terrain.
    protected int width = 128;
    // Height of the terrain.
    protected int height = 128;
    protected Color32[] colors;
    protected float[] heightMap;
    protected float widthHeight;
    protected int GRAIN = 6;
    protected Texture2D texture;
    protected int planetValue;
    protected float avgHeight;


    /// <summary>
    /// Method for unity object initialization.
    /// </summary>
    public void Start()
    {

        widthHeight = (float)width + height;
        heightMap = new float[width * height];

        texture = new Texture2D(width, height);
        GetComponent<Renderer>().material.mainTexture = texture;

        colors = new Color32[width * height];

        Draw(width, height);

        System.IO.StreamWriter file = new System.IO.StreamWriter("D:\\Education\\4course\\PCG\\speed.txt");
        file.WriteLine("Speed of Diamond-square algorithm: " + (Time.realtimeSinceStartup * 1000));

        file.Close();

        texture.SetPixels32(colors);
        texture.Apply();
    }

    /// <summary>
    /// Update object on mouse click.
    /// </summary>
    public void Update()
    {

        // Click mouse button to generate new.
        if (Input.GetMouseButton(0))
        {
            Draw(width, height);
            texture.SetPixels32(colors);
            texture.Apply();
        }

    }

    /// <summary>
    /// Randomply displace the point.
    /// </summary>
    /// <param name="num">The point to displace.</param>
    /// <returns></returns>
    public float Displace(float num)
    {
        float max = num / widthHeight * GRAIN;
        return Random.Range(-0.5f, 0.5f) * max;
    }

    /// <summary>
    /// Calculates average height of the heightmap.
    /// </summary>
    /// <returns>Value of the average height of the map</returns>
    public float CalculateAverageHeight()
    {
        float totalHeight = 0;

        for (int i = 0; i < width * height; i++)
        {

            totalHeight += heightMap[i];

        }

        return (totalHeight / (height * width));
    }

    /// <summary>
    /// Assign a color to cocrete point on the base of it's height.
    /// Color sheme is used on the base of the planet type.
    /// </summary>
    /// <param name="pointHeight">Height of the terrain in the concrete point.</param>
    /// <returns>A color based on the height's value.</returns>
    public Color GetColor(float pointHeight)
    {
        if (planetValue == 1)
        {
            // WaterGrass coloring scheme.

            if (pointHeight < avgHeight)
            {
                return Color.blue;
            }
            else
            {
                return Color.green;
            }
        }
        else if (planetValue == 2)
        {
            // LavaAsh coloring scheme.

            if (pointHeight < avgHeight)
            {
                return new Color(0.647f, 0.1647f, 0.1647f);
            }
            else
            {
                return new Color(0.2F, 0.3F, 0.4F);
            }

        }
        else
        {
            // IceSnow coloring scheme.

            if (pointHeight < 0.5f)
            {
                return Color.gray;
            }
            else
            {
                return Color.white;
            }

        }
    }

    //This is the recursive function that implements the random midpoint
    //displacement algorithm.  It will call itself until the grid pieces
    //become smaller than one pixel.
    /// <summary>
    /// Build a heightmap for the terrain by dividing it on squares
    /// on the base of Dimond-Square algorithm.
    /// </summary>
    /// <param name="x">Coordinate  on the x-axis of the terrain's point.</param>
    /// <param name="y">Coordinate on y-axis of the terrain's point.</param>
    /// <param name="width">Width of the current square.</param>
    /// <param name="height">Height of the current square.</param>
    /// <param name="point1">Height of terrain in left-top point of the square.</param>
    /// <param name="point2">Height of terrain in right-top point of the square.</param>
    /// <param name="point3">Height of terrain in right-bottom point of the square.</param>
    /// <param name="point4">Height of terrain in left-bottom point of the square.</param>

    public void DimondSquare(float x, float y, float width, float height, float point1, float point2, float point3, float point4)
    {

        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        // Average height of the square's corners.
        float averageHeight = (point1 + point2 + point3 + point4) * 0.25f;

        // If the square is less than 1px, then color it.
        if (width < 1 || height < 1)
        {
            // Add average height of the square's corners to the heightmap of the terrain.
            heightMap[(int)x + (int)y * this.width] = averageHeight;
        }
        else
        {
            // Divide square on four parts.

            // Calculate midle point of the diamond.
            float midPoint = averageHeight + Displace(halfWidth + halfHeight);

            // Calculate points of the diamond.
            float a = (point1 + point2) * 0.5f;
            float b = (point2 + point3) * 0.5f;
            float c = (point3 + point4) * 0.5f;
            float d = (point4 + point1) * 0.5f;

            // Check wheaser midpoint is not out of range.
            if (midPoint <= 0)
            {
                midPoint = 0;
            }
            else if (midPoint > 1.0f)
            {
                midPoint = 1.0f;
            }

            // Divide each of four obtained squares.                 
            DimondSquare(x, y, halfWidth, halfHeight, point1, a, midPoint, d);
            DimondSquare(x + halfWidth, y, halfWidth, halfHeight, a, point2, b, midPoint);
            DimondSquare(x + halfWidth, y + halfHeight, halfWidth, halfHeight, midPoint, b, point3, c);
            DimondSquare(x, y + halfHeight, halfWidth, halfHeight, d, midPoint, c, point4);
        }
    }

    /// <summary>
    /// Forms a colors map of the terrain.
    /// </summary>
    /// <param name="w">Width of the terrain.</param>
    /// <param name="h">Height of the terrain.</param>
    public void Draw(float w, float h)
    {
        float point1, point2, point3, point4;

        // Assign the four corners of the intial grid random color values.    
        point1 = Random.value;
        point2 = Random.value;
        point3 = Random.value;
        point4 = Random.value;
        planetValue = Random.Range(1, 4);
        DimondSquare(0.0f, 0.0f, w, h, point1, point2, point3, point4);
        avgHeight = CalculateAverageHeight();
        for (int i = 0; i < heightMap.Length; i++)
        {
            colors[i] = GetColor(heightMap[i]);
        }
    }

}
