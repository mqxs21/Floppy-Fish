using UnityEngine;
using UnityEngine.SceneManagement;
public class HydrationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static float maxHydration = 100;
    public static float currentHydration = 5;

    public UnityEngine.UI.Image HydrationBar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HydrationBar.fillAmount = Mathf.Lerp(HydrationBar.fillAmount,(currentHydration / maxHydration), Time.deltaTime * 5);

       // Debug.Log("Hydration: " + HydrationManager.currentHydration);
        
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
