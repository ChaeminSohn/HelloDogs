using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Slider heartSlider;
    [SerializeField]
    TextMeshProUGUI textLv;
    public static GameManager instance;
    public static float intimacy = 0;
    public static bool isPause;
    public static int friendLv = 1;

    private int maxLv = 2;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        //intimacy = PlayerPrefs.GetFloat("DOG_INTIMACY", 0);
        isPause = false;
        //friendLv = PlayerPrefs.GetInt("DOG_LEVEL", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            changeSlider(0.1f);
        }
    }

    public void changeSlider(float f)
    {
        intimacy += f;
        if(intimacy >= 1)
        {
            if(friendLv >= maxLv)
            {
                intimacy = 1.0f;
                return;
            }
            friendLv += 1;
            intimacy = 0;
            textLv.text = friendLv.ToString();
            //PlayerPrefs.SetInt("DOG_LEVEL", friendLv);
        }
        //PlayerPrefs.SetFloat("DOG_INTIMACY", intimacy);
        heartSlider.value = intimacy;
    }
}
