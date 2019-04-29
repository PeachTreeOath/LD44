using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    public static TreasureController instance;
    public event System.Action<int> CountChangedEvent;

    #region Editor Properties

    public bool showGizmos = false;
    public bool showTreasures = false;
    public GameObject[] treasurePrefabs;
    public GameObject treasureContainer;
    public int maxTreasures = 10;
    public float fieldWidth = 4;
    public float fieldHeight = 4;
    public float distanceBetweenTreasures = 1;

    #endregion Editor Properties

    #region Private Variables

    private PoissonDiscSampler poissonNoise;
    private List<KeyValuePair<Vector2, GameObject>> treasures = new List<KeyValuePair<Vector2, GameObject>>();

    #endregion Private Variables

    #region Game Loop

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
		// Sets the delegate action to have GameManager assess game state
		CountChangedEvent = GameManager.instance.AssessTreasureCount;

		GenerateTreasure();
    }

    private void Update()
    {
        /* For testing only
        if(Input.GetMouseButtonUp(0))
        {
            GameObject treasure = ClickSelect();
            TakeTreasure(treasure);
        }
        */
    }

    private void OnValidate()
    {
        if (showTreasures)
            GenerateTreasure();
        else
            CleanUpTreasure();
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            foreach (KeyValuePair<Vector2, GameObject> treasurePile in treasures)
            {
                Gizmos.DrawSphere(treasurePile.Key, 0.3f);
            }
        }
    }

    private void OnApplicationQuit()
    {
        CleanUpTreasure();
    }

    #endregion Game Loop

    #region Public Methods

    public void PlaceTreasure(GameObject treasure, Vector2 location)
    {
        treasures.Add(new KeyValuePair<Vector2, GameObject>(location, treasure));
        CountChangedEvent?.Invoke(treasures.Count);
        treasure.transform.position = location;
        treasure.SetActive(true);
    }

    public GameObject TakeTreasure(GameObject treasure)
    {
        KeyValuePair<Vector2, GameObject> found = treasures.FirstOrDefault(obj => obj.Value == treasure);

        if (found.Key != null)
        {
            found.Value.SetActive(false);
        }

        return found.Value;
    }

    public void DropTreasure(GameObject treasure, Vector2 location)
    {
        KeyValuePair<Vector2, GameObject> found = treasures.FirstOrDefault(obj => obj.Value == treasure);

        if (found.Key != null)
        {
            CountChangedEvent?.Invoke(treasures.Count);
            treasure.transform.position = location;
            treasure.SetActive(true);
        }
        
    }

    public bool DestoryTreasure(GameObject treasure)
    {
        KeyValuePair<Vector2, GameObject> found = treasures.FirstOrDefault(obj => obj.Value == treasure);

        if (found.Key != null)
        {
            treasures.Remove(found);
            Destroy(found.Value);
            CountChangedEvent?.Invoke(treasures.Count);
            return true;
        }

        return false;
    }

    #endregion Public Methods

    #region Private Methods

    private GameObject PlaceTreasure(Vector3 location)
    {
        GameObject treasure = Instantiate(treasurePrefabs[Random.Range(0, treasurePrefabs.Length)], location, Quaternion.identity);
        treasure.transform.parent = treasureContainer.transform;
        return treasure;
    }

    private void GenerateTreasure()
    {
        CleanUpTreasure();

        Vector2 offset = new Vector2(0.5f * fieldWidth, 0.5f * fieldHeight); // To Do : Making assumptions about the default position of the arena
        treasures.Clear();
        poissonNoise = new PoissonDiscSampler(fieldWidth, fieldHeight, distanceBetweenTreasures);
        List<Vector2> locations = poissonNoise.Samples().ToList();
        locations = locations.Select(obj => obj -= offset).ToList();

        for (int i = 0; i < (maxTreasures >= locations.Count ? locations.Count : maxTreasures); i++)
        {
            treasures.Add(new KeyValuePair<Vector2, GameObject>(locations[i], PlaceTreasure(locations[i])));
        }
    }

    private void CleanUpTreasure()
    {
        foreach (Transform child in treasureContainer.transform)
        {
            //if (Application.isEditor)
            //    UnityEditor.EditorApplication.delayCall += () => { if (child != null) { DestroyImmediate(child.gameObject); } };
            //else
                Destroy(child.gameObject);

        }
    }

    #endregion Private Methods

    #region Tests

    /// <summary>
    /// For Testing Only
    /// </summary>
    /// <returns></returns>
    private GameObject ClickSelect()
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            foreach (Transform child in treasureContainer.transform)
            {
                if (child.gameObject == hit.transform.gameObject)
                    return hit.transform.gameObject;
            }
            return null;
        }

        else return null;
    }

    #endregion Tests
}
