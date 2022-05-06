using UnityEngine;

namespace Project.Bot
{
    public class GridCellsConditionChecker
    {
        private readonly Camera _camera;

        public GridCellsConditionChecker(Camera camera)
        {
            _camera = camera;
        }
        
        public bool IsWalkable(Cell cell)
        {
            var overlapCircleAll = Physics2D.OverlapBoxAll(cell.transform.position, cell.transform.localScale, 90);

            return overlapCircleAll.Length > 0 == false;
        }

        public bool IsOutOfBounds(Cell cell)
        {
            Vector3 screenPoint = _camera.WorldToScreenPoint(cell.transform.position);

            if (screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
            {
                return true;
            }

            return false;

        }
        
    }
}