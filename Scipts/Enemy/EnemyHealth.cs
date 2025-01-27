using HCID274._UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] public string enemyName;
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthbarSlider;
    [SerializeField] private Image healthbarFillImage;
    [SerializeField] private Color maxHealthColor;
    [SerializeField] private Color zeroHealthColor;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private int enemyScore = 1000;
    [SerializeField] private float knockbackMultiplier;
    [SerializeField] private float minKnockbackForce;
    [SerializeField] private float maxKnockbackForce;

    public KillEffectController killEffectController;

    private float currentHealth;
    public EnemyAI enemyAI;
    private Rigidbody enemyRigidbody;

    private void Start()
    {
        currentHealth = maxHealth;
        SetHealthbarUi();
        killEffectController = FindObjectOfType<KillEffectController>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void DealDamage(int damage, Vector3 originPosition)
    {
        
        currentHealth -= damage;

        
        Instantiate(damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageText>().Initialise(damage);

        
        Vector3 knockbackDirection = (transform.position - originPosition).normalized;

        
        float knockbackMagnitude = Mathf.Clamp(damage * knockbackMultiplier, minKnockbackForce, maxKnockbackForce);

        
        Vector3 upwardForce = Vector3.up * knockbackMagnitude*0.2f; 
        
        Vector3 backwardForce = knockbackDirection * knockbackMagnitude * 0.8f; 

        
        Vector3 knockbackForce = upwardForce + backwardForce;

        
        StartCoroutine(ApplyKnockback(knockbackForce));

        CheckIfDead();
        
        SetHealthbarUi();
    }

    
    private IEnumerator ApplyKnockback(Vector3 knockbackForce)
    {
        float knockbackStartTime = Time.time;
        float knockbackDuration = 1f;

        enemyAI.isKnockedBack = true; 
        enemyRigidbody.isKinematic = false; 
        enemyRigidbody.AddForce(knockbackForce, ForceMode.Impulse);

        while (Time.time < knockbackStartTime + knockbackDuration)
        {
            yield return null;
        }

        enemyRigidbody.velocity = Vector3.zero; 
        enemyRigidbody.isKinematic = true; 
        enemyAI.isKnockedBack = false; 
    }


    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            LevelManager.instance.EnemyDefeated(); 
            ScoreUI scoreUI = FindObjectOfType<ScoreUI>(); 
            if (scoreUI != null)
            {
                scoreUI.AddScore(enemyScore); 
            }
            Destroy(gameObject); 
            killEffectController.TriggerKillEffect();
            Debug.Log("Destroy!");
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage(); 
        healthbarSlider.value = healthPercentage; 
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / 100f); 
    }

    private float CalculateHealthPercentage()
    {
        return Mathf.Clamp((currentHealth / maxHealth) * 100, 0f, 100f); 
    }
}