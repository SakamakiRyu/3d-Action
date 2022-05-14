using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditControl : MonoBehaviour
{
    [SerializeField]
    private Slider _bgmSlider;

    [SerializeField]
    private Slider _seSlider;

    public void OnBGMValueChenged()
    {
        SoundManager.Instance.ChengeBGMVolume(_bgmSlider.value);
    }

    public void OnSEValueChenged()
    {
        SoundManager.Instance.ChengeSEVolume(_seSlider.value);
    }
}
