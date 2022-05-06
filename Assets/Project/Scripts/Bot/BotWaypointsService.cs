using System.Linq;
using UnityEngine;

namespace Project.Bot
{
    public class BotWaypointsService : MonoBehaviour
    {
        [SerializeField] private Waypoint[] _waypoints;

        public Waypoint GetPoint(Vector3 pos) => 
            _waypoints.OrderBy(x => (x.transform.position - pos).sqrMagnitude).ToList()[Random.Range(0, _waypoints.Length - 3)];
    }
}