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
        inMush = true;
        if (other.gameObject.tag == "Player") {
            Debug.Log("boing");
            player.Jump(true);
        }
	}
    void OnTriggerExit (Collider other) 
	{
        inMush = false;
	}
    public bool getInMush() {
        return inMush;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
