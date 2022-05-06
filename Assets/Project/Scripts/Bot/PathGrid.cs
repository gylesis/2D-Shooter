using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Project.Bot
{
    public class PathGrid : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;

        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private float _cellSize = 3;
        [SerializeField] private Cell _cell;

        [SerializeField] private BoxCollider2D[] _obstacles;
        
        private List<CellData> _cells = new List<CellData>();

        private readonly Dictionary<CellData, List<CellData>> _neighbours = new Dictionary<CellData, List<CellData>>();
        private GridCellsConditionChecker _gridCellsConditionChecker;

        public void Init(GridCellsConditionChecker gridCellsConditionChecker)
        {
            _gridCellsConditionChecker = gridCellsConditionChecker;
            Vector3 startPos = _pivot.position;

            int gridSizeX = _gridSize.x;
            int gridSizeY = _gridSize.y;
            float cellSize = _cellSize;

            int id = 0;
            
            foreach (BoxCollider2D collider in _obstacles) 
                collider.size = new Vector2(1.5f, 1.3f);

            for (int width = 0; width < gridSizeX; width++)
            {
                for (int height = 0; height < gridSizeY; height++)
                {
                    Vector3 spawnPos = startPos;

                    spawnPos.x += (width * cellSize);
                    spawnPos.y += (height * cellSize);

                    Cell cell = Instantiate(_cell, transform);
                    cell.transform.position = spawnPos;

                    var isWalkable = _gridCellsConditionChecker.IsWalkable(cell);
                    var isOutOfBounds = _gridCellsConditionChecker.IsOutOfBounds(cell);

                    if (isOutOfBounds)
                    {
                        Destroy(cell.gameObject);
                        id++;
                        continue;
                    }

                    if (isWalkable)
                    {
                        var cellData = new CellData();
                        cellData.Cell = cell;
                        cellData.Id = id;

                        cell.Init(cellData);

                        _cells.Add(cellData);
                    }
                    else
                        Destroy(cell.gameObject);

                    id++;
                }
            }
            
            foreach (BoxCollider2D collider in _obstacles) 
                collider.size = Vector2.one;
        }

        public List<CellData> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            if (_neighbours.Count == 0)
            {
                foreach (CellData cellData in _cells)
                {
                    var neighbours = GetNeighbours(cellData);
                
                    _neighbours.Add(cellData, neighbours);
                }
            }
            
            var cellDatas = _cells;

            Profiler.BeginSample("LINQ");
             
            CellData startNode = cellDatas.OrderBy(cellData => (startPos - cellData.Cell.transform.position).sqrMagnitude).First();
            CellData targetNode = cellDatas.OrderBy(cellData => (targetPos - cellData.Cell.transform.position).sqrMagnitude).First();

            Profiler.EndSample();

            List<CellData> openSet = new List<CellData>();
            HashSet<CellData> closedSet = new HashSet<CellData>();
            
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                CellData cellData = openSet[0];

                var neighbours = _neighbours[cellData];

                openSet.Remove(cellData);
                closedSet.Add(cellData);

                if (cellData.Id == targetNode.Id) {
                    
                    List<CellData> path = new List<CellData>();
                    
                    CellData currentNode = targetNode;

                    while (currentNode != startNode) {
                        path.Add(currentNode);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();

                    return path;
                }

                foreach (CellData neighbour in neighbours)
                {
                    if (closedSet.Contains(neighbour)) {
                        continue;
                    }

                    float newCostToNeighbour = cellData.GCost + GetDistance(cellData, neighbour);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = cellData;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return null;
        }

        private float GetDistance(CellData start, CellData target)
        {
            return Vector3.Distance(start.Cell.transform.position, target.Cell.transform.position);
        }
        
        private List<CellData> GetNeighbours(CellData cellData)
        {
            var results = Physics2D.OverlapBoxAll(cellData.Cell.transform.position, cellData.Cell.transform.localScale * 2, 90);

            List<CellData> neighbours = new List<CellData>();

            foreach (Collider2D coll in results)
            {
                if (coll.TryGetComponent<Cell>(out var cell))
                {
                    neighbours.Add(cell.Data);
                }
            }

            return neighbours;
        }

        
        
    }
}