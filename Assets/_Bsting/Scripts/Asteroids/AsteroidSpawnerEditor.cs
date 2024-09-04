using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * Override the Inspector editor to allow for a GUI button to spawn 
 * in a single asteroid for testing purposes.
 */
[CustomEditor(typeof(AsteroidUnitSpawner))]
public class AsteroidSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the basic Unity Inspector first (we'll add on to it):
        DrawDefaultInspector();

        // Target the Asteroid Spawner script:
        AsteroidUnitSpawner asteroidSpawnerScript = (AsteroidUnitSpawner)target;

        // Add a testing spawn button to the Inspector:
        if (GUILayout.Button("Spawn Test Asteroid"))
        {
            asteroidSpawnerScript.SpawnAsteroid(asteroidSpawnerScript.TargetSource, 
                                                asteroidSpawnerScript.AngleSpawnCone, 
                                                asteroidSpawnerScript.RadiusToSpawnAwayFrom, 
                                                asteroidSpawnerScript.PrefabToSpawn);
        }
    }
}
