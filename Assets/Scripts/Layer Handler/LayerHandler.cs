using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerHandler : MonoBehaviour
{

    //The Y-offset from this object for the layer effect to occur
    [SerializeField] float yOffsetEffect;

    //The target sorting order when the layer effect is occurring 
    [SerializeField] int effectLayerOrder;

    //original sorting order
    private int initLayerOrder;

    //Cache components
    private SpriteRenderer artRenderer;
    private Transform playerPos;



    //Cache the sprite renderer and find the player in the scene. Also retrieve the original sorting order.
    private void OnEnable() {
        artRenderer = GetComponent<SpriteRenderer>();
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();

        initLayerOrder = artRenderer.sortingOrder;
    }

    private void Update() {
        //If the player is found in the scene
        if (playerPos) {
            
            //and if the player is placed higher than this object, prioritize this object's layer over the player (by setting its order to "effectLayerOrder")
            if (playerPos.position.y > transform.position.y + yOffsetEffect) {
                artRenderer.sortingOrder = effectLayerOrder;
            } else {

                //If the player is under the object, prioritize the player's layer over this object's
                artRenderer.sortingOrder = initLayerOrder;
            }
        }
    }
}
