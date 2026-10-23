using System;
using System.Collections;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private RawImage[] energyBar;
    [SerializeField] private int totalEnergy = 350;
    private static int currentEnergy = 350;

    public bool OperationEnded(int pEnergyLost)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - pEnergyLost);

        int barsRemaining = Mathf.CeilToInt(
            (float)currentEnergy / totalEnergy * energyBar.Length);

        for (int i = 0; i < energyBar.Length - barsRemaining; i++)
        {
            if (energyBar[i].enabled)
            {
                energyBar[i].enabled = false;
            }
        }

        if (energyBar.Length - barsRemaining >= 6)
        {
            StartCoroutine(ShowEnergyBar());
            return true;
        }
        
        return false;
    }

    private IEnumerator ShowEnergyBar()
    {
        yield return new WaitForSeconds(1);
        currentEnergy = totalEnergy;
        for (int i = 0; i < energyBar.Length; i++)
        {
            energyBar[i].enabled = true;
        }
    }
}