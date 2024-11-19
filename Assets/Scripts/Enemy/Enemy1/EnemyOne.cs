public class EnemyOne : EnemyWander
{   
    private EnemyChase enemyChaseScript;

    private protected void Awake(){
        GetTransforms();
        if(enemyChaseScript ==null){
            enemyChaseScript = GetComponent<EnemyChase>();
        }
    }

    protected void Start(){
        FindValidLocation();
    }

    protected void Update(){
        if(!enemyChaseScript.GetChaseState()){
            Move();
            enemyChaseScript.PlayerDetected();
        }else{
            enemyChaseScript.Chase();
        }
    }
}
