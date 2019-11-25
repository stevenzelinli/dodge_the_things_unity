using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public float projectileSpeed = 2.0f;

    public Rigidbody projectile;

    public Rigidbody missile;

    public bool isSeeking;

    public ProjectileType projectileType;

    public Color projectileColour = Color.white;

    public Color spreadColor = Color.cyan;

    public Color seekingColor = Color.magenta;

    public Vector3 spawnLocation;

    public Vector3 getWorldSpawnLocation()
    {
        return transform.TransformPoint(spawnLocation);
    }

    [ContextMenu("Launch")]
    public void Launch(Vector3 direction)
    {
        Vector3 radius = new Vector3(
            projectileType.projectileRadius,
            projectileType.projectileRadius,
            projectileType.projectileRadius
        );
        Vector3 leftSpread = Vector3.zero;
        Vector3 rightSpread = Vector3.zero;
        if (projectileType.spread > 0)
        {
            projectileColour = spreadColor;
        }
        for (int i = 0; i <= projectileType.spread; i++)
        {
            if (i == 0)
            {
                if (isSeeking)
                {
                    Rigidbody newProjectile = Instantiate(
                        missile,
                        transform.TransformPoint(spawnLocation),
                        transform.rotation
                    );
                    newProjectile.gameObject.SetActive(true);
                    newProjectile.velocity = (direction + Vector3.up * 4f) * projectileSpeed;
                }
                else
                {
                    Rigidbody newProjectile = Instantiate(
                        projectile,
                        transform.TransformPoint(spawnLocation),
                        transform.rotation
                    );
                    newProjectile.GetComponent<Renderer>().material.color = projectileColour;
                    newProjectile.transform.localScale = radius;
                    newProjectile.gameObject.SetActive(true);
                    newProjectile.velocity = direction * projectileSpeed;
                }
                leftSpread = direction;
                rightSpread = direction;
            }
            else
            {
                leftSpread = Quaternion.AngleAxis(-10, transform.up) * leftSpread;
                if (isSeeking)
                {
                    Rigidbody leftProjectile = Instantiate(
                        missile,
                        transform.TransformPoint(spawnLocation),
                        transform.rotation
                    );
                    leftProjectile.gameObject.SetActive(true);
                    leftProjectile.velocity = (leftSpread + Vector3.up * 1.5f) * projectileSpeed;
                }
                else
                {
                    Rigidbody leftProjectile = Instantiate(
                        projectile,
                        transform.TransformPoint(spawnLocation),
                        transform.rotation
                    );
                    leftProjectile.transform.localScale = radius;
                    leftProjectile.GetComponent<Renderer>().material.color = projectileColour;
                    leftProjectile.gameObject.SetActive(true);
                    leftProjectile.velocity = leftSpread * projectileSpeed;
                }
                rightSpread = Quaternion.AngleAxis(10, transform.up) * rightSpread;
                if (isSeeking)
                {
                    Rigidbody rightProjectile = Instantiate(
                        missile,
                        transform.TransformPoint(spawnLocation),
                        transform.rotation
                    );
                    rightProjectile.gameObject.SetActive(true);
                    rightProjectile.transform.LookAt(rightProjectile.transform.up);
                    rightProjectile.velocity = (rightSpread + Vector3.up * 1.5f) * projectileSpeed;
                }
                else
                {
                    Rigidbody rightProjectile = Instantiate(
                        projectile,
                        transform.TransformPoint(spawnLocation),
                        transform.rotation
                    );
                    rightProjectile.transform.localScale = radius;
                    rightProjectile.GetComponent<Renderer>().material.color = projectileColour;
                    rightProjectile.gameObject.SetActive(true);
                    rightProjectile.velocity = rightSpread * projectileSpeed;
                }
            }
        }
    }
}
