/// <summary>
/// Class type.
/// This is a main script of hero use to control hero
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ClassType { None, Swordman, Archer, Mage }

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationManager))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(PlayerSkill))]
public class HeroController : MonoBehaviour
{

    public enum ControlAnimationState { Idle, Move, WaitAttack, Attack, Cast, ActiveSkill, TakeAtk, Death }; //Hero state
    public ClassType classType; //Class hero
    public GameObject target;     //Target enemy
    public GameObject targetHP;  //Target show hp
    [SerializeField]
    public List<GameObject> modelMesh;      //Model to chage color if take attack
    public Color colorTakeDamage;		//Color take damage


    public ControlAnimationState ctrlAnimState; //Control Animation State

    public bool autoAttack;

    //Get component other script

    private AnimationManager animationManager;
    private PlayerStatus playerStatus;
    private PlayerSkill playerSkill;

    private float delayAttack = 100;		//Delay Attack speed
    private Vector3 destinationPosition;		// The destination Point
    private float destinationDistance;			// The distance between this.transform and destinationPosition
    private Vector3 movedir;
    private CharacterController controller;
    private float moveSpeed;						// The Speed the character will move
    private Vector3 ctargetPos;					//Convert Target Position
    private Vector3 targetPos;					//Target Pos
    private Quaternion targetRotation;			//Rotation]
    private bool checkCritical;					//Check Critical
    private bool onceAttack;					//Check Attack if disable AutoAttack
    private float flinchValue;					//Check Enemy flinch
    private Color[] defaultColor;				//Default Material Color
    private bool getSkillTarget;                //Check Get Skill Target
    private bool alreadyLockSkill;				//Check lock freeskill


    public bool useSkill;                    //Check use skill

    public bool useFreeSkill;                    //Check use Free Target skill

    public Vector3 freePosSkill;				//Position Skill

    [HideInInspector]
    public float skillRange;                 //Skill Range Detect
    [HideInInspector]
    public int castid;						//Cast skill id
    [HideInInspector]
    public GameObject DeadSpawnPoint;     //Spawn point when hero dead
    [HideInInspector]
    public int typeAttack;						//Type Attack
    [HideInInspector]
    public int typeTakeAttack;			//Type TakeAttack

    public bool dontMove;
    public bool dontClick;

    private bool oneShotOpenDeadWindow;

    private Vector3 input;

    public int layerActiveGround = 11;
    public int layerActiveItem = 10;
    public int layerActiveEnemy = 9;


    //Editor Variable
    [HideInInspector]
    public int sizeMesh;


