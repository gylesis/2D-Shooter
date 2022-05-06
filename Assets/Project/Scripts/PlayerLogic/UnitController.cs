using UnityEngine;

namespace Project.PlayerLogic
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        public void Rotate(Quaternion quaternion)
        {
            transform.rotation = quaternion;
        }

        public void Move(Vector2 moveDirection)
        {
            _rigidbody.velocity = moveDirection;
        }

        public void ResetVelocity()
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}