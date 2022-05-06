using System.Threading.Tasks;
using Project.PlayerLogic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Project.Bot
{
    public class BotMovementService
    {
        private readonly PathGrid _pathGrid;

        private bool _kostyl = true;
        
        public BotMovementService(PathGrid pathGrid)
        {
            _pathGrid = pathGrid;
        }

        public async Task Move(BotMoveContext context)
        {
            _kostyl = true;
            await Task.Delay(10);
            await MoveTask(context);
        }
        
        private async Task MoveTask(BotMoveContext context)
        {
            _kostyl = false;
            Profiler.BeginSample("Nyam");

            var cellDatas = _pathGrid.FindPath(context.Origin, context.TargetPos);

            Profiler.EndSample();
            
            UnitController unit = context.Unit;  

            foreach (CellData cellData in cellDatas)
            {
                Cell cell = cellData.Cell;

                var distance = Vector3.Distance(unit.transform.position, cell.transform.position);

                while (distance > 0.05f)
                {
                    if(_kostyl) return;
                    
                    //Debug.Log($"Left distance: {distance} ,SqrDistance {(cell.transform.position - unit.transform.position).sqrMagnitude}, ID: {cellData.Id}");

                    distance = Vector2.Distance(unit.transform.position, cell.transform.position);

                    Vector3 direction = (cell.transform.position - unit.transform.position);
                    unit.Move(direction * context.Speed);
                    
                    
                    await Task.Delay(1);
                }
            }

            _kostyl = false;
            unit.ResetVelocity();
        }
        
    }
}