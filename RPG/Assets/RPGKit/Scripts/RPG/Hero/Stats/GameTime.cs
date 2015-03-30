using System;
using System.Collections;

public class GameTime
{
	public DateTime CurrentGameTime;
	public float CurrentBlender;
	public float RainCurrentMin;
	public float RainCurrentMax;
	public float FogIntensity;
	
	public void FillGameObjects()
	{
		Weather.CurrentDateTime = CurrentGameTime;
		DayCycle.blender = CurrentBlender;
        if (Weather.rainComponent != null)
        {
            Weather.rainComponent.maxEmission = RainCurrentMax;
            Weather.rainComponent.minEmission = RainCurrentMin;
        }
		DayCycle.isLoaded = true;
	}
	
	public void Store(float fog)
	{
		CurrentBlender = DayCycle.blender;
		CurrentGameTime = Weather.CurrentDateTime;
		FogIntensity = fog;
        if (Weather.rainComponent != null)
        {
            RainCurrentMax = Weather.rainComponent.maxEmission;
            RainCurrentMin = Weather.rainComponent.minEmission;
        }
	}
}
