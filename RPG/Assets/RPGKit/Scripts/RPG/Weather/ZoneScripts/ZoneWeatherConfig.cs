using UnityEngine;
using System.Collections;

public class ZoneWeatherConfig
{
	public int SceneId;
	
	public bool IsPossibleRain;
	public bool IsPossibleFog;
	public bool IsPossibleSnow;
	
	//rain		
	public int ChanceToRain;
	public int ChanceToContinueRain;
	public int MaximumRainIntensity;
	public int MinimumRainIntensity;
	public int MinimumRainParticles;
	public int RainChangeTime;
	
	//fog
	public int ChanceToFog;
	public int ChanceToContinueFog;
	public int FogStartHour;
	public int FogEndHour;
	public int MaximumFogIntensity;
	public int MinimumFogIntensity;
	public int FogChangeTime;
	public Color DayFogColor;
	public Color NightFogColor;
	
	//snow
	public int ChanceToSnow;
	public int ChanceToContinueSnow;
	public int MaximumSnowIntensity;
	public int MinimumSnowIntensity;
	public int MinimumSnowParticles;
	public int SnowChangeTime;
}
