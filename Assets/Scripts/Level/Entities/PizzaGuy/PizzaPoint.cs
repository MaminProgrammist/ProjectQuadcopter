using UnityEngine;
using System.Collections;

namespace Entities
{
    [System.Obsolete]
	public class PizzaPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }

}

