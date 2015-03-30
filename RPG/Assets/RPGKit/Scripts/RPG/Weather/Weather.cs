using UnityEngine;
using System;
using System.Collections;

public class Weather : MonoBehaviour {
	
	//rain object
	GameObject rain;
	//emiter of rain
	public static ParticleEmitter rainComponent;
	
	//snow object
	GameObject snow;
		
	private Queue Forecasts;
	private Material currentSkybox;
	
	
	private float rainTick;
	private float rainIncrement;
	
	private float fogTick;
	private float fogIncrement;
	
	private Forecast PastForecast;
	private Forecast CurrentForecast;
	
	//used only for creating new forecast
	Forecast previousForecast = new Forecast();
	
	private int CurrentSceneId;
	private Player player;
		
	//current game time
	public static DateTime CurrentDateTime;
	
	void Start () 
	{
		player = gameObject.GetComponent<Player>();
		CurrentForecast = new Forecast();
		float dayRatio = WeatherConfig.const_day / (WeatherConfig.gameDayInMinutes * WeatherConfig.const_minute);
		Forecasts = new Queue();
		CurrentDateTime = new DateTime(WeatherConfig.StartYear, WeatherConfig.StartMonth, WeatherConfig.StartGameDay, WeatherConfig.StartHour, WeatherConfig.StartMinute, 0);
		
		//store skybox
		currentSkybox = RenderSettings.skybox;
		
		//rain component
		rain = (GameObject)GameObject.Instantiate(Resources.Load("weather/rain"));
		
		rainComponent = rain.GetComponent<ParticleEmitter>();
		rainComponent.minEmission = 0;
		rainComponent.maxEmission = 0;
		DontDestroyOnLoad(rain);
		
		float timeDeltaRatio = Time.deltaTime * dayRatio;
		
		DontDestroyOnLoad(rain);
		rainTick = WeatherConfig.RainChangeTime * 60 / timeDeltaRatio;
		fogTick = WeatherConfig.FogChangeTime * 60 / timeDeltaRatio;
		fogIncrement = 0.65f / fogTick;
		rainIncrement = WeatherConfig.MaximumRainIntensity / rainTick;
		CreateForecast();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.scene.SceneType == SceneTypeEnum.MainMenu)
			return;
		
		PrepareForecasts();
		
		//setting for for main camera
		//color must be same like missing skybox
		Camera.mainCamera.backgroundColor  = WeatherConfig.dayFogColor;
		RenderSettings.fogColor = WeatherConfig.dayFogColor;
		
		//update game time
		float addSecond = Time.deltaTime * DayCycle.dayRatio;
		CurrentDateTime = CurrentDateTime.AddSeconds(addSecond);
		
		ChangeFog();
		
		ChangeRain();
		
