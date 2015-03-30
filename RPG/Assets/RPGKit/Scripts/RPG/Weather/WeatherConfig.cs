using UnityEngine;
using System.Collections;

public class WeatherConfig
{
	//sunset is calculated according day rotation and sunrise and day modifier
	public static int sunRise = 6;
	
	public static float gameDayInMinutes = 1;
	
	//how long will take change day to night or night to day
	public static float blendingTimeInMinutes = 60;
	
	//modifier how is day longer than night
	public static float DayLongerThanNight = 1;
	
	public const float const_second = 1;
	public const float const_minute = 60 * const_second;
	public const float const_hour = 60 * const_minute;
	public const float const_day = 24 * const_hour;
	public const float degreesPerSecond = 360 / const_day;
	
	//start game time
	public const int StartGameDay = 7;
	public const int StartMonth = 8;
	public const int StartHour = 10;
	public const int StartMinute = 0;
	public const int StartYear = 2011;
	
	public const int MaximumForecast = 2;
	
	//rain		
	public const bool IsRain = true;
	public const int ChanceToRain = 10;
	public const int ChanceToContinueRain = 20;
	public const int MaximumRainIntensity = 2000;
	public const int MinimumRainIntensity = 1000;
	public const int MinimumRainParticles = 100;
	public const int RainChangeTime = 30;
	
	//snow
	public const bool IsSnow = false;
	public const int ChanceToSnow = 20;
	public const int ChanceToContinueSnow = 30;
	public const int MaximumSnowIntensity = 2000;
	public const int MinimumSnowIntensity = 1000;
	public const int MinimumSnowParticles = 100;
	public const int SnowChangeTime = 30;
	
	//fog
	public const bool IsFog = true;
	public const int ChanceToFog = 10;
	public const int ChanceToContinueFog = 60;
	public const int FogStartHour = 4;
	public const int FogEndHour = 10;
	public const int MaximumFogIntensity = 15;
	public const int MinimumFogIntensity = 5;
	public const int FogChangeTime = 30;
	public static Color dayFogColor = new Color(0.5f, 0.5f, 0.5f, 1);
	public static Color nightFogColor = new Color(0.5f, 0.5f, 0.5f, 1);
}
