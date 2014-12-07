using UnityEngine;
using System.Collections;

public class Player : Entities 
{

	public bool paused;
	private Texture2D scroll;

	private int previousDirection;
	private float velocity;
	public bool alive = true;
	public float curHealth;
	private MakeMap Dungeon;
	public equipmentStats equippedSword;
	public equipmentStats equippedArmor;
	public equipmentStats equippedHelmet;
	public equipmentStats equippedNecklace;
	
	Animator anim;

	Transform weapon;

	public ParticleSystem blood;//turned public
	public GameObject bloodSpatter;

	// GUI Object
	private PlayerGUI playerGUI;

	private void Start () {
        
        InitializeEntity();
        SelfCast = new Heal(gameObject);
		SelfCast2 = new PoisonCloud(gameObject);
        AutoTarget = new Fireball(gameObject);
		AutoTarget2 = new Mudball (gameObject);
        PosTarget = new MagicMissle(gameObject);

		anim = GetComponent<Animator> ();
		curHealth = gameObject.GetComponent<Status> ().getHealth();
		blood = transform.Find("Blood").GetComponent<ParticleSystem>();

        Dungeon = GameObject.Find("MapGenerator").GetComponent<MakeMap>();

		weapon = transform.Find ("Weapon");

		playerGUI = new PlayerGUI (this);


		paused = false;
		Time.timeScale = 1;

	}

	private void FixedUpdate () 
		//walkDirection: 1 = left, 2 = up, 3 = right, 4 = down;
		//idleDirection: saves previous walkDirection to animate idle
	{
        float dx = 0;
        float dy = 0;
		if (!gameObject.GetComponent<Status>().isStunned){
			if (Input.GetAxisRaw("Vertical") > 0) 
			{
				anim.SetInteger ("direction", 2);
				anim.SetFloat ("velocity", 1.0f);
				previousDirection = 2;
                dy = 1;
			}
			if (Input.GetAxisRaw("Horizontal") < 0) 
			{
				//anim.SetBool("a", true);
				//anim.SetBool("d", false);

				anim.SetInteger ("direction", 1);
				anim.SetFloat ("velocity", 1.0f);
				previousDirection = 1;
                dx = -1;
			}
			if (Input.GetAxisRaw("Vertical") < 0) 
			{
				if (!Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.LeftArrow)) 
					anim.SetInteger ("direction", 4);
				anim.SetFloat ("velocity", 1.0f);
				previousDirection = 4;
                dy = -1;
			}
			if (Input.GetAxisRaw("Horizontal") > 0) 
			{			
				Vector3 theScale = transform.localScale;
				theScale.x = 1;
				transform.localScale = theScale;

				anim.SetInteger ("direction", 3);
				anim.SetFloat ("velocity", 1.0f);
				previousDirection = 3;
                dx = 1;
			}
            //
            if(Input.GetButton("Heal"))
            {
                SelfCast.cast( gameObject);
            }

			if(Input.GetButton("PoisonCloud"))
			{
				SelfCast2.cast( gameObject);
			}

            if(Input.GetButton("Fireball"))
            {
				AutoTarget.cast( cStat.FindClosestEnemy());
            }

			if(Input.GetButton("Clayball")){
				AutoTarget2.cast (cStat.FindClosestEnemy());
			}
            if(Input.GetMouseButtonDown(0))
            {
                PosTarget.cast(Input.mousePosition);

            }
            
		}
		//if not moving
		if(!PlayerInput.isMoving())
		{
			anim.SetInteger ("direction", previousDirection);
			anim.SetFloat ("velocity", 0.0f);
		}

        this.setDirection(new Vector3(dx,dy,0));
        Move();

		if(Input.GetKeyDown (KeyCode.Escape)) 
		{
			if(paused)
			{
				paused = false;
				playerGUI.paused = false;
			}
			else
			{
				paused = true;
				playerGUI.paused = true;
			}
		}
		
		if(gameObject.GetComponent<Status> ().getHealth() <= 0 && Time.time > 1)
		{
			Die();		
		}

		UpdateGameState();
	}

	//NEEDS TO BE CALLED
	
	public void UpdateGameState()
	{
		Time.timeScale = paused ? 0 : 1;

	}
	
	public override void takeHealth(int amount)
	{
		health = health - amount;
		Debug.Log ("health left" + health);
		blood.Play();
		if(Random.value < 0.25f) {
			Instantiate(bloodSpatter, transform.position, Quaternion.identity);
		}
		
		if (health <= 0) {
			Die ();
		}
	}

	private void OnGUI() 
	{
		playerGUI.onGUI ();
	}

	private void OnTriggerStay2D( Collider2D other )
	{
		if(other.CompareTag("goal") && Input.GetButtonDown("Action")) 
		{
			Dungeon.NextFloor();
			//Application.LoadLevel ("Game");
		} 
		if(other.CompareTag("UpStairs") && Input.GetButtonDown("Action"))
		{
			Dungeon.PreviousFloor();
		}
	}

	public void Respawn(Vector3 spawnPt)
	{
		transform.position = spawnPt;
	}

	public override void Die()
	{
		alive = false;
		paused = true;
		playerGUI.alive = false;
		UpdateGameState ();
	}

    public void refreshEquipStats()
    {
        Debug.Log(cStat.getStrength());
        cStat.clearEquip();
        cStat.equipStrength += equippedSword.str;
       cStat.equipAgility += equippedSword.agility;
      cStat.equipIntelligence += equippedSword.intelligence; 
      cStat.refreshStats();
        Debug.Log(equippedSword.str);
        
    }
}
