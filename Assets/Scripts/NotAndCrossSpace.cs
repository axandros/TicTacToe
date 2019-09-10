using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAndCrossSpace : MonoBehaviour
{   
    private bool? state = null;
    public bool? State { get { return state; } }

    private SpriteRenderer SR = null;
    private AudioSource AS = null;

    [SerializeField]
    private float secondsToAnimate = 0.2f;
    private float animating = 0.0f;


    [SerializeField]
    private Sprite Nought = null;
    [SerializeField]
    private Sprite Cross = null;

    //private TTTManager 


    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        AS = GetComponent<AudioSource>();
        animating = secondsToAnimate;

        Reset();
    }
    public void Reset()
    {
        state = null;
        UpdateSprite();
    }

    private void StartTest()
    {
        float rand = Random.Range(0,3);
        if (rand < 1)
            state = null;
        else if (rand < 2)
            state = false;
        else state = true;
    }

    public void ChangeState(bool? value)
    {
        state = value;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (SR)
        {
            if (state == null)
            {
                SR.sprite = null;
            }
            else
            {
                //Debug.Log("Space is changing to " + state.Value);
                if (state.Value)
                {
                    SR.sprite = Cross;
                }
                else
                {
                    SR.sprite = Nought;
                }
                AnimateIn();
                if (AS) { AS.Play(); }
             }
        }
        
    }

    private void AnimateIn()
    {
        if (animating >= secondsToAnimate)
        {
            animating = 0;
        }
        if (animating < secondsToAnimate)
        {
            animating += Time.deltaTime;
            if (animating > secondsToAnimate) { animating = secondsToAnimate; }
            float s = animating / secondsToAnimate;
            transform.localScale = new Vector3(s, s, s);
        }
    }

    private void Update()
    {
        
        if(animating < secondsToAnimate)
        {
            AnimateIn();
        }
    }

    public static bool operator ==(NotAndCrossSpace ns1, NotAndCrossSpace ns2)
    {
        if (!ReferenceEquals(ns1, null) && !ReferenceEquals(ns2, null))
        {
            if (ns1.State == null && ns2.State == null)
            { return true; }
            if (ns1.State == null || ns2.State == null)
            { return false; }
            return ns1.State != ns2.State;
        }
        return false;
    }
    public static bool operator !=(NotAndCrossSpace ns1, NotAndCrossSpace ns2)
    {
        return !(ns1 == ns2);
    }
}
