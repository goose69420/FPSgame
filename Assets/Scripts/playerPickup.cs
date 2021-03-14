using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerPickup : MonoBehaviour
{
    public GameObject player;
    public GameObject center;
    public GameObject position;
    public GameObject item;
    public pickupController PController;
    public bool pickedAny;
    public float Epresses = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().gameManager.GetComponent<GameControl>().paused)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Epresses++;
            }

            if (Epresses == 1 && pickedAny)
            {
                pickedAny = false;
                PController.picked = false;
                item = null;
                PController = null;
            }
            if (Epresses == 0)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (!hitObject.CompareTag("pickupable"))
                    {
                        return;
                    }
                    if (hitObject.GetComponent<pickupController>().pickupable)
                    {
                        item = hitObject;
                        PController = item.GetComponent<pickupController>();
                        pickedAny = true;
                        PController.picked = true;
                    }
                }
            }
            if (pickedAny)
            {
                item.transform.position = transform.position + transform.forward * 5 * transform.localScale.y + PController.offset;
                if (PController.needsRotation) { item.transform.LookAt(player.transform); };
                if (Input.GetKeyDown(KeyCode.Mouse0) && PController.hasAction)
                {
                    Debug.Log("прива)");
                    PController.Action();
                }
            }
            Epresses = Epresses % 2;
        }
    }
}
