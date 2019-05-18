using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TTTManager tManager;

    [SerializeField]
    private bool? state;

    // Start is called before the first frame update
    void Start()
    {
        state = false;
        if(tManager == null)
        {
            var go = GameObject.FindGameObjectWithTag("Tag");
            if(go != null) { tManager = go.GetComponent<TTTManager>(); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Click") > 0)
        {
           // Debug.Log("Click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition
                , Input.mousePosition - Camera.main.ScreenToWorldPoint(Input.mousePosition)
                , Mathf.Infinity);
            //Debug.Log("Hit: " + hit.collider);
            if (hit)
            {
                //Debug.Log("Hit: " + hit.ToString());

                GameObject obj = hit.collider.gameObject;
                NotAndCrossSpace NCS = obj.GetComponent<NotAndCrossSpace>();
                if (NCS != null)
                {
                    // We have contact!  We can tell it what to do now
                    tManager.SetSpace(NCS, state);
                }
            }
        }
    }
    
}
