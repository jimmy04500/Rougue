using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {

	//public Hashtable<string, float> attributes = new Hashtable<string, float>();
	private GameObject[] enemies; //= new GameObject[200];
	static GameObject player;
	public static GameObject closest;

	public LayerMask enemiesWalls;
	public LayerMask playerWalls;

	public int level = 0;
	public bool levelUp = false;
	public int upgradePoints;
	public int floor = 1;

    //Different Stat categories, so things don't get messed up when leveling up or when debuffs get whatever'd
    //base is controlled by levels and the species
    //mod is controlled by buffs and debuffs
    //eff is the effective values used in Status, maybe
    //equip is used by armor and items and whatnot
    public int baseMaxHealth;
    public int baseMaxMana;
    public float baseManaRegen;
    public float baseHealthRegen;
    public float baseSpeed;
    public int baseAgility;
    public int baseStrength;
    public int baseIntelligence;
    public int baseAttackDamage;
    public int baseAttackSpeed;


    [HideInInspector] public int modMaxHealth;
    [HideInInspector] public int modMaxMana;
    [HideInInspector] public float modManaRegen;
    [HideInInspector] public float modHealthRegen;
    [HideInInspector] public float modSpeed;
    [HideInInspector] public int modAgility;
    [HideInInspector] public int modStrength;
    [HideInInspector] public int modIntelligence;
    [HideInInspector] public int modAttackDamage;
    [HideInInspector] public float modAttackSpeed;


    [HideInInspector] private int effMaxHealth;
    [HideInInspector] private int effMaxMana;
    [HideInInspector] private float effManaRegen;
    [HideInInspector] private float effHealthRegen;
    [HideInInspector] private float effSpeed;
    [HideInInspector] private int effAgility;
 /*   [HideInInspector]*/ public int effStrength;
    [HideInInspector] private int effIntelligence;
    [HideInInspector] private int effAttackDamage;
    [HideInInspector] private float effAttackSpeed;
    

    [HideInInspector] public int equipMaxHealth;
    [HideInInspector] public int equipMaxMana;
    [HideInInspector] public float equipManaRegen;
    [HideInInspector] public float equipHealthRegen;
    [HideInInspector] public float equipSpeed;
    [HideInInspector] public int equipAgility;
    [HideInInspector] public int equipStrength;
    [HideInInspector] public int equipIntelligence;
    [HideInInspector] public int equipAttackDamage;
    [HideInInspector] public float equipAttackSpeed;


	private float speedx;//speed-buff or slow-debuff
	public float agilityx;

    //public int agility;
   // public int strength;
  //  public int intelligence;
  //  private int attackDamage;

	[HideInInspector]
 //   public float maxHealth;
	private float health;
//	public float healthRegen;
	
	[HideInInspector]
   // public float maxMana;
	private float mana;
//	public float manaRegen;
	
	[HideInInspector]
	public float rage = 0.0f;
	public float rageDecay;
	[HideInInspector]
	public float rageTimer = 0.0f;
	[HideInInspector]
	public bool isRaged = false;
	
	[HideInInspector]
	public float attackTimer = 0.0f;
	
	public float damagex;
	public float defense;//type 1 only defends against type1
	public float resistence;

	public float range1;
	
	//[HideInInspector]
	public bool isStunned = false;
	public bool getSlowed = false;
	public bool isSlowed = false;
	public float slowTimer=0;

	public int exp;
	public int expDrop;
	public float money;

	public bool see=false;
	public GameObject sword;
	
	// Use this for initialization
	void Start () {
		if (gameObject.tag == "Enemy") {
			level = Random.Range (floor, floor+10);
		}
        speedx = 1;




        refreshStats();
		//maxHealth = 100f*(Mathf.Pow(1.05f, level-1));
		
		health = effMaxHealth;
		mana = effMaxMana;

		player = GameObject.FindGameObjectWithTag ("Player");
	}

    public void clearEquip()
    {

     equipMaxHealth = 0;
     equipMaxMana = 0;
     equipManaRegen = 0;
     equipHealthRegen = 0;
     equipSpeed = 0;
     equipAgility = 0;
     equipStrength = 0;
     equipIntelligence = 0;
    equipAttackDamage = 0;
    equipAttackSpeed = 0;

    }


    public void refreshStats() //Terrible, terrible things will happen if this is called while debuffed/buffed.
    {
    effAgility = baseAgility + modAgility + equipAgility;
    effStrength = baseStrength + modStrength + equipStrength;
    effIntelligence = baseIntelligence + modIntelligence + equipIntelligence;

    effMaxHealth = baseMaxHealth + modMaxHealth + equipMaxHealth + 10*effStrength;
    effMaxMana = baseMaxMana + modMaxMana + equipMaxMana + 10*effIntelligence;
    effManaRegen = baseManaRegen + modManaRegen + equipManaRegen + 0.1f*effIntelligence;
    effHealthRegen = baseHealthRegen + modHealthRegen + equipHealthRegen + 0.1f*effStrength;
    effSpeed = baseSpeed + modSpeed + equipSpeed + (effAgility/10); //Should also include + effAgility/10, but I get a really wierd bug.
    effAttackDamage = baseAttackDamage + modAttackDamage + equipAttackDamage + effStrength;
    effAttackSpeed = baseAttackSpeed + modAttackSpeed + equipAttackSpeed + (effAgility/10); //Same comment as effSpeed

    }
	
	// Update is called once per frame
	void Update () {

		if (health <= 0 && gameObject.tag=="Enemy") {
			player.GetComponent<Status>().money+=level+expDrop;//don't think the (Clone) part is needed, but I put anyways; could also maybe find by tag "Player"
			player.GetComponent<Status>().exp+=level+expDrop;
			Destroy (gameObject);
		}

		if (exp>100+10*level){
			exp-=100+10*level;
			level++;
			baseStrength += 2;
			baseIntelligence += 2;
            baseAgility += 2;
			upgradePoints++;
            refreshStats();
		}
		
		if (health>effMaxHealth){
			health=effMaxHealth;
		}else if (health<effMaxHealth){
			health += Time.deltaTime * effHealthRegen;
		}
		
		if(mana > effMaxMana) {
			mana = effMaxMana;
		} else if(mana < effMaxMana){
			mana += Time.deltaTime * effManaRegen;
		}
		
		if (rage < 0) {
			rage=0;
		}else if (rage>=100&&gameObject.tag == "Player" && !isRaged){//rage on
			GameObject rageEffect = Resources.Load ("Rage") as GameObject; 
			rageEffect.GetComponent<TimedEffect>().target = gameObject;
			GameObject.Instantiate(rageEffect, player.transform.position, Quaternion.identity);
			//speedx+=1f;
			//damagex+=10f;
			//attackSpeed+=0.5f;
			//rageTimer = 6f; 
			//isRaged=true;
		}else if (rage > 0) {
			rage -= Time.deltaTime * rageDecay;
		}
		if (rageTimer>0){
			rageTimer -= Time.deltaTime;
		}else if (isRaged && rageTimer<=0){//rage off
			//speedx-=1f;
			//damagex-=10f;
			//attackSpeed-=0.5f;
			//isRaged=false;
			rageTimer=0;
			rage = 0;
		}
		
		if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.T)){
			getSlowed=true;
		}

		if (getSlowed && !isSlowed) {
			speedx-=0.8f;
			isSlowed=true;
			getSlowed=false;
			slowTimer=3;//default can be changed, I believe
		}
		if (slowTimer > 0) {
			slowTimer-=Time.deltaTime;
		} else if (isSlowed && slowTimer<=0){
			speedx+=0.8f;
			isSlowed=false;
		}

		autoAttack ();
	}

	public void MagicDamage(float d)
	{
	
		health -= d;
	}

	public void PhysicalDamage(float d)
	{

		health -= d;
	}

	public void PureDamage(float d)
	{
		health -= d;
	}

	public void dRage(float r){
		rage += r;
	}
	void Attack(Status target)
	{
		target.PhysicalDamage(effAttackDamage* damagex);
	}
	void Rage(Status target){
		target.dRage (effAttackDamage * damagex * 100f / effMaxHealth);
	}
	//void OnCollisionStay2D (Collision2D collider){
	void autoAttack(){//not necessarily melee range (uses range1), but this uses strength
		//closest = FindClosestEnemy();
		if (attackTimer<=0){
			//closest = FindClosestEnemyWalls(range1);
			closest = FindClosestEnemyWalls(range1);//
			if (closest != null){
				//try
				if (gameObject.tag == "Player" && getDistance(closest) < range1 
				    //&&//need if FindClosestEnemy w/o the Walls
				    ) 
				{
					Debug.Log ("AutoAttack1");
					Attack (closest.gameObject.GetComponent<Status> ());


					if (!closest.gameObject.GetComponent<Status> ().isRaged) {
						Rage(closest.gameObject.GetComponent<Status> ());
					}
					attackTimer = 1 / effAttackSpeed;
					GameObject swing = Instantiate(sword, transform.position, Quaternion.identity) as GameObject;
					swing.transform.parent = gameObject.transform;
				}
			}
			if (gameObject.tag=="Enemy" && getDistance(player) < range1 
			    //&& //
			    )
			{
				var heading = player.transform.position - gameObject.transform.position;
				var distance2 = heading.magnitude;
				var direction = heading/distance2;
				RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, range1, playerWalls);
				see = false;
				if (hit != null && hit.collider.tag == "Player"){
					see = true;
					//Debug.Log ("AutoAttack2");
					Attack (player.gameObject.GetComponent<Status> ());
				
					if (!player.gameObject.GetComponent<Status> ().isRaged){
						Rage(player.gameObject.GetComponent<Status> ());
					}
					player.gameObject.GetComponent<Player>().blood.Play();
					attackTimer = 1/effAttackSpeed;
				}
			}
		}
	}

	public GameObject FindClosestEnemy() {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject closest = null;
		if (gameObject.tag == "Player"){
			float distance = Mathf.Infinity;
			Vector3 position = transform.position;
			foreach (GameObject go in enemies) {
				float curDistance = (go.transform.position - position).sqrMagnitude;
				//Vector3 diff = go.transform.position - position;
				//float curDistance = diff.sqrMagnitude;
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
		}
		if (closest != null) {
			return closest;
		} else {
			return null;
		}
	}
	public GameObject FindClosestEnemyWalls(float range) {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject closest = null;
		if (gameObject.tag == "Player"){
			float distance = Mathf.Infinity;
			Vector3 position = transform.position;
			foreach (GameObject go in enemies) {
				float curDistance = (go.transform.position - position).sqrMagnitude;
				//Vector3 diff = go.transform.position - position;
				//float curDistance = diff.sqrMagnitude;
				if (curDistance < distance) {
					var heading = go.transform.position - transform.position;
					var distance2 = heading.magnitude;
					var direction = heading/distance2;
					RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, enemiesWalls);
					//Debug.Log (hit.collider.tag);
					if (hit){//implicit unity operator
						if(hit.collider.tag == "Enemy"){
						//Debug.Log (hit.collider.tag);
						closest = go;
						distance = curDistance;
						}
					}
				}
			}
		}
		if (closest != null) {
			return closest;
		} else {
			return null;
		}
	}
	//public GameObject FindPlayerWalls(float range) {}
	public float getDistance(GameObject go){
		return (go.transform.position - gameObject.transform.position).sqrMagnitude;
	}

