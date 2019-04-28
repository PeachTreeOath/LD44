using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    // camera offset variables
    [SerializeField]
    private float shake;

    private float angle = 1.5f;
    private float offset = 0.3f;
    private float xOffset;
    private float yOffset;
    private float angleOffset;
    private float randomNegOneToOne;
    private float test = 0.0f;

    // turn shake on variable
    public bool shakeOn;

    public float trauma;

    Vector3 originalPosition;
    Quaternion originalRotation;

    protected override void Awake()
    {
        base.Awake();

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

        Shake();
    }

    private void Shake()
    {

        if (trauma > 0)
        {

            shake = Mathf.Pow(trauma, 2);
            if (shake > 1)
            {
                shake = 1.0f;
            }
            Debug.Log("Shake: " + shake);
            //angleOffset = angle * shake * Random.Range(-1, 1);
            //xOffset = offset * shake * Random.Range(-1, 1);
            //yOffset = offset * shake * Random.Range(-1, 1);
            angleOffset = angle * shake * Mathf.PerlinNoise(test - 1, test + 1) * Random.Range(-1, 1);
            xOffset = offset * shake * Mathf.PerlinNoise(test - 2f, test + 2f) * Random.Range(-1, 1);
            yOffset = offset * shake * Mathf.PerlinNoise(test - 3f, test + 3f) * Random.Range(-1, 1);

            transform.localPosition = new Vector3(xOffset, yOffset, originalPosition.z);
            transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, angleOffset);

            trauma -= Time.deltaTime;
        }
        else
        {
            trauma = 0.0f;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }

    private void TurnShakeOnOff()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            trauma += 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            trauma += 0.5f;
        }
    }


}
