using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to conveniently store references to dynamically loaded resources. Use this
/// when you need references to prefabs but don't have a place in the Unity inspector to assign it.
/// </summary>
/// <typeparam name="ResourceLoader"></typeparam>
public class ResourceLoader : Singleton<ResourceLoader>
{
    [HideInInspector] public Sprite mimicClosedSprite;
    [HideInInspector] public Sprite mimicOpenedSprite;
    [HideInInspector] public GameObject lickParticles;
    [HideInInspector] public GameObject bloodParticles;
    [HideInInspector] public GameObject bloodSprite;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        LoadResources();
    }

    private void LoadResources()
    {
        mimicClosedSprite = Resources.Load<Sprite>("Sprites/sweet_mimic");
        mimicOpenedSprite = Resources.Load<Sprite>("Sprites/sCARY_MIMIC");
        lickParticles = Resources.Load<GameObject>("Prefabs/LickParticles");
        bloodParticles = Resources.Load<GameObject>("Prefabs/BloodParticles");
        bloodSprite = Resources.Load<GameObject>("Prefabs/Bloodpuddle");
    }
}