//	public void buff(float duration, float startTime, float repeatTime){
//		if (repeatTime>0){
//			duration-=Time.deltaTime;
//			if (duration==0){
//				return;
//			}
//		}
//
//	}

	public int getExperienceLeft(){
		return 100+10*level - (int)exp;
	}
	public float getPercentExp(){
		return 100f * exp / (100f + 10f * (float) level);
	}

	public float getEffectiveSpeed(){
		return speedx * effSpeed;
	}

    public float getSpeedx() {
        return speedx;
    }

    public void setSpeedx(float p) {
        speedx = p;
    }

    public int getPercentMana(){
        return (int) ((mana/effMaxMana + 0.0001) * 100);
    }

    public int getPercentHealth() {
        return (int) ((health/effMaxHealth + 0.0001) * 100);
    }

    public int getIntelligence()
    {
        return effIntelligence;
    }

    public int getStrength()
    {
        return effStrength;
    }

    public int getAgility()
    {
        return effAgility;
    }

    public void useMana(float m)
    {
        mana -= m;
    }
    
    public void healHealth(float h)
    {
        health += h;
        
        if(health > effMaxHealth)
            health = effMaxHealth;
    }

    public float getMana()
    {
        return mana;
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxMana()
    {
        return effMaxMana;
    }

    public float getMaxHealth()
    {
        return effMaxHealth;
    }
}
