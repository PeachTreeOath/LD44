using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFader : MonoBehaviour
{
    private float totalAnimationTime = 7;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        StartCoroutine("StartWaveTextCR");
        Invoke("Delete", 8);
    }

    IEnumerator StartWaveTextCR()
    {
        float timeToAnimate = 0;
        while (timeToAnimate < totalAnimationTime)
        {
            timeToAnimate += Time.deltaTime;
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, totalAnimationTime - timeToAnimate);
            yield return null;
        }
    }


    void Delete()
    {
        Destroy(gameObject);
    }
}
