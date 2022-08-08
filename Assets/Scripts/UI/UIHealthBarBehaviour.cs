using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBarBehaviour : MonoBehaviour
{
    [Header("Unit Health")]

    [SerializeField] private UnitHealthBehaviour healthBehaviour;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI healthTextMesh;
    [SerializeField] private Image healthBar;


    // Start is called before the first frame update
    void Start()
    {
        SetupHealthUI();
    }

    private void OnEnable()
    {
        healthBehaviour.HealthChangedEvent += UpdateHealthUI;
    }
    private void OnDisable()
    {
        healthBehaviour.HealthChangedEvent -= UpdateHealthUI;
    }
    private void SetupHealthUI() 
    {
        UpdateHealthUI(healthBehaviour.GetCurrentHealth());
    }

    private void UpdateHealthUI( int healthAmount) 
    {
        float maxHealth = healthBehaviour.GetMaxHealth();
        healthBar.fillAmount = healthAmount / maxHealth;
        healthTextMesh.text = string.Format("{0}/{1}", healthAmount, maxHealth);
    }

}
