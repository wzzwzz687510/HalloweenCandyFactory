using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyT.AStar;
using UnityEngine.Tilemaps;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance { get; private set; }

    public Tilemap map;
    private RoyT.AStar.Grid m_grid;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_grid = new RoyT.AStar.Grid(40, 25);
        for (int x = -20; x < 19; x++) {
            for (int y = -10; y < 14; y++) {
                if (map.HasTile(new Vector3Int(x, y, 0))) {
                    m_grid.BlockCell(new Position(x + 20, y + 10));
                    //Debug.Log("(" + x + "," + y + ") blocked");
                }
                    
            }
        }
    }

    public Vector3[] CalculatePath(Vector3 startPoint, Vector3 endPoint)
    {
        Position[] path = m_grid.GetPath(new Position((int)(startPoint.x - 0.5f) + 20, (int)(startPoint.y - 0.5f) + 10), new Position((int)(endPoint.x - 0.5f) + 20, (int)(endPoint.y - 0.5f) + 10));
        Vector3[] positions = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++) {
            positions[i] = new Vector3(path[i].X - 19.5f, path[i].Y - 9.5f, 0);
        }
        return positions;
    }
}
