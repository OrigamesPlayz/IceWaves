using System.Collections;
using UnityEngine;

public class UseMana : MonoBehaviour
{
    public int maxMana = 100;
    public int currentMana;

    public Mana manaBar;
    public WandAttackRaycast wandScript;

    private bool restoring;

    void Start()
    {
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !restoring)
        {
            StartCoroutine(RestoreMana());
        }
    }

    public void UsingMana(int mana)
    {
        currentMana -= mana;
        manaBar.SetMana(currentMana);
    }

    IEnumerator RestoreMana()
    {
        restoring = true;
        wandScript.enabled = false;

        int startMana = currentMana;
        int missingMana = maxMana - currentMana;

        if (missingMana <= 0)
        {
            restoring = false;
            wandScript.enabled = true;
            yield break;
        }

        float restoreDuration = missingMana / (float)maxMana * 5f;
        float elapsed = 0f;

        while (elapsed < restoreDuration)
        {
            elapsed += Time.deltaTime;

            currentMana = (int)Mathf.Lerp(startMana, maxMana, elapsed / restoreDuration);
            manaBar.SetMana(currentMana);

            yield return null;
        }

        currentMana = maxMana;
        manaBar.SetMana(currentMana);

        restoring = false;
        wandScript.enabled = true;
    }
}
