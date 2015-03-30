using UnityEngine;
using System;
using System.Collections;

public class Forecast
{
	public DateTime DateTime;
	public int SceneId;
	
	public bool IsFog;
	public float FogIntensity;
	public Color DayFogColor;
	public Color NightFogColor;
	
	public bool IsRain;
	public int RainIntensity;
	
	public bool IsSnow;
	public int SnowIntensity;
	
	public WeatherTypeEnum WeatherType;
	
	private Forecast previousForecast;
	
	private ZoneConfig config;
	
	public Forecast()
	{
		config = new ZoneConfig();
	}
	
	private void FogChance()
	{
		//fog is disabled
		if (!config.IsPossibleFog)
		{
			IsFog = false;
			return;
		}
		int chance = BasicRandom.Instance.Next(99);
		//detect fog chance
		if (DateTime.Hour >= WeatherConfig.FogStartHour
		   	&& DateTime.Hour <= WeatherConfig.FogEndHour)
		{
			if ((previousForecast == null || !previousForecast.IsFog) && chance > 100 - config.ChanceToFog)
			{
				IsFog = true;
			}
			else if (previousForecast.IsFog && chance > 100 - config.ChanceToContinueFog)
			{
				IsFog = true;
			}
			else
			{
				IsFog = false;
			}
			FogIntensity = Convert.ToSingle((BasicRandom.Instance.Next(config.MaximumFogIntensity)) + config.MinimumFogIntensity)  / 100;
		}
	}
	
	private void RainChance()
	{
		//fog is disabled
		if (!config.IsPossibleRain)
		{
			IsRain = false;
			return;
		}
		int chance = BasicRandom.Instance.Next(99);
		if ((previousForecast == null || !previousForecast.IsRain) && (chance > 100 - config.ChanceToRain))
		{
			IsFog = true;
			FogIntensity = 0.091f;
			IsRain = true;
		}
		else if (previousForecast.IsRain && chance > 100 - config.ChanceToContinueRain)
		{
			IsFog = true;
			FogIntensity = 0.091f;
			IsRain = true;
		}
		else
		{
			IsRain = false;
		}
		RainIntensity = BasicRandom.Instance.Next(config.MaximumRainIntensity - config.MinimumRainIntensity) + config.MinimumRainIntensity;
	}
	
	private void SnowChance()
	{
		//snow is disabled or rain is in this hour
		if (!config.IsPossibleSnow || IsRain)
		{
			IsSnow = false;
			
			return;
		}
		int chance = BasicRandom.Instance.Next(99);
		if ((previousForecast == null || !previousForecast.IsSnow) && (chance > 100 - config.ChanceToSnow))
		{
			IsFog = true;
			FogIntensity = 0.091f;
			IsSnow = true;
		}
		else if (previousForecast.IsSnow && chance > 100 - config.ChanceToContinueSnow)
		{
			IsFog = true;
			FogIntensity = 0.091f;
			IsSnow = true;
		}
		else
		{
			IsSnow = false;
		}
		RainIntensity = BasicRandom.Instance.Next(config.MaximumSnowIntensity - config.MinimumSnowIntensity) + config.MinimumSnowIntensity;
	}
	
	public void Prepare(Forecast PreviousForecast)
	{
		SceneId = config.SceneId = Application.loadedLevel;
		
		previousForecast = PreviousForecast;
		
		FogChance();
		RainChance();
		SnowChance();
	}
}
