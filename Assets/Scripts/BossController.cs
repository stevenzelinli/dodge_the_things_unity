using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    private bool hasHitGround;

    private GameObject enemies;

    private void Start()
    {
        hasHitGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.transform.tag == "Ground" && !hasHitGround)
		{
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.None;
            hasHitGround = true;
		}
        else if (collision.transform.tag == "Player" && !hasHitGround)
        {
            collision.gameObject.GetComponent<PlayerController>().Hit(9999999);
        }
    }
}