    // Use this for initialization
    void Start()
    {

        layerActiveGround = 11;
        layerActiveItem = 10;
        layerActiveEnemy = 9;

        destinationPosition = this.transform.position;
        animationManager = this.GetComponent<AnimationManager>();
        playerSkill = this.GetComponent<PlayerSkill>();
        playerStatus = this.GetComponent<PlayerStatus>();
        controller = this.GetComponent<CharacterController>();

        flinchValue = 100; //Declare flinch value (if zero it will flinch)
        delayAttack = 100; //Declare delay 100 sec

        defaultColor = new Color[modelMesh.Count];

        DeadSpawnPoint = GameObject.FindGameObjectWithTag("SpawnHero");

        SetDefualtColor();

        input = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {

        TargetLock();
        HeroAnimationState();


        if (ctrlAnimState != ControlAnimationState.Death && ctrlAnimState != ControlAnimationState.Cast && ctrlAnimState != ControlAnimationState.ActiveSkill && dontMove == false)
        {
            ClickToMove();
            //KeyBoardToMove();
            CancelSkill();
        }
        else if (dontMove == true)
        {
            ctrlAnimState = ControlAnimationState.Idle;
        }

    }

    void CancelSkill()
    {
        //Ray to enemy
        Ray r = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit h;

        if (Input.GetMouseButtonDown(1) && useSkill && ctrlAnimState != ControlAnimationState.Death)
        {
            playerSkill.oneShotResetTarget = false;
            useFreeSkill = false;
            useSkill = false;
            GameSetting.Instance.SetMouseCursor(0);
            castid = 0;
            skillRange = 0;
        }

        if (Physics.Raycast(r, out h, 100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
        {

            if (h.collider != null)
            {
                if (h.collider.tag == "Ground" && !useFreeSkill)
                {

                    if (Input.GetMouseButtonDown(0) && useSkill && ctrlAnimState != ControlAnimationState.Death)
                    {
                        playerSkill.oneShotResetTarget = false;
                        useSkill = false;
                        GameSetting.Instance.SetMouseCursor(0);
                        castid = 0;
                        skillRange = 0;
                    }
                }

                if (h.collider.tag == "Enemy")
                {

                    if (Input.GetMouseButtonDown(0) && useSkill && ctrlAnimState != ControlAnimationState.Death)
                    {
                        GameSetting.Instance.SetMouseCursor(0);
                    }
                }
            }
        }

    }


    //State Hero
    void HeroAnimationState()
    {

        if (ctrlAnimState == ControlAnimationState.Idle)
        {
            animationManager.animationState = animationManager.Idle;
        }

        if (ctrlAnimState == ControlAnimationState.Move)
        {
            animationManager.animationState = animationManager.Move;
        }

        if (ctrlAnimState == ControlAnimationState.WaitAttack)
        {
            animationManager.animationState = animationManager.Idle;
            WaitAttack();

        }
        if (ctrlAnimState == ControlAnimationState.Attack)
        {
            if (target)
            {
                LookAtTarget(target.transform.position);

                if (checkCritical)
                {
                    animationManager.animationState = animationManager.CriticalAttack;
                    delayAttack = 100;
                    onceAttack = false;
                }
                else if (!checkCritical)
                {
                    animationManager.animationState = animationManager.Attack;
                    delayAttack = 100;
                    onceAttack = false;
                }
            }
            else
            {
                ctrlAnimState = ControlAnimationState.Idle;
            }


        }


        if (ctrlAnimState == ControlAnimationState.TakeAtk)
        {
            animationManager.animationState = animationManager.TakeAttack;

        }

        if (ctrlAnimState == ControlAnimationState.Cast)
        {
            playerSkill.CastSkill(playerSkill.FindSkillType(castid), playerSkill.FindSkillIndex(castid));

            animationManager.animationState = animationManager.Cast;
        }

        if (ctrlAnimState == ControlAnimationState.ActiveSkill)
        {
            animationManager.animationState = animationManager.ActiveSkill;
        }

        if (ctrlAnimState == ControlAnimationState.Death)
        {
            animationManager.animationState = animationManager.Death;
        }
    }

    //Wait before attack
    void WaitAttack()
    {
        if (delayAttack > 0)
        {
            delayAttack -= Time.deltaTime * playerStatus.statusCal.atkSpd;
        }
        else if (delayAttack <= 0)
        {
            checkCritical = CriticalCal(playerStatus.statusCal.criticalRate);

            if (checkCritical)
            {
                typeAttack = Random.Range(0, animationManager.criticalAttack.Count);
                animationManager.checkAttack = false;
            }
            else if (!checkCritical)
            {
                typeAttack = Random.Range(0, animationManager.normalAttack.Count);
                animationManager.checkAttack = false;
            }

            if (autoAttack)
            {
                ctrlAnimState = ControlAnimationState.Attack;
            }
            else
            {
                if (onceAttack)
                {
                    ctrlAnimState = ControlAnimationState.Attack;
                }
            }

        }
    }

    void TargetLock()
    {
        //Ray to enemy
        Ray r = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit h;


        if (target == null)
        {
            if (Physics.Raycast(r, out h, 100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
            {

                if (h.collider != null)
                {
                    if (h.collider.tag == "Enemy")
                    {
                        targetHP = h.collider.gameObject;

                        if (!useSkill)
                            GameSetting.Instance.SetMouseCursor(1);

                    }
                    else if (h.collider.tag == "Ground")
                    {
                        targetHP = null;

                        if (!useSkill)
                            GameSetting.Instance.SetMouseCursor(0);

                    }
                    else if (h.collider.tag == "Npc_Shop")
                    {
                        targetHP = null;

                        if (!useSkill)
                            GameSetting.Instance.SetMouseCursor(4);

                    }
                    else if (h.collider.tag == "Item")
                    {
                        targetHP = null;

                        if (!useSkill)
                            GameSetting.Instance.SetMouseCursor(5);

                    }
                }
            }

        }
        else
        {

            if (Physics.Raycast(r, out h, 100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
            {

                if (h.collider != null)
                {
                    if (h.collider.tag == "Ground")
                    {
                        if (!useSkill)
                            GameSetting.Instance.SetMouseCursor(0);
                    }

                    if (h.collider.tag == "Enemy")
                    {
                        if (!useSkill)
                            GameSetting.Instance.SetMouseCursor(1);
                    }
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(r, out h, 100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
                {

                    if (h.collider != null)
                    {
                        if (h.collider.tag == "Enemy")
                        {
                            targetHP = h.collider.gameObject;


                        }

                        if (h.collider.tag == "Ground")
                        {
                            targetHP = null;

                        }
                    }
                }
            }

        }

        //Show enemy hp bar
        if (targetHP)
        {
            EnemyStatus enemyStatus;
            EnemyController enemyControl;
            enemyStatus = targetHP.GetComponent<EnemyStatus>();
            enemyControl = targetHP.GetComponent<EnemyController>();

            EnemyHP.Instance.ShowHPbar(true);
            EnemyHP.Instance.GetHPTarget(enemyControl.defaultHP, enemyStatus.status.hp, enemyStatus.enemyName);

        }
        else if (!targetHP)
        {
            EnemyHP.Instance.ShowHPbar(false);
        }
    }


    void KeyBoardToMove()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(input.x, 0, input.y);
        if (dir.sqrMagnitude > 0.2)
        {
            ctrlAnimState = ControlAnimationState.Move;
            moveSpeed = playerStatus.statusCal.movespd;
            movedir = transform.TransformDirection(dir * moveSpeed);
            controller.Move(movedir * Time.deltaTime);
        }
        else
        {

        }


    }

    //Movement Method
    void ClickToMove()
    {
        if (useFreeSkill && useSkill && getSkillTarget)
        {
            destinationDistance = Vector3.Distance(destinationPosition, this.transform.position); //Check Distance Player to Destination Point

            if (destinationDistance < skillRange)
            {		// Reset speed to 0

                //Change to state Cast

                if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
                {
                    ctrlAnimState = ControlAnimationState.Cast;
                    playerSkill.canCast = true;
                    getSkillTarget = false;
                }

                LookAtTarget(freePosSkill);
                moveSpeed = 0;
            }
            else if (destinationDistance > skillRange)
            {			//Reset Speed to default
                //Change to state move
                if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
                    ctrlAnimState = ControlAnimationState.Move;
                moveSpeed = playerStatus.statusCal.movespd;
            }
        }
        else
        {
            if (target != null && !useSkill) //Click Enemy
            {
                destinationDistance = Vector3.Distance(target.transform.position, this.transform.position); //Check Distance Player to Destination Point

                if (destinationDistance <= playerStatus.statusCal.atkRange)
                {		// Reset speed to 0

                    //Change to state Idle
                    if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
                        ctrlAnimState = ControlAnimationState.WaitAttack;
                    moveSpeed = 0;

                    LookAtTarget(target.transform.position);
                }
                else if (destinationDistance > playerStatus.statusCal.atkRange)
                {			//Reset Speed to default
                    //Change to state move
                    LookAtTarget(target.transform.position);
                    if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.WaitAttack)
                        ctrlAnimState = ControlAnimationState.Move;
                    moveSpeed = playerStatus.statusCal.movespd;
                }


            }
            else

                if (target != null && useSkill) //Click Enemy
                {
                    destinationDistance = Vector3.Distance(target.transform.position, this.transform.position); //Check Distance Player to Destination Point

                    if (destinationDistance <= skillRange)
                    {		// Reset speed to 0

                        //Change to state Cast

                        if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
                        {
                            ctrlAnimState = ControlAnimationState.Cast;
                            playerSkill.canCast = true;
                        }

                        LookAtTarget(target.transform.position);
                        moveSpeed = 0;
                    }
                    else if (destinationDistance > skillRange)
                    {			//Reset Speed to default
                        //Change to state move
                        LookAtTarget(target.transform.position);
                        if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle || ctrlAnimState == ControlAnimationState.WaitAttack)
                            ctrlAnimState = ControlAnimationState.Move;
                        moveSpeed = playerStatus.statusCal.movespd;
                    }


                }
                else

                    if (target == null) // Click Ground
                    {
                        destinationDistance = Vector3.Distance(destinationPosition, this.transform.position); //Check Distance Player to Destination Point

                        if (destinationDistance < .5f)
                        {		// Reset speed to 0

                            //Change to state Idle
                            if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
                                ctrlAnimState = ControlAnimationState.Idle;
                            moveSpeed = 0;
                        }
                        else if (destinationDistance > .5f)
                        {			//Reset Speed to default
                            //Change to state move
                            if (ctrlAnimState == ControlAnimationState.Move || ctrlAnimState == ControlAnimationState.Idle)
                                ctrlAnimState = ControlAnimationState.Move;
                            moveSpeed = playerStatus.statusCal.movespd;
                        }
                    }
        }



        destinationDistance = Vector3.Distance(destinationPosition, this.transform.position);

        // Moves the Player if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0 && dontClick == false)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitdist;


            //if disable auto attack it can attack 1 time
            if (!autoAttack)
            {
                onceAttack = true;
            }


            if (Physics.Raycast(ray, out hitdist, 100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
            {

                if (hitdist.collider.tag != "Player")
                {
                    Vector3 targetPoint = Vector3.zero;
                    targetPoint.x = hitdist.point.x;
                    targetPoint.y = transform.position.y;
                    targetPoint.z = hitdist.point.z;
                    destinationPosition = hitdist.point;
                    targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                    if (alreadyLockSkill)
                    {
                        playerSkill.oneShotResetTarget = false;
                        ResetOldCast();
                        useFreeSkill = false;
                        useSkill = false;
                        getSkillTarget = false;
                        alreadyLockSkill = false;
                    }

                    if (useFreeSkill && !alreadyLockSkill)
                    {
                        freePosSkill = destinationPosition;
                        getSkillTarget = true;
                        alreadyLockSkill = true;
                    }

                }
            }

            Ray r = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit h;
            if (Physics.Raycast(r, out h, 100, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
            {
                if (h.collider.tag == "Ground")
                {
                    //Reset Lock Target
                    if (ctrlAnimState != ControlAnimationState.Attack)
                        target = null;
                    //Spawn Mouse Effect
                    Instantiate(GameSetting.Instance.mousefxNormal, new Vector3(h.point.x, h.point.y + 0.02f, h.point.z), h.collider.transform.rotation);


                }
                else if (h.collider.tag == "Enemy")
                {

                    if (ctrlAnimState != ControlAnimationState.Attack)
                        target = h.collider.gameObject;
                    //Spawn Mouse Effect
                    GameObject go = (GameObject)Instantiate(GameSetting.Instance.mousefxAttack, new Vector3(h.collider.transform.position.x, h.collider.transform.position.y + 0.02f, h.collider.transform.position.z), Quaternion.identity);
                    go.transform.parent = target.transform;


                }
            }

        }


        // Moves the player if the mouse button is hold down
        else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0 && dontClick == false)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitdist;

            if (Physics.Raycast(ray, out hitdist, 1 << layerActiveEnemy | 1 << layerActiveGround | 1 << layerActiveItem))
            {
                if (hitdist.collider.tag != "Player")
                {
                    Vector3 targetPoint = Vector3.zero;//hitdist.point;
                    targetPoint.x = hitdist.point.x;
                    targetPoint.y = transform.position.y;
                    targetPoint.z = hitdist.point.z;
                    destinationPosition = hitdist.point;

                    targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                }

            }
        }

        //Reset State when release left-click
        if (Input.GetMouseButtonUp(0))
        {
            if (ctrlAnimState != ControlAnimationState.Attack)
                ctrlAnimState = ControlAnimationState.Idle;
            moveSpeed = 0;
        }


        //Disable Auto Attack Command
        if (Input.GetMouseButton(0) && target && dontClick == false)
        {
            //if disable auto attack it can attack 1 time
            if (!autoAttack)
            {
                onceAttack = true;
            }
        }



        if (ctrlAnimState == ControlAnimationState.Move)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 25);

            if (controller.isGrounded)
            {
                movedir = Vector3.zero;
                movedir = transform.TransformDirection(Vector3.forward * moveSpeed);
            }
        }
        else
        {

            movedir = Vector3.Lerp(movedir, Vector3.zero, Time.deltaTime * 10);

        }
        movedir.y -= 20 * Time.deltaTime;
        controller.Move(movedir * Time.deltaTime);

    }

    //Look at target method
    void LookAtTarget(Vector3 _targetPos)
    {
        targetPos.x = _targetPos.x;
        targetPos.y = this.transform.position.y;
        targetPos.z = _targetPos.z;
        this.transform.LookAt(targetPos);
    }

    //Critical Calculate
    bool CriticalCal(float criticalStat)
    {
        float calCritical = criticalStat - Random.Range(0, 101f);

        if (calCritical > 0)
        {
            return true; //Critical
        }
        else
        {
            return false; //Not Critical
        }
    }

    //ResetState Method
    public void ResetState()
    {
        moveSpeed = 0;
        movedir = Vector3.zero;
        destinationDistance = 0;
        destinationPosition = this.transform.position;
        target = null;
        ctrlAnimState = ControlAnimationState.Idle;
        alreadyLockSkill = false;
        Invoke("resetCheckAttack", 0.1f);

    }

    public void ResetBeforeCast()
    {
        moveSpeed = 0;
        movedir = Vector3.zero;
        destinationDistance = 0;
        destinationPosition = this.transform.position;
        target = null;
        alreadyLockSkill = false;
        Invoke("resetCheckAttack", 0.1f);
    }

    void ResetMove()
    {
        moveSpeed = 0;
        movedir = Vector3.zero;
        destinationDistance = 0;
        destinationPosition = this.transform.position;
    }

    public void ResetAttack()
    {
        target = null;
    }

    public void DeadReset()
    {
        useSkill = false;
        GameSetting.Instance.SetMouseCursor(0);
        castid = 0;
        skillRange = 0;
        moveSpeed = 0;
        movedir = Vector3.zero;
        destinationDistance = 0;
        destinationPosition = this.transform.position;
        target = null;

        if (!oneShotOpenDeadWindow)
        {
            Invoke("OpenDeadWindow", 0.5f);
            oneShotOpenDeadWindow = true;
        }


    }

    void OpenDeadWindow()
    {
        if (!DeadWindow.enableWindow)
            DeadWindow.enableWindow = true;
    }

    void resetCheckAttack()
    {
        animationManager.checkAttack = false;
    }

    public void GetDamage(float targetAttack, float targetHit, float flinchRate, GameObject atkEffect, AudioClip atksfx)
    {
        //Calculate Hit
        targetHit += Random.Range(-10, 30);

        if (playerStatus.statusCal.spd - targetHit > 0) //Attack Miss
        {
            InitTextDamage(Color.white, "Miss");
            SoundManager.instance.PlayingSound("Attack_Miss");

        }
        else
        {
            int damage = Mathf.FloorToInt((targetAttack - playerStatus.statusCal.def) * Random.Range(0.8f, 1.2f));

            if (damage <= 5)
            {
                damage = Random.Range(1, 11); // if def < enemy attack
            }

            //Play SFX
            if (atksfx)
                AudioSource.PlayClipAtPoint(atksfx, transform.position);
            //Spawn Effect
            if (atkEffect)
                Instantiate(atkEffect, transform.position, Quaternion.identity);

            InitTextDamage(Color.red, damage.ToString());

            playerStatus.statusCal.hp -= damage;
            GetDamageColorReset();

            if (playerStatus.statusCal.hp <= 0)
            {
                playerStatus.statusCal.hp = 0;
                ctrlAnimState = ControlAnimationState.Death;
            }
            else
            {
                flinchValue -= flinchRate;

                if (flinchValue <= 0)
                {
                    if (ctrlAnimState == ControlAnimationState.Cast || ctrlAnimState == ControlAnimationState.ActiveSkill)
                        playerSkill.CastBreak();

                    ctrlAnimState = ControlAnimationState.TakeAtk;
                    flinchValue = 100;
                    playerSkill.oneShotResetTarget = false;
                }

            }
        }


    }

    public void InitTextDamage(Color colorText, string damageGet)
    {
        // Init text damage
        GameObject loadPref = (GameObject)Resources.Load("TextDamage");
        GameObject go = (GameObject)Instantiate(loadPref, transform.position + (Vector3.up * 1.0f), Quaternion.identity);
        go.GetComponentInChildren<TextDamage>().SetDamage(damageGet, colorText);
    }

    void GetDamageColorReset()
    {
        int index = 0;
        while (index < modelMesh.Count)
        {
            modelMesh[index].renderer.material.color = defaultColor[index];
            index++;
        }

        StartCoroutine(GetDamageColor(0.2f));
    }

    void SetDefualtColor()
    {
        int index = 0;
        while (index < modelMesh.Count)
        {
            defaultColor[index] = modelMesh[index].renderer.material.color;
            index++;
        }
    }

    private IEnumerator GetDamageColor(float time)
    {
        //if take damage material monster will change to setting color
        int index = 0;
        Color[] colorDef = new Color[modelMesh.Count];
        while (index < modelMesh.Count)
        {
            colorDef[index] = modelMesh[index].renderer.material.color;
            modelMesh[index].renderer.material.color = colorTakeDamage;
            index++;
        }
        yield return new WaitForSeconds(time);
        index = 0;
        while (index < modelMesh.Count)
        {
            modelMesh[index].renderer.material.color = colorDef[index];
            index++;
        }
        yield return 0;
        StopCoroutine("GetDamageColor");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Collider")
        {
            //Stop movement if collision with collider
            ResetMove();
        }
    }

    public void GetCastID(int caseID)
    {

        if (ctrlAnimState != ControlAnimationState.Cast && ctrlAnimState != ControlAnimationState.ActiveSkill && ctrlAnimState != ControlAnimationState.Death &&
            ctrlAnimState != ControlAnimationState.Attack)
        {
            castid = caseID;
            ctrlAnimState = ControlAnimationState.Cast;
        }

    }

    public void ResetOldCast()
    {
        useSkill = false;
        useFreeSkill = false;
        GameSetting.Instance.SetMouseCursor(0);
    }

    public void Reborn()
    {
        //Refil HP
        playerStatus.statusCal.hp = playerStatus.hpMax / 2;
        playerStatus.statusCal.mp = playerStatus.mpMax / 2;

        playerStatus.status.exp -= (playerStatus.status.exp / GameSetting.Instance.deadExpPenalty);
        if (playerStatus.status.exp < 0)
        {
            playerStatus.status.exp = 0;
        }

        playerStatus.StartRegen();

        transform.position = DeadSpawnPoint.transform.position;
        moveSpeed = 0;
        movedir = Vector3.zero;
        destinationDistance = 0;
        destinationPosition = this.transform.position;
        target = null;
        ctrlAnimState = ControlAnimationState.Idle;
        alreadyLockSkill = false;
        Invoke("resetCheckAttack", 0.1f);

        oneShotOpenDeadWindow = false;

    }
}
