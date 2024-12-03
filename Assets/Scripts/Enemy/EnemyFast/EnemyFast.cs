using UnityEngine;

public class EnemyFast : EnemyWander
{
    [Header("Enemy Information")]
    public const int ENEMY_ID = 02;
    public bool isEnemyAlive = true;
    
    private EnemyChase enemyChaseScript;
    private Damageable EHP;

    [Header("Animation Controller")]
    [SerializeField] private EnemyanimatorController EAC;
    [Header("Death Explosion")]
    [SerializeField] private GameObject deathExplosion;

    protected void Awake(){
        // get enemy transfrom
        GetTransforms();
        // Sets chase script if not done so in editor
        if(enemyChaseScript ==null) enemyChaseScript = GetComponent<EnemyChase>();
        enemyChaseScript.SetEnemy(ENEMY_ID);

        if(EHP ==null) EHP = GetComponent<Damageable>();
        
        // Sets EAC. Needs to be specified since each enemy has two animators
        if(EAC == null) EAC = GetComponent<Transform>().GetChild(0).GetComponentInChildren<EnemyanimatorController>();
    }

    // Runs on start, Finds initial location for enemy to wander to
    protected void Start(){
        FindValidLocation();
    }

    // Runs once per frame
    protected void Update(){
        if(EHP.GetHP() > 0){
            Act();
        }else{
            isEnemyAlive = false;
            Instantiate(deathExplosion, transform.position, transform.rotation);
            enemySprite.gameObject.SetActive(false);
        }
    }

    protected void Act(){
        // Switches between chase state and wander state
        if(!enemyChaseScript.GetChaseState()){
            // Moves player, and alse sets the current animation state
            EAC.PlayState(Move());

            // Runs player detection to see if player is in range
            enemyChaseScript.PlayerDetected();
        }else{
            // Runs chase script
            EAC.PlayState(enemyChaseScript.Chase());
        }
    }
}

