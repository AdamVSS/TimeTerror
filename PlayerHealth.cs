using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private bool isDead = false;

    //Health regeneration
    public float regenRate; //time in secs between each regen
    public int regenAmount;
    private float regenTimer;
    public float combatCooldown = 5f;
    private float combatTimer = 0f;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth(){
        return currentHealth;
    }

    public void TakeDamage(int damage){
        if (isDead) return;

        currentHealth -= damage;
        combatTimer = 0f; //resets combat timer on damage taken

        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0){
            Die();
        }
    }

    private void Die(){
        isDead = true;
        Debug.Log("Player Dead");
        
        //NAVIGATE TO GAME OVER SCREEN HERE
        SceneManager.LoadScene("DeathScene");
        //unlocks mouse cursor when puzzle is active
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!isDead)
        {
            combatTimer += Time.deltaTime;
            RegenerateHealth();
        }
    }

    public void NotifyUnderAttack(){
        combatTimer = 0f;
    }

    private void RegenerateHealth(){
        //conditions that stop regenerate health activating
        if (currentHealth >= maxHealth) return;
        if (combatTimer < combatCooldown) return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenRate){
            currentHealth += regenAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            regenTimer = 0f;
        }
    }
}
