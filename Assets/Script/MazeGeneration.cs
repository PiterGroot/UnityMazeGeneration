using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MazeGeneration : MonoBehaviour
{

    private List<Vector2> _neighbourPositions = new List<Vector2>()
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };
    
    private Dictionary<Vector2, GameObject> _unvisitedCells = new Dictionary<Vector2, GameObject>();
    private Dictionary<Vector2, GameObject> _allCells = new Dictionary<Vector2, GameObject>();
    private List<GameObject> _visited = new List<GameObject>();
    private Vector2 _currentCell;

    [SerializeField] private Vector2 size;
    [SerializeField] private GameObject baseCell;

    // Start is called before the first frame update
    private void Start()
    {
        GenerateMaze(size);
    }

    private void GenerateMaze(Vector2 mazeSize)
    {
        GameObject mazeParent = new GameObject("MazeParent");
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                GameObject cell = Instantiate(baseCell, new Vector2(x, y), Quaternion.identity, mazeParent.transform);
                //_unvisited.Add(cell);
                _unvisitedCells.Add(cell.transform.position, cell);
            }
        }

        _allCells = _unvisitedCells;
        CreateCentre();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextCell();
        }
    }

    private void CreateCentre()
    {
        Vector2 position = new Vector2(size.x / 2, size.y / 2);

        Destroy(_unvisitedCells[position]);
        _unvisitedCells.Remove(position);

        _currentCell = position;
    }

    private void NextCell()
    {
        //print("eeee");
        

        Vector2 direction = GetRandomDirection();
      
        if (_unvisitedCells.ContainsKey(_currentCell + direction))
        {
            Destroy(_allCells[_currentCell + direction]);
            _currentCell += direction;
            _unvisitedCells.Remove(_currentCell);
        }
        else NextCell();
    }

    private Vector2 GetRandomDirection()
    {
        return _neighbourPositions[Random.Range(0, _neighbourPositions.Count)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0.016f, .5f);
        Gizmos.DrawCube(_currentCell, Vector2.one);
    }
}

public class Cell
{
    public Vector2 position;
    public GameObject cell;
}