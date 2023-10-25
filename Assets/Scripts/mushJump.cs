using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushJump : MonoBehaviour
{

    public PlayerMovement player;
    private Vector3 boing = new Vector3(0, 40, 0);
    public bool inMush = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter (Collider other) 
	{
        
        if (other.gameObject.tag == "Player") {
            StartCoroutine(mushJiggle());
            inMush = true;
            Debug.Log("boing");
            player.Jump(true);
        }
	}
    void OnTriggerExit (Collider other) 
	{
        if (other.gameObject.tag == "Player") {
            inMush = false;
        }
        
	}
    public bool getInMush() {
        return inMush;
    }
    public bool jiggling = false;
    public IEnumerator mushJiggle() {
        jiggling = true;
        yield return new WaitForSeconds(1f);
        jiggling = false;

    }
    public MeshFilter mushMesh;
    private float timer;
    void Update()
    {
        
        timer += Time.deltaTime;
        if (jiggling) {
            mushMesh.transform.localScale = new Vector3(3,Mathf.Sin(timer*20)/4+ 1,3);
        } else {
            if (Mathf.Sin(timer*20)/4  == 0) mushMesh.transform.localScale = new Vector3(3,1,3);
        }
    }
}
