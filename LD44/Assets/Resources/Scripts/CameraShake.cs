using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // camera offset variables
    [SerializeField]
    private float shake;

    private float angleMax = 0.5f;
    private float offsetMax = 0.3f;
    private float xOffset;
    private float yOffset;
    private float angleOffset;
    private float randomNegOneToOne;

    // turn shake on variable
    public bool shakeOn;

    public float trauma;

    Vector3 originalPosition;
    Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        shakeOn = false;
        trauma = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TurnShakeOnOff();

        //if (shakeOn)
        //{
        //   Shake();
        //}

        Shake();
    }

    private void Shake()
    {

        if (trauma > 0)
        {

            shake = Mathf.Pow(trauma, 2);
            if (shake >= 1)
            {
                shake = 1.0f;
            }
            Debug.Log("Shake: " + shake);
            angleOffset = angleMax * shake * Random.Range(-1, 1);
            xOffset = offsetMax * shake * Random.Range(-1, 1);
            yOffset = offsetMax * shake * Random.Range(-1, 1);

            transform.localPosition = new Vector3(xOffset, yOffset, originalPosition.z);
            transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, angleOffset);

            trauma -= Time.deltaTime;
        }
        else
        {
            //shakeOn = false;
            trauma = 0.0f;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }

    private void TurnShakeOnOff()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            shakeOn = true;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            trauma += 0.2f;
        }
    }


}
