using Unity.Mathematics;
using UnityEngine;
using Gege;
public class Grid
{
    readonly float CellSize = 5f;
    readonly int Width;
    readonly int Height;
    string[,] Values;
    GameObject Parent;
    TextMesh[,] DebugText;
    BoxCollider[,] Colliders;
    private void Init()
    {
        Values = new string[Width, Height];
        DebugText = new TextMesh[Width, Height];
        Colliders = new BoxCollider[Width,Height];
        GameObject textMeshContainer = new("TextMeshContainer");
        GameObject colliderContainer = new("ColliderContainer");
        textMeshContainer.transform.SetParent(Parent.transform);
        colliderContainer.transform.SetParent(Parent.transform);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TextMesh debugText = Utility.DrawWorldText(x + "," + y, textMeshContainer, GetWorldPosition(x, y));
                DebugText[x, y] = debugText;

                GameObject collider = new("Collider", typeof(BoxCollider));
                collider.transform.SetParent(colliderContainer.transform);
                collider.transform.position = debugText.transform.position;
                Colliders[x, y] = collider.GetComponent<BoxCollider>();
                Colliders[x, y].size = new(CellSize, CellSize);

                SetValue(x, y, x + "," + y);
                
                Debug.DrawLine(GetWorldPosition(x, y) - new Vector3(CellSize/2,CellSize/2), GetWorldPosition(x, y+1) - new Vector3(CellSize / 2, CellSize/2), Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x, y) - new Vector3(CellSize / 2, CellSize/2), GetWorldPosition(x+1, y) - new Vector3(CellSize/2, CellSize / 2), Color.white,100f);
            }
        }
        
        Debug.DrawLine(GetWorldPosition(0,Height) - new Vector3(CellSize / 2, CellSize / 2), GetWorldPosition(Width,Height) - new Vector3(CellSize / 2, CellSize / 2), Color.white,100f);
        Debug.DrawLine(GetWorldPosition(Width,0) - new Vector3(CellSize / 2, CellSize / 2), GetWorldPosition(Width,Height) - new Vector3(CellSize / 2, CellSize / 2), Color.white,100f);
        
        GameObject.Find("KeyboardListener").GetComponent<KeyboardListener>().CameraRotated.AddListener(HandleRotation);
        GameObject.Find("KeyboardListener").GetComponent<KeyboardListener>().GridClick.AddListener(GridClick);
    }
    public Grid(int width, int height, float cellSize = 5f) 
    {
        CellSize = cellSize;
        Width = width;
        Height = height;
        Parent = new("Grid");
        Init();
    }
    public Grid(int width, int height, GameObject parent, float cellSize = 5f)
    {
        CellSize = cellSize;
        Width = width;
        Height = height;
        Parent = parent;
        Init();
    }
    private Vector3 GetWorldPosition(int x,int y)
    {
        Vector3 offset = new(-Width*CellSize/2 + CellSize / 2, -Height*CellSize/2 + CellSize/2);
        return (new Vector3(x,y) * CellSize) + offset;
    }
    public void HandleRotation()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                DebugText[x, y].transform.eulerAngles = new(0, 0, GameObject.Find("CamContainer").transform.eulerAngles.z);
            }
        }
    }
    public void SetValue(int x, int y, int value)
    {
        if (!(x >= 0 && y >= 0)) return;
        SetValue(x, y, value.ToString());
    }
    private void SetValue(int x, int y, string value)
    {
        if (!(x >= 0 && y >= 0)) return;
        Values[x, y] = value;
        DebugText[x, y].text = value;
    }
    public string GetValue(int x, int y)
    {
        return Values[x, y];
    }
    private int2 GetIndexPosition(Vector3 position)
    {
        Vector3 offset = new(-Width * CellSize / 2 + CellSize / 2, -Height * CellSize / 2 + CellSize / 2);
        Vector3 result = (new Vector3(position.x, position.y) - offset)/CellSize;
        return new((int)result.x, (int)result.y);
    }
    public void GridClick(Vector3 worldPosition)
    {
        Debug.Log(GetIndexPosition(worldPosition));
    }
}
