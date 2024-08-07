using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float secondtimer;
    public int second;
    public int minute;
    public int day;
    [SerializeField] public float clockspeed = 1;
    public Text clocktext;
    public Text daytext;

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
                day++;
                minute = 0;
            }

            secondtimer = 0;

            clocktext.text = $"{minute}:{second}";
            daytext.text = $"Day: {day}";
        }
    }
}
