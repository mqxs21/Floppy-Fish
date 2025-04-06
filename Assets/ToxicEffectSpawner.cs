using UnityEngine;

public class ToxicEffectSpawner : MonoBehaviour
{
    public GameObject toxicParticleEffectPrefab; // drag your prefab here
    public float moveSpeed = 0.2f;

    private GameObject currentEffect;
    private float nextSpawnTime;

    void Start()
    {
        SpawnNewEffect();
    }

    void Update()
    {
        if (currentEffect != null)
        {
            // ✅ Move the effect to the LEFT
            currentEffect.transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            // ✅ Destroy when it goes OFF the LEFT side
            if (currentEffect.transform.position.x < -20f)
            {
                Destroy(currentEffect);
                currentEffect = null;
                ScheduleNextSpawn();
            }
        }
        else
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnNewEffect();
            }
        }
    }

    void SpawnNewEffect()
    {
        currentEffect = Instantiate(toxicParticleEffectPrefab, transform.position, Quaternion.identity);
    }

    void ScheduleNextSpawn()
    {
        float randomDelay = Random.Range(10f, 15f);
        nextSpawnTime = Time.time + randomDelay;
    }
}
