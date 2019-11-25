using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public PlayerController player;

    public PlayerController boss;

    public GameObject playerHp;

    public Text playerHpText;

    public Text playerPowerUpText;

    public GameObject bossHpBar;

    public GameObject bossHp;

    public Text bossHpText;

    private void Update()
    {
        float playerHealthPercentage = player.health / player.maxHealth;
        playerHp.transform.localScale = new Vector3(playerHealthPercentage, 1, 1);
        playerHpText.text = Mathf.FloorToInt(player.health) + " HP";

        string powerUpText = "Power Up:\n";
        if (player.powerUp == null)
        {
            powerUpText += "<b>None</b>";
        }
        else if (player.powerUp.getType().Equals(PowerUpType.SPLIT))
        {
            playerPowerUpText.color = Color.cyan;
            powerUpText += "<b>Split Shot " + player.powerUp.stackCount + "</b>";
        }
        else if (player.powerUp.getType().Equals(PowerUpType.TRACKING))
        {
            playerPowerUpText.color = Color.magenta;
            powerUpText += "<b>Seek Shot " + player.powerUp.stackCount + "</b>";
        }
        else
        {
            playerPowerUpText.color = Color.black;
            powerUpText += "<b>None</b>";
        }
        playerPowerUpText.text = powerUpText;

        if (boss.enabled == true)
        {
            bossHpBar.SetActive(true);
            float bossHealthPercentage = boss.health / boss.maxHealth;
            bossHp.transform.localScale = new Vector3(bossHealthPercentage, 1, 1);
            bossHpText.text = "<b>" + boss.name + "</b>: " + Mathf.FloorToInt(boss.health) + " HP";
        }
    }
}
