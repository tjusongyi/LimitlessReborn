using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneSettings 
{
	public static void FillZoneConfig(List<ZoneWeatherConfig> zones)
	{
		ZoneWeatherConfig zone;
		
		/* copy this code - begining*/
		zone = new ZoneWeatherConfig();
		
		zone.SceneId = 1;
		
		zone.IsPossibleFog = true;
		zone.IsPossibleRain = false;
		zone.IsPossibleSnow = true;
	
		//rain		
		zone.ChanceToRain = 0;
		zone.ChanceToContinueRain = 100;
		zone.MaximumRainIntensity = 2000;
		zone.MinimumRainIntensity = 1000;
		zone.MinimumRainParticles = 100;
		zone.RainChangeTime = 30;
	
		//fog
		zone.ChanceToFog = 20;
		zone.ChanceToContinueFog = 15;
		zone.FogStartHour = 4;
		zone.FogEndHour = 10;
		zone.MaximumFogIntensity = 3;
		zone.MinimumFogIntensity = 15;
		zone.FogChangeTime = 30;
		zone.DayFogColor = new Color(0.5f, 0.5f, 0.5f, 1);
		zone.NightFogColor = new Color(0.5f, 0.5f, 0.5f, 1);
	
		//snow
		zone.ChanceToSnow = 0;
		zone.ChanceToContinueSnow = 80;
		zone.MaximumSnowIntensity = 4000;
		zone.MinimumSnowIntensity = 1000;
		zone.MinimumSnowParticles = 400;
		zone.SnowChangeTime = 30;
		
		zones.Add(zone);
		/* end of copy */
	}
}
