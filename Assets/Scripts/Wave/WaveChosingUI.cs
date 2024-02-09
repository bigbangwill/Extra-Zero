using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class WaveChosingUI : MonoBehaviour
{
    private WaveOptionUI currentWaveOption;


    public void SetSelectedWaveOption(WaveOptionUI waveOption)
    {
        if (waveOption != null)
        {
            waveOption.DeselectThisOption();
        }
        currentWaveOption = waveOption;
        currentWaveOption.SelectThisOption();
    }

    public void RunTheWaveEffects()
    {
        if (currentWaveOption)
        {
            //WaveManager.Instance.
        }
        currentWaveOption.ExecuteImpact();
    }


}
