using UnityEngine;
using UnityEditor;
using System.Collections;



public class DungeonEditor : EditorWindow {
	
	private int sizeOfCoridor = 10;
	
	private int dungeonX = 24;
	private int dungeonY = 24;
	private int startX = 12;
	private int startY = 0;
	private bool allowAdjacent = false;
	private int direction = 50;
	private int sparseness = 50;
	private int cycleParameter = 50;
	
	private Texture2D FloorTexture;
	private Texture2D WallTexture;

	[MenuItem ("Window/Dungeon Toolkit")]
	static void Init()
	{
		DungeonEditor window = (DungeonEditor)EditorWindow.GetWindow(typeof(DungeonEditor));
		 window.position = new Rect(200, 50, 400, 440); 
	}
	
	void OnGUI()
	{
		
		#region editor controls
		
		EditorGUIUtility.LookLikeControls();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Size X");
		dungeonX = (int) EditorGUILayout.Slider(dungeonX, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Size Z");
		dungeonY = (int) EditorGUILayout.Slider(dungeonY, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Start X");
		startX = (int) EditorGUILayout.Slider(startX, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Start Y");
		startY = (int) EditorGUILayout.Slider(startY, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Thin walls");
		allowAdjacent =  EditorGUILayout.Toggle(allowAdjacent);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Sparseness");
		sparseness = (int) EditorGUILayout.Slider(sparseness, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Direction");
		direction = (int) EditorGUILayout.Slider(direction, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("No cycle");
		cycleParameter = (int) EditorGUILayout.Slider(cycleParameter, 0, 100);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		FloorTexture = (Texture2D)EditorGUILayout.ObjectField(FloorTexture, typeof(Texture2D), true);
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		WallTexture = (Texture2D)EditorGUILayout.ObjectField(WallTexture, typeof(Texture2D), true);
		EditorGUILayout.EndHorizontal();
		
		#endregion
		
		if (GUILayout.Button("Generate dungeon", GUILayout.Width(300))) 
		{
			//place floor
			GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
			floor.name = "floor";
        	floor.transform.position = new Vector3((dungeonX/2) * sizeOfCoridor,0, (dungeonY/2) * sizeOfCoridor); 
			Vector3 sizeOfTerrain = new Vector3(dungeonX, 0, dungeonY);
			floor.transform.localScale += sizeOfTerrain; 
			if (FloorTexture != null)
				floor.renderer.material.mainTexture = FloorTexture;
			
			Dungeon map = Generator.Generate(dungeonX, dungeonY, startX, startY, allowAdjacent, direction, sparseness, cycleParameter);
			
			
			for (int x = 0; x < map.Width; x++)
			{
				for (int y = 0; y < map.Height; y++)
				{
					Cell cell = map[x, y];
					if (cell.Visited)
						continue;
					GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
					wall.name = "wall";
					wall.transform.position = new Vector3(x * sizeOfCoridor + 5, 5, y * sizeOfCoridor+ 5);
					wall.transform.localScale = new Vector3(sizeOfCoridor,sizeOfCoridor, sizeOfCoridor);
					if (WallTexture != null)
						wall.renderer.material.mainTexture = WallTexture;
					wall.transform.parent = floor.transform;
				}
			} 
		}
		
	}
}
