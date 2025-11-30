using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;

    [SerializeField] private GameObject spawnPoint;

    public Wave[] waves;
    public int currentWaveIndex = 0;
    [HideInInspector] public bool readyToCountDown;

    public TextMeshProUGUI wavesNum;
    public DamagePlayer damagePlayer;
    public END end;

    private bool restoringHealth;

    private void Start()
    {
        readyToCountDown = true;
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    private void Update()
    {   
        if (currentWaveIndex >= waves.Length)
        {
            wavesNum.SetText("Waves Complete");
            end.ended = true;
            return;
        }

        if (!readyToCountDown)
            wavesNum.SetText("Wave " + (currentWaveIndex + 1));
        else if (readyToCountDown)
            wavesNum.SetText("Next wave in: " + countdown.ToString("F2") + "s");

        if (readyToCountDown) countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            readyToCountDown = false;
            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWaveIndex++;

            StartCoroutine(RestoreHealth());
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                EnemyAI enemy = Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.transform);

                enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }

    private IEnumerator RestoreHealth()
    {
        if (damagePlayer == null) yield break;

        restoringHealth = true;

        int startHealth = damagePlayer.currentHealth;
        int missingHealth = damagePlayer.maxHealth - damagePlayer.currentHealth;

        if (missingHealth <= 0)
        {
            restoringHealth = false;
            yield break;
        }

        float restoreDuration = (missingHealth / (float)damagePlayer.maxHealth) * 5f;
        float elapsed = 0f;

        while (elapsed < restoreDuration)
        {
            elapsed += Time.deltaTime;
            damagePlayer.currentHealth = (int)Mathf.Lerp(startHealth, damagePlayer.maxHealth, elapsed / restoreDuration);
            damagePlayer.healthBar.SetHealth(damagePlayer.currentHealth);
            yield return null;
        }

        damagePlayer.currentHealth = damagePlayer.maxHealth;
        damagePlayer.healthBar.SetHealth(damagePlayer.currentHealth);

        restoringHealth = false;
    }
}

 [System.Serializable]

 public class Wave
{
    public EnemyAI[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}
