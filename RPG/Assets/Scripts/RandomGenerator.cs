using UnityEngine;
using System.Collections;

public class RandomGenerator : Singleton<RandomGenerator>
{

    public void Init()
    {

    }

    public int GetRandomInt(int min, int max)
    {
        int result = Random.Range(min,max);
        return result;
    }
	
}
