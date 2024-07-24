using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float secondtimer;
    int second;
    int minute;
    [SerializeField] public float clockspeed = 1;
    public Text clocktext;

    // Start is called before the first frame update
    void Start()
    {
        // clocktext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        secondtimer += Time.deltaTime * clockspeed;

        if (secondtimer >= 1f)
        {
            second++;
            if (second == 60)
            {
                minute++;
                second = 0;
            }

            if (minute == 24)
            {
                minute = 0;
            }

            secondtimer = 0;

            clocktext.text = $"{minute}:{second}";
        }
    }
}
