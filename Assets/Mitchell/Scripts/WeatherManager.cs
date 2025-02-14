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
    public TimeManager timemanager;
    public Text weathertext;
    public ParticleSystem rain;
    public ParticleSystem snow;
    public ParticleSystem godRays;

    int randomWeatherTime;
    int minuteCount;

    void Start()
    {
        ChangeWeather();
        ChooseNextWeatherTime();
    }

    void Update()
    {
        minuteCount = timemanager.Minutes;
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

        if (currentWeather.weatherName == "Raining")
        {
            rain.Play();
            snow.Stop();
            godRays.Stop();
        }

        if (currentWeather.weatherName == "Snowy")
        {
            rain.Stop();
            snow.Play();
            godRays.Stop();
        }

        if (currentWeather.weatherName == "Cloudy")
        {
            rain.Stop();
            snow.Stop();
            godRays.Stop();
        }

        if (currentWeather.weatherName == "No Weather")
        {
            rain.Stop();
            snow.Stop();
            godRays.Play();
        }
    }

    void ChooseNextWeatherTime()
    {
        randomWeatherTime = UnityEngine.Random.Range(minimumWeatherTime, maximumWeatherTime);
    }
}
