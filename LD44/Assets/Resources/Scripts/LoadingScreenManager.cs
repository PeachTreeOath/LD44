using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
	public GameObject movementInstr;
	public GameObject grabInstr;
	public GameObject attackInstr;
	public GameObject loadingText;

	GameObject playerObjInScene;

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	Vector3 upperSpawnPt;
	Vector3 lowerSpawnPt;

	bool hasMoved;
	bool hasGrabbed;

	int timer;

	private enum LoadingScreenState
	{
		Practice, // 0
		Pause,    // 1
		Test,     // 2
		Loading   // 3
	}

	LoadingScreenState thisState;
	
	// Start is called before the first frame update
    void Start()
    {
		movementInstr.SetActive(true);
		grabInstr.SetActive(true);
		attackInstr.SetActive(false);
		loadingText.SetActive(false);

		playerObjInScene = Instantiate(playerPrefab);
		playerObjInScene.transform.position = Vector3.zero;

		upperSpawnPt = new Vector3(0f, 1.5f, 0f);
		lowerSpawnPt = new Vector3(0f, -1.5f, 0f);

		timer = 200;

		thisState = LoadingScreenState.Practice;
    }

    // Update is called once per frame
    void Update()
    {
		switch (thisState)
		{
			case LoadingScreenState.Practice:
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
					Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
				{
					hasMoved = true;
				}
				if (Input.GetMouseButton(0))
				{
					hasGrabbed = true;
				}
				if (hasMoved && hasGrabbed)
				{
					thisState = LoadingScreenState.Pause;
				}
				break;
			case LoadingScreenState.Pause:
				timer--;
				if (timer < 1)
				{
					thisState = LoadingScreenState.Test;

					movementInstr.SetActive(false);
					grabInstr.SetActive(false);
					attackInstr.SetActive(true);
					loadingText.SetActive(false);

					GameObject objref = Instantiate(enemyPrefab);
					objref.transform.position =
						(playerObjInScene.transform.position - upperSpawnPt).sqrMagnitude >
						(playerObjInScene.transform.position - lowerSpawnPt).sqrMagnitude ?
							upperSpawnPt :
							lowerSpawnPt;
				}
				break;
			case LoadingScreenState.Test:
				if (GameObject.FindGameObjectWithTag("Enemy") == null)
				{
					movementInstr.SetActive(false);
					grabInstr.SetActive(false);
					attackInstr.SetActive(false);
					loadingText.SetActive(true);

					thisState = LoadingScreenState.Loading;
				}
				break;
			case LoadingScreenState.Loading:
			default:
				SceneManager.LoadScene("Game");
				break;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
			Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			hasMoved = true;
		}
		if (Input.GetMouseButton(0))
		{
			hasGrabbed = true;
		}
	}
}
