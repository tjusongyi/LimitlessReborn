using UnityEngine;
using System.Collections;

public class Container : BaseGameObject 
{
	public RPGContainer container;
	public int ID;
    public string CloseAnimation = "Close";
    public string OpenAnimation = "Open";
	
	bool opened;
    bool opening = false;

    private float TimerOpened;
    private Player player;

    private bool CanOpen;

	// Use this for initialization
	void Start () 
	{
		container = new RPGContainer();
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		if (ID == 0)
			return;
        player = go.GetComponent<Player>();
		container = Storage.LoadById<RPGContainer>(ID, container);
        
        container.Initialize(player);
        CanOpen = container.CanOpen(player);
	}

    void Update()
    {
        if (BasicGUI.isMainMenuDisplayed)
            return;

        if (container == null)
            return;
        CanOpen = container.CanOpen(player);
        if (BasicGUI.isContainerDisplayed && player.rpgContainer.ID != this.ID)
            return;

        if (opening)
        {
            TimerOpened += Time.deltaTime;
            if (!opened && animation != null && animation.clip != null && TimerOpened > animation.clip.length)
            {
                player.ChangeMouseControl(false);
                opened = true;
                Player.Container = ContainerType.Container;
                player.rpgContainer = container;
                opening = false;
                TimerOpened = 0;
                BasicGUI.isContainerDisplayed = true;
                container.DoEvents(player);
            }
        }

        if (opened && !BasicGUI.isContainerDisplayed)
            CloseContainer();
    }

    public override string DisplayCloseInfo(Player player)
    {
        CanOpen = container.CanOpen(player);
        if (!CanOpen)
            return string.Empty;

        return container.Name + " - press E (open " + container.Name + ")";
    }

    public override string DisplayInfo(Player player)
    {
        CanOpen = container.CanOpen(player);
        if (!CanOpen)
            return string.Empty;

        return container.Name;
    }

    public override float GetActivateRange(Player player)
    {
        return player.Hero.Settings.ObjectActivateRange + 1;
    }

    public override void DoAction(Player player)
    {
        if (opened || TimerOpened > 0)
            return;

        if (!container.CanOpen(player))
            return;

        if (container.IsEffect(player))
        {

            foreach (Effect e in container.OpenEffects)
            {
                player.Hero.Buffs.AddEffect(e);
            }
        }


        OpenContainer(player);
    }

    public void CloseContainer()
	{
        if (!CanOpen)
            return;

        if (animation != null && animation[CloseAnimation] != null)
		{
            animation.Play(CloseAnimation);
			animation.wrapMode = WrapMode.Clamp;
			if (audio != null)
			{
				audio.PlayOneShot(audio.clip);
			}
			opened = false;
            player.ChangeMouseControl(true);
            //if container allow to destroy empty, remove it from game
            if (container.DestroyEmpty && container.Items.Count == 0)
                Destroy(gameObject);
            new WaitForSeconds(animation.clip.length);
		}
	}

    void OpenContainer(Player player)
	{
        if (animation != null && animation[OpenAnimation] != null)
        {
            opened = false;
            animation.Play(OpenAnimation);
            animation.wrapMode = WrapMode.Clamp;
            if (audio != null)
            {
                audio.PlayOneShot(audio.clip);
            }
            opening = true;
            TimerOpened = 0;
        }
        else
        {
            opened = true;
            Player.Container = ContainerType.Container;
            player.rpgContainer = container;
            player.ChangeMouseControl(false);
            BasicGUI.isContainerDisplayed = true;
        }
	}
}
