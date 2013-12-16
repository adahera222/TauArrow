using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
     _
  _.;_'-._
 {`--.-'_,}
{; \,__.-'/}
{.'-`._;-';
 `'--._.-'
    .-\\,-"-.
    `- \( '-. \
        \;---,/
    .-""-;\
   /  .-' )\   
   \,---'` \\
            \|
*/

public class TauHUD : MonoBehaviour 
{
    public static TauHUD instance;
    public static TauHUD Instance { get { return instance; } }


    public TauController focused;
    public SpriteRenderer mouseSprite;
    public Sprite[] mouseSprites;

    public SpriteRenderer[] keyBGSprite;
    public SpriteRenderer[] keyLetterSprite;
    public Sprite[] keyBGSprites;
    public Sprite[] keyLetterSprites;
    
    
    public TauHUD()
    {
        instance = this;
    }

    public void Update()
    {
        if (focused != null)
        {
            if (focused.isShooting && focused.chargeDuration > 0f)
            {
                mouseSprite.sprite = mouseSprites[3];
            }
            else if (focused.isShooting)
            {
                mouseSprite.sprite = mouseSprites[2];
            }
            else if (focused.actor.currentWeapon == null)
            {
                mouseSprite.sprite = mouseSprites[1];
            }
            else
            {
                mouseSprite.sprite = mouseSprites[0];
            }



            keyBGSprite[1].sprite = (focused.forceX < 0f) ? keyBGSprites[2] : keyBGSprites[0];
            keyBGSprite[0].sprite = (focused.jumpDuration > 0f) ? keyBGSprites[2] : keyBGSprites[0];
            keyBGSprite[3].sprite = (focused.forceX > 0f) ? keyBGSprites[2] : keyBGSprites[0];
            
            if (focused.crouchDuration > 0f)
            {
                keyBGSprite[2].sprite = keyBGSprites[1];
            }
            else if (focused.didCrouch)
            {
                keyBGSprite[2].sprite = keyBGSprites[2];
            }
            else 
            {
                keyBGSprite[2].sprite = keyBGSprites[0];
            }
        }
    }

}
