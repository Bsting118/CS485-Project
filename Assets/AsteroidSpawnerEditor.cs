using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AsteroidUnitSpawner))]
public class AsteroidSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AsteroidUnitSpawner asteroidSpawnerScript = (AsteroidUnitSpawner)target;
    }
}
