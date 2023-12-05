using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSliderCtrl : MonoBehaviour
{
    public Slider heartSlider;

    void Start()
    {
        heartSlider = GetComponent<Slider>();
        heartSlider.value = GameManager.intimacy;
        StartCoroutine(checkVal());
    }

    IEnumerator checkVal()
    {
        heartSlider.value = GameManager.intimacy;
        if(heartSlider.value >= 1)
        {
            GameManager.friendLv += 1;
            GameManager.intimacy = 0;
        }
        yield return new WaitForSeconds(1.0f);
    }
}
