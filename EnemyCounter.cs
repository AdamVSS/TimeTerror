using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    //enemy counter variables
    public TextMeshProUGUI enemiesLeftText;
    private int totalEnemies;
    private int currentEnemies;

    //player health variables
    public PlayerHealth playerHealth;
    public Image healthFillImage;
    public TextMeshProUGUI healthText;
    public Gradient healthGradient;

    //Bomb code spawn variables
    public GameObject bombBookPrefab;
    public TextMeshProUGUI codeStatusText;
    private bool bookSpawned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnemies = totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateEnemyText();
    }

    void Update()
    {
        if (playerHealth == null) return;

        //fill health bar based on how much health player has
        float fillAmount = (float)playerHealth.GetCurrentHealth() / playerHealth.maxHealth;
        healthFillImage.fillAmount = fillAmount;
        healthFillImage.color = healthGradient.Evaluate(fillAmount);

        healthText.SetText(playerHealth.GetCurrentHealth() + "/" + playerHealth.maxHealth);
    }

    public void OnEnemyKilled(Vector3 deathPosition)
    {
        currentEnemies--;
        UpdateEnemyText();

        if (currentEnemies <= 0 && !bookSpawned)
        {
            SpawnBombBook(deathPosition);
        }
    }

    private void UpdateEnemyText()
    {
        enemiesLeftText.SetText("Enemies Alive: " + currentEnemies + "/" + totalEnemies);
    }

    private void SpawnBombBook(Vector3 position)
    {
        GameObject book = Instantiate(bombBookPrefab, position, Quaternion.identity);

        BombCodePickup pickup = book.GetComponent<BombCodePickup>();
        if (pickup != null)
        {
            pickup.codeStatusText = codeStatusText;
        }
    }
}

