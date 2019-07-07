using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TTTManager tManager = null;

    [SerializeField]
    private bool State = true;
    public bool Symbol { get { return State; } set { State = value; } }

    // Determines how active the player is - allows pausing input.
    [SerializeField]
    private bool _reading = true;
    
    public bool Reading { get { return _reading; } set {
            //Debug.Log("Reading = " + value);
            _reading = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if(tManager == null)
        {
            var go = GameObject.FindGameObjectWithTag("Tag");
            if(go != null) { tManager = go.GetComponent<TTTManager>(); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Reading)
        {
            CheckClick();
        }
        //AltClick();
    }
    
    private void CheckClick()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.transform != null)
            {
                //Debug.Log("hit: " + hit.ToString());
                GameObject obj = hit.collider.gameObject;
                NotAndCrossSpace NCS = obj.GetComponent<NotAndCrossSpace>();
                if (NCS != null)
                {
                    // We have contact!  We can tell it what to do now
                    tManager.SetSpace(NCS, State);
                }
            }
        }
    }

    public void AltClick()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.transform != null)
            {
                GameObject obj = hit.collider.gameObject;
                NotAndCrossSpace NCS = obj.GetComponent<NotAndCrossSpace>();
                if (NCS != null)
                {
                    // We have contact!  We can tell it what to do now
                    tManager.SetSpace(NCS, !State);
                }
            }
        }
    }
}
