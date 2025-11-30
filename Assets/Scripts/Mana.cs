using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public Slider manaSlider;

    public void SetMaxMana(int mana)
    {
        manaSlider.maxValue = mana;
        manaSlider.value = mana;
    }
    public void SetMana(int health)
    {
        manaSlider.value = health;
    }
}
