using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HydrationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static float maxHydration = 100;
    public static float currentHydration = 100;

    public UnityEngine.UI.Image HydrationBar;
    public bool muteHydrationBar = false;
    public UnityEngine.UI.Image statusPanel;
    public Color blueStatusColor;
    public Color dryStatusColor;
    private Color targetColor;
    public ParticleSystem dieParticleEffect;
    public Transform playerLocation;
    private bool hasDiedDueToDehydration = false;
    public TextMeshProUGUI hydrationTextPopup;
    public 
    void Start()
    {
        currentHydration = maxHydration;
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
{
    hydrationTextPopup.gameObject.SetActive(hasDiedDueToDehydration);
    if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
    {
        targetColor = blueStatusColor;
    }
    else
    {
        targetColor = dryStatusColor;
    }
    statusPanel.color = Color.Lerp(statusPanel.color, targetColor, Time.deltaTime * 5f);
    // Update the hydration bar UI
    HydrationBar.fillAmount = Mathf.Lerp(HydrationBar.fillAmount, currentHydration / maxHydration, Time.deltaTime * 5f);

    // Drain or restore hydration based on environment
    if (PlayerStateManager.currentPlayerState == PlayerStateManager.PlayerState.Water)
    {
        currentHydration += 20f * Time.deltaTime; 
    }
    else
    {
        currentHydration -= 10f * Time.deltaTime; 
    }
    if (muteHydrationBar)
    {
        HydrationBar.fillAmount = 1;
        currentHydration = maxHydration;
    }

    // Clamp hydration between 0 and max
    currentHydration = Mathf.Clamp(currentHydration, 0f, maxHydration);

    // Optional: Do something when hydration is empty
    if (currentHydration <= 0f)
    {
        Debug.Log("Fish is dried out! Restarting...");
        if (!hasDiedDueToDehydration)
    {
        Instantiate(dieParticleEffect, playerLocation.position, Quaternion.identity);
        StartCoroutine(WaitForDeath());
        
        hasDiedDueToDehydration = true;
    }
        
         // Or show game over UI
    }
}
IEnumerator WaitForDeath(){
    yield return new WaitForSecondsRealtime(0.2f);
    Time.timeScale = 0.3f;
    yield return new WaitForSecondsRealtime(1f);
    
    
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}



    public void AddHydration(int amount)
    {
        HydrationManager.currentHydration += amount;
        if (HydrationManager.currentHydration > HydrationManager.maxHydration)
        {
            HydrationManager.currentHydration = HydrationManager.maxHydration;
        }
    }

    public void RemoveHydration(int amount)
    {
        HydrationManager.currentHydration -= amount;
        if (HydrationManager.currentHydration < 0)
        {
            HydrationManager.currentHydration = 0;
        }
    }
}
