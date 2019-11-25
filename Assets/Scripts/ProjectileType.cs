using UnityEngine;
using System.Collections;

public class ProjectileType
{
	public int spread;

	public float projectileRadius;

	public bool isArtillery;

	public float artilleryArcHeight;

	public bool hasSplashDamage;

	public float splashRadius;

    public enum TypeKey
    {
        REGULAR = 0,
        SPREAD = 1,
        FOLLOW = 2,
        ARTILLERY = 3
    }

    private ProjectileType(int spread, float projectileRadius)
    {
		this.spread = spread;
		this.projectileRadius = projectileRadius;
		this.isArtillery = false;
		this.artilleryArcHeight = 0;
		this.hasSplashDamage = false;
		this.splashRadius = 0;
    }

    private ProjectileType(int spread, float projectileRadius, bool isArtillery, float artilleryArcHeight, bool hasSplashDamage, float splashRadius)
    {
		this.spread = spread;
		this.projectileRadius = projectileRadius;
		this.isArtillery = isArtillery;
		this.artilleryArcHeight = artilleryArcHeight;
		this.hasSplashDamage = hasSplashDamage;
		this.splashRadius = splashRadius;
	}

    public static ProjectileType getRegular(float projectileRadius)
    {
        return new ProjectileType(0, projectileRadius);
    }

    public static ProjectileType getSpread(float projectileRadius, int spread)
    {
        return new ProjectileType(spread, projectileRadius);
    }

    public static ProjectileType getArtillery(float projectileRadius, int spread, float artilleryArcHeight, bool hasSplashDamage, float splashRadius)
    {
        return new ProjectileType(spread, projectileRadius, true, artilleryArcHeight, hasSplashDamage, splashRadius);
    }
}
