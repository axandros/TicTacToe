using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAndCrossSpace : MonoBehaviour
{   
    private bool? state;
    public bool? State { get { return state; } }

    private SpriteRenderer SR;

    [SerializeField]
    private float secondsToAnimate = 0.2f;
    private float animating;


    [SerializeField]
    private Sprite Nought;
    [SerializeField]
    private Sprite Cross;

    //private TTTManager 


    // Start is called before the first frame update
    void Start()
    {
        state = null;
        SR = GetComponent<SpriteRenderer>();

        animating = secondsToAnimate;

        //StartTest();
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
}
