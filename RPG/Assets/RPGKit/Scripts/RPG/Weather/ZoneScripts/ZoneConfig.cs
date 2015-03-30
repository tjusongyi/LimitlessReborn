using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneConfig
{
	public List<ZoneWeatherConfig> Zones;
	public int SceneId;
	
	public ZoneConfig()
	{
		Zones = new List<ZoneWeatherConfig>();
		
		ZoneSettings.FillZoneConfig(Zones);
	}
	
	
	public bool IsPossibleRain
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
				{
					return z.IsPossibleRain;
				}
			}
			return WeatherConfig.IsRain;
		}
	}
	
	public bool IsPossibleFog
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.IsPossibleFog;
			}
			return WeatherConfig.IsFog;
		}
	}
	
	public bool IsPossibleSnow
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.IsPossibleSnow;
			}
			return WeatherConfig.IsSnow;
		}
	}
	
	//rain		
	public int ChanceToRain
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
				{
					return z.ChanceToRain;
				}
			}
			return WeatherConfig.ChanceToRain;
		}
	}
	
	public int ChanceToContinueRain
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.ChanceToRain;
			}
			return WeatherConfig.ChanceToRain;
		}
	}
	
	public int MaximumRainIntensity
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MaximumRainIntensity;
			}
			return WeatherConfig.MaximumRainIntensity;
		}
	}
	
	public int MinimumRainIntensity
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MaximumRainIntensity;
			}
			return WeatherConfig.MinimumRainIntensity;
		}
	}
		
	public int MinimumRainParticles
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MinimumRainParticles;
			}
			return WeatherConfig.MinimumRainParticles;
		}
	}
	
	public int RainChangeTime
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.RainChangeTime;
			}
			return WeatherConfig.RainChangeTime;
		}
	}
	
	//fog
	public int ChanceToFog
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.ChanceToFog;
			}
			return WeatherConfig.ChanceToFog;
		}
	}
	
	public int ChanceToContinueFog
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.ChanceToContinueFog;
			}
			return WeatherConfig.ChanceToContinueFog;
		}
	}
	
	public int FogStartHour
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.FogStartHour;
			}
			return WeatherConfig.FogStartHour;
		}
	}
		
	public int FogEndHour
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.FogEndHour;
			}
			return WeatherConfig.FogEndHour;
		}
	}
	
	public int MaximumFogIntensity
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MaximumFogIntensity;
			}
			return WeatherConfig.MaximumFogIntensity;
		}
	}	
	
	public int MinimumFogIntensity
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MinimumFogIntensity;
			}
			return WeatherConfig.MinimumFogIntensity;
		}
	}	
	
	public int FogChangeTime
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.FogChangeTime;
			}
			return WeatherConfig.FogChangeTime;
		}
	}	
	
	public Color DayFogColor
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.DayFogColor;
			}
			return WeatherConfig.dayFogColor;
		}
	}	
	
	public Color NightFogColor
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.NightFogColor;
			}
			return WeatherConfig.nightFogColor;
		}
	}	
	
	//snow
	public int ChanceToSnow
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.ChanceToSnow;
			}
			return WeatherConfig.ChanceToSnow;
		}
	}	
	
	public int ChanceToContinueSnow
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.ChanceToContinueSnow;
			}
			return WeatherConfig.ChanceToContinueSnow;
		}
	}	
	
	public int MaximumSnowIntensity
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MaximumSnowIntensity;
			}
			return WeatherConfig.MaximumSnowIntensity;
		}
	}	
	
	public int MinimumSnowIntensity
	{
	get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MinimumSnowIntensity;
			}
			return WeatherConfig.MinimumSnowIntensity;
		}
	}	
	
	public int MinimumSnowParticles
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.MinimumSnowParticles;
			}
			return WeatherConfig.MinimumSnowParticles;
		}
	}	
	
	public int SnowChangeTime
	{
		get
		{
			foreach(ZoneWeatherConfig z in Zones)
			{
				if (SceneId == z.SceneId)
					return z.SnowChangeTime;
			}
			return WeatherConfig.SnowChangeTime;
		}
	}	
}