		ChangeSnow();
	}
	
	private void PrepareForecasts()
	{
		CurrentSceneId = Application.loadedLevel;
		
		//checking forecasts
		if (Forecasts == null || Forecasts.Count == 0)
			CreateForecast();
		
		Forecast tempForecast = (Forecast)Forecasts.Peek();
		
		//forecasts are for different zone! 
		//need to refresh and create new forecasts
		if (CurrentSceneId != tempForecast.SceneId)
		{
			Forecasts = new Queue();
			CreateForecast();
		}
		
		//setting current forecast
		if (CurrentDateTime > tempForecast.DateTime)
		{
			PastForecast = CurrentForecast;
			CurrentForecast = (Forecast)Forecasts.Dequeue();
			CreateForecast();
		}
		
		//keep rain at same position where is player
		Vector3 playerPosition = transform.position;
		playerPosition.y += 10;
		if (snow != null)
			snow.transform.position = playerPosition;
		playerPosition.y += 40;
		rain.transform.position = playerPosition;
	}
	
	void ChangeRain()
	{
		if (CurrentForecast == null)
		{
			PrepareForecasts();
			CurrentForecast = (Forecast)Forecasts.Dequeue();
		}
		
		if (CurrentForecast.IsRain)
		{
			//detecting if we need to increase rain
			if (PastForecast.RainIntensity < CurrentForecast.RainIntensity || !PastForecast.IsRain)
			{
				if (RenderSettings.skybox != null)
					return;	
				//increating rain
				if (rainComponent.maxEmission < CurrentForecast.RainIntensity)
					rainComponent.maxEmission += (int)rainIncrement;
			}
			else if (rainComponent.maxEmission > CurrentForecast.RainIntensity)
					rainComponent.maxEmission -= (int)rainIncrement;
			//minimum rain particles
			rainComponent.minEmission = WeatherConfig.MinimumRainParticles;
		}
		else
		{
			if (rainComponent == null)
				return;
			//lowering raing
			if (rainComponent.maxEmission - (int)rainIncrement < 0)
			{
				rainComponent.minEmission = rainComponent.maxEmission = 0;
			}
			else
				rainComponent.maxEmission -= (int)rainIncrement;
		}
	}
	
	void ChangeSnow()
	{
		if (CurrentForecast == null)
			PrepareForecasts();
		
		if (CurrentForecast.IsSnow && !CurrentForecast.IsRain)
		{
			if (snow == null)
			{
				snow = (GameObject)GameObject.Instantiate(Resources.Load("weather/snow"));
			}
		}
		else
		{
			if (snow != null)
			{
				Destroy(snow);
				snow = null;
			}
		}
	}
	
	void ChangeFog()
	{
		if (CurrentForecast == null)
			PrepareForecasts();
		
		if (CurrentForecast == null)
			return;
		
		if (CurrentForecast.IsFog)
		{
			RenderSettings.fog = true;
			if (PastForecast.FogIntensity < CurrentForecast.FogIntensity)
			{
				if (RenderSettings.fogDensity < CurrentForecast.FogIntensity)
					RenderSettings.fogDensity += fogIncrement;
			}
			else if (RenderSettings.fogDensity > CurrentForecast.FogIntensity)
			{
				RenderSettings.fogDensity -= fogIncrement;
			}
		}
		else
		{
			if (RenderSettings.fogDensity > 0)
			{
				if (RenderSettings.fogDensity - fogIncrement < 0)
				{
					RenderSettings.fogDensity = 0;
					RenderSettings.fog = false;
				}
				else
					RenderSettings.fogDensity -= fogIncrement;
			}
		}
		if ((CurrentForecast.IsRain || CurrentForecast.IsSnow) && RenderSettings.fogDensity < 0.9f)
			RenderSettings.fogDensity = 0.091f;
		
		if (RenderSettings.fogDensity > 0.09f)
		{
			RenderSettings.skybox = null;
		}
		else
		{
			RenderSettings.skybox = currentSkybox;
			RenderSettings.skybox.SetFloat("_Blend", DayCycle.blender);
		}
		
		if (RenderSettings.fogDensity < 0)
			RenderSettings.fogDensity = 0;
	}
	
	void OnGUI()
	{
		/*Rect rect;
		int index = 1;
		foreach(Forecast forecast in Forecasts)
		{
			rect = new Rect(10,10 + (30 * index) , 400, 20);
			GUI.Label(rect, index+ ". forecast will be snow: " + forecast.IsSnow);
			index++;
		}
		rect = new Rect(500,550,350,25);
		GUI.Label(rect, RenderSettings.fogDensity + " " + RenderSettings.fog);*/
	}
	
	void CreateForecast()
	{
		if (Forecasts == null)
			Forecasts = new Queue();
		
		for(int index = 1; index <= WeatherConfig.MaximumForecast; index++)
		{
			//if you repeat that progress
			if (Forecasts.Count == WeatherConfig.MaximumForecast)
				return;
			
			Forecast forecast = new Forecast();
			forecast.Prepare(previousForecast);
			forecast.DateTime = CurrentDateTime.AddHours(Forecasts.Count + 1);
			forecast.SceneId = Application.loadedLevel;
			
			previousForecast = forecast;
			Forecasts.Enqueue(forecast);
		}
	}
}

public enum WeatherTypeEnum
{
	Normal = 0,
	Fog = 1,
	Rain = 2,
	Snow = 3
}