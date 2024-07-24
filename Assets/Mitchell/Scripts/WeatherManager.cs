using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherManager : MonoBehaviour
{
    public List<WeatherPattern> weatherPatterns = new List<WeatherPattern>();
    private WeatherPattern currentWeather;
    private int minimumWeatherTime = 6;
    private int maximumWeatherTime = 12;
    public Timer clocktimer;
    public Text weathertext;

    int randomWeatherTime;
    int minuteCount;

    void Start()
    {
        ChangeWeather();
        ChooseNextWeatherTime();
    }

    void Update()
    {
        minuteCount = clocktimer.minute;
        if (minuteCount == randomWeatherTime)
        {
            ChangeWeather();
            ChooseNextWeatherTime();
        }
    }

    void ChangeWeather()
    {
        int randomWeatherIndex = UnityEngine.Random.Range(0, weatherPatterns.Count);
        currentWeather = weatherPatterns[randomWeatherIndex];
        weathertext.text = currentWeather.weatherName;
    }

    void ChooseNextWeatherTime()
    {
        randomWeatherTime = UnityEngine.Random.Range(minimumWeatherTime, maximumWeatherTime);
    }
}
