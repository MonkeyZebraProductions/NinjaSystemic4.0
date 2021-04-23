using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEmotions : MonoBehaviour
{
    public SpriteRenderer emotionRenderer;

    public Sprite soundHeard;
    public Sprite playerSeen;

    public void SoundHeard()
    {
        if(GetComponent<EnemyAI>().playerSeen == false)
        {
            emotionRenderer.sprite = soundHeard;
            FindObjectOfType<AudioManager>().Play("EnemyHuh");
        }
    }

    public void PlayerSeen()
    {
        emotionRenderer.sprite = playerSeen;
    }

    public void ClearEmotion()
    {
        emotionRenderer.sprite = null;
    }
}
