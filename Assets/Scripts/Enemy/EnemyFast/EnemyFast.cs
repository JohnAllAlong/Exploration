using UnityEngine;

public class EnemyFast : EnemyWander
{
    private EnemyChase enemyChaseScript;

    [Header("Enemy Information")]
    public const float ENEMY_ID = 02;
    public bool isEnemyAlive = true;

    [Header("Animation Controller")]
    [SerializeField]private EnemyanimatorController EAC;

    protected void Awake(){
        // get enemy transfrom
        GetTransforms();

        // Sets chase script if not done so in editor
        if(enemyChaseScript ==null) enemyChaseScript = GetComponent<EnemyChase>();
        
        // Sets EAC. Needs to be specified since each enemy has two animators
        if(EAC == null) EAC = GetComponent<Transform>().GetChild(0).GetComponentInChildren<EnemyanimatorController>();
    }

    // Runs on start, Finds initial location for enemy to wander to
    protected void Start(){
        FindValidLocation();
    }

    // Runs once per frame
    protected void Update(){
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

