using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (FPSCharacterMotor))]
public class PlayerController : MonoBehaviour 
{
    public Player player;
    public bool IsMobileControl;
	private FPSCharacterMotor motor;
    public Texture2D AttackButton;
    public Texture2D AttackButtonCD;
    public BaseGameObject selectedObject;

    private float timeInterval = 0;

	void Awake () 
	{
		motor = GetComponent<FPSCharacterMotor>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (player.scene == null || player.scene.SceneType == SceneTypeEnum.MainMenu || !BasicGUI.IsAllWindowsClosed || BasicGUI.isNoteDisplayed)
            return;

        if (Player.IsLoading)
            return;

        timeInterval += Time.deltaTime;
        //raycast
        RaycastHit vHit = new RaycastHit();
        
        Ray vRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + 0.1f));
        
        if (Physics.Raycast(vRay, out vHit, 200))
        {
            selectedObject = vHit.transform.gameObject.GetComponent<BaseGameObject>();
        }
        
        motor.v = Input.GetAxis("Vertical");
        motor.h = Input.GetAxis("Horizontal");

        if (selectedObject != null && Input.GetKey(KeyCode.E))
        {
            ActivateButton();
        }
    }


    public void ActivateButton()
    {
        float distance = Vector3.Distance(transform.position, selectedObject.transform.position);

        if (distance > selectedObject.GetActivateRange(player))
            return;
        timeInterval = 0;
        selectedObject.DoAction(player);

        if (selectedObject is NPC)
        {
            NPCGUI.MenuMode = NPCMenuType.Conversation;
            player.ChangeMouseControl(false);
        }
    }

    private bool IsInControlRect(Rect rect, Vector2 position)
    {
        position.y = Screen.height - position.y;

        if (rect.Contains(position))
            return true;

        return false;
    }
}
