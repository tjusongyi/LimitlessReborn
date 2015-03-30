using UnityEngine;
using System;
using System.Collections;

public class DayCycle : MonoBehaviour {

	private float blenderIncrement = 0;
	
	private float gameDayInSeconds;
	
	public float currentTime;
	
	private float dayRotation = 0;
	private float nightRotation = 0;
	private float currentAngle = 0;
	
	public static float dayRatio = 0;
	public static DayCycleTypeEnum DayState;
	public static float blender = 1;
	public static bool isLoaded = false;
	
	private float sunset = 0;
	private float sunrise = 0;
	private float rotation;
	
	private float dayPartInSeconds = 0;
	
	private LensFlare flare;
	private Flare storeFlare;
	
	//set sun position before time calculation
	private void SetupStartupSunPosition()
	{
		/*float zPos = Terrain.activeTerrain.terrainData.size.z / 2;
		float xPos = Terrain.activeTerrain.terrainData.size.x;
		
		Vector3 sunPos = new Vector3(xPos, 50, zPos);
		Quaternion rotation = Quaternion.Euler(0,270,0);
		transform.position = sunPos;
		transform.rotation = rotation;*/
	}
	
	private void DefaultInit()
	{
		SetupStartupSunPosition();
		
		//how long takes one game day
		gameDayInSeconds = WeatherConfig.gameDayInMinutes * WeatherConfig.const_minute;
		//ratio between normal day and game day
		dayRatio = WeatherConfig.const_day / gameDayInSeconds;
		//calculates sun rotation per day (for case that day is same like night)
		rotation = WeatherConfig.degreesPerSecond * dayRatio;
		
		sunrise = ((WeatherConfig.sunRise * 3600) / dayRatio);
		
		CalculateDayLength();
		
		CalculateSunPosition();
		
		
		//recalcute sunset according start before real sunset
		float timeOfSunrise = 24 * 60 / WeatherConfig.blendingTimeInMinutes;
		
		float minutes = gameDayInSeconds / timeOfSunrise;
		sunset -= minutes;
		blenderIncrement = 1 / minutes / 60;
		sunrise -= minutes;
	}
	
	
	
	private void CalculateSunPosition()
	{
		DateTime date = Weather.CurrentDateTime;
		int second;
		float defaultRotation;
		if (date.Hour >= WeatherConfig.sunRise)
		{
		    second = date.Second + (date.Minute * 60) + ((date.Hour - WeatherConfig.sunRise) * 3600);
			currentTime = second / dayRatio + sunrise;
			if (WeatherConfig.DayLongerThanNight != 1)
			{
				if (currentTime > sunset)
				{
					float temp = currentTime - sunset;
					float night = gameDayInSeconds - dayPartInSeconds;
					defaultRotation = (180 / night) * temp + 180;
				}
				else
				{
					defaultRotation = (180 / dayPartInSeconds) * (currentTime - sunrise);
				}
			}
			else
			{
				defaultRotation = WeatherConfig.degreesPerSecond * second;
			}
			
		}
		else
		{
			second = date.Second + (date.Minute * 60) + (date.Hour * 3600);
			
			currentTime = second / dayRatio;
			if (WeatherConfig.DayLongerThanNight != 1)
			{
				float temp = currentTime;
				float night = gameDayInSeconds - dayPartInSeconds;
				defaultRotation = (90 / (night/2)) * temp + 270;
			}
			else
			{
				//270 is 3/4 of whole circle, we are starting at 0 but it is not midnight
				defaultRotation = WeatherConfig.degreesPerSecond * second + 270;
			}
			
		}
		currentAngle = defaultRotation;
		transform.Rotate(new Vector3(defaultRotation, 0, 0));
		if (currentAngle > 180)
		{
			blender = 0;
		}
		else
		{
			blender = 1;
		}
		RenderSettings.skybox.SetFloat("_Blend", blender);
	}
	
	private void CalculateDayLength()
	{
		if (WeatherConfig.DayLongerThanNight == 1)
		{
			//all rotations are same
			dayRotation = nightRotation = rotation;
			//seting sunset
			sunset = sunrise + (gameDayInSeconds / 2);
			return;
		}
		
		dayPartInSeconds = (gameDayInSeconds / 2) * (WeatherConfig.DayLongerThanNight);
		//sunrise is different if day is longer than night
		sunrise = (gameDayInSeconds - dayPartInSeconds) / 2;
		//seting sunset
		sunset = sunrise + dayPartInSeconds;
		dayRotation = 180 / dayPartInSeconds;
		
		float night = gameDayInSeconds - dayPartInSeconds;
		nightRotation = 180 / night;
	}
	
	// Use this for initialization
	void Start () {
		DefaultInit();
		
		flare = this.GetComponent<LensFlare>();
		
		if (flare != null)
			storeFlare = flare.flare;
	}
	
	void OnGUI()
	{
		/*Rect rect = new Rect(20, 300, 500,25);
		GUI.Label(rect, "Rotation.y " + transform.rotation.y + " rotation.x " + transform.rotation.x);*/
	}
	
	// Update is called once per frame
	void Update () {
		if (isLoaded)
		{
			DefaultInit();
			isLoaded = false;
		}
		
		currentTime += Time.deltaTime;
		//determine how fast will rotate during day or night (below 180 degrees is day)
		if (currentAngle < 180)
		{
			DayState = DayCycleTypeEnum.Day;
			rotation = dayRotation;
		}
		else
		{
			DayState = DayCycleTypeEnum.Night;
			rotation = nightRotation;
		}
		
		//rotation sun
		transform.Rotate(new Vector3(rotation, 0, 0) * Time.deltaTime);
		//angle counting
		currentAngle += rotation * Time.deltaTime;
		
		if (currentTime > gameDayInSeconds)
			currentTime -= gameDayInSeconds;
		
		if (currentAngle > 360)
			currentAngle -= 360;
		
		if (blender + blenderIncrement < 1 && currentTime > sunrise && currentTime < sunset)
		{
			blender += blenderIncrement;
			DayState = DayCycleTypeEnum.Sunrise;
			if (RenderSettings.skybox != null)
				RenderSettings.skybox.SetFloat("_Blend", blender);
		}
		if (blender - blenderIncrement > 0 && currentTime > sunset)
		{
			blender -= blenderIncrement;
			DayState = DayCycleTypeEnum.Sunset;
			if (RenderSettings.skybox != null)
				RenderSettings.skybox.SetFloat("_Blend", blender);
		}
		if (flare != null && RenderSettings.fogDensity > 0.1f)
			flare.flare = null;
		else if (flare != null)
			flare.flare = storeFlare;
		
		if (blender > 1)
			blender = 1;
		if (blender < 0)
			blender = 0;
	}
	
	
}

public enum DayCycleTypeEnum
{
	Night = 0,
	Sunrise = 1,
	Day = 2,
	Sunset = 3
}

