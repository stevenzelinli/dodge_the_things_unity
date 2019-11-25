using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public float projectileLiveTime;

    public float damage;

    public float turnSpeed;

    public bool isSeeking;

    public float seekSpeed;

    public bool isHuman;

    public float seekStartDelay;

    private float seekStartTime;

    private Rigidbody rb;

    public GameObject[] targets;

    public GameObject target;

    private float projectileEndTime;

    void Start()
    {
        if (isHuman)
        {
            targets = GameObject.FindGameObjectsWithTag("Enemy");
        }
        else
        {
            targets = new GameObject[] { GameObject.FindGameObjectWithTag("Player") };
        }
        rb = GetComponent<Rigidbody>();
        seekStartTime = Time.time + seekStartDelay;
        projectileEndTime = Time.time + projectileLiveTime;
    }

    void Update()
    {
        if (isSeeking && Time.time > seekStartTime)
        {
            GameObject closestTarget;
            Array.Sort(targets, new Comparison<GameObject>((a, b) =>
            {
                float aDistance = (a.transform.position - transform.position).magnitude;
                float bDistance = (b.transform.position - transform.position).magnitude;
                return aDistance.CompareTo(bDistance);
            }));
            closestTarget = targets[0];
            rb.useGravity = false;
            if (closestTarget != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(closestTarget.transform.position - transform.position + new Vector3(0, 0.2f, 0));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.velocity = Vector3.Normalize(closestTarget.transform.position - transform.position) * seekSpeed;
                //transform.Translate(transform.forward * Time.deltaTime * seekSpeed);
            }
        }
    }

    void LateUpdate()
    {
        if (Time.time > projectileEndTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Enemy Projectile")
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.Hit(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (gameObject.tag == "Projectile")
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.Hit(damage);
                Destroy(gameObject);
            }
        }
        //else if (collision.gameObject.tag == "Projectile")
        //{
        //    if (gameObject.tag == "Enemy Projectile")
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
