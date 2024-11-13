public class EnemyOne : EnemyWander
{   
    private EnemyChase enemyChaseScript;

    private void Awake(){
        if(enemyChaseScript ==null){
            enemyChaseScript = GetComponent<EnemyChase>();
        }
    }

    void Start(){
        FindValidLocation();
    }

    void Update(){
        if(!enemyChaseScript.GetChaseState()){
            Move();
            enemyChaseScript.PlayerDetected();
        }else{
            enemyChaseScript.Chase();
        }
    }
}
