using HCID274._UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthbarSlider; 
    [SerializeField] private Image healthbarFillImage; 
    [SerializeField] private Color maxHealthColor; 
    [SerializeField] private Color zeroHealthColor; 
    [SerializeField] private GameObject damageTextPrefab; 
    [SerializeField] private AudioClip healingAudioClip; 

    private int currentHealth; 
    private AudioSource audioSource; 

    private int healingAmount = 1; 
    private float healingTimer = 10f; 
    private float lastDamageTime; 
    private float healingAccumulator = 0f;


    private void Start()
    {
        currentHealth = 100; 
        SetHealthbarUi(); 

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= healingTimer && currentHealth < (int)maxHealth)
        {
            Heal(healingAmount * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = healingAudioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (audioSource.clip == healingAudioClip && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }


    public void DealDamage(int damage)
    {
        currentHealth -= damage; 
        lastDamageTime = Time.time; 

        CheckIfDead(); 
        SetHealthbarUi(); 
    }

    private void Heal(float amount)
    {
        healingAccumulator += amount;
        int healingToApply = Mathf.FloorToInt(healingAccumulator);
        if (healingToApply >= 1)
        {
            currentHealth = Mathf.Min((int)maxHealth, currentHealth + healingToApply);
            healingAccumulator -= healingToApply;
            SetHealthbarUi();
        }
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject); 
            Debug.Log("Destroy!");
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage(); 
        healthbarSlider.value = healthPercentage; 
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / maxHealth); 
    }

    private float CalculateHealthPercentage()
    {
        return Mathf.Clamp(((float)currentHealth / maxHealth) * 100, 0f, maxHealth); 
    }
}
