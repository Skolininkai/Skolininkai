using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AimGameManagerTests
{
    GameObject center;
    GameObject player;
    GameObject door;
    AimGameManager manager;
    GameObject spherePrefab;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        center = new GameObject("Center");
        player = new GameObject("Player");
        player.tag = "Player";
        GameObject door = new GameObject("Door");
        door.tag = "Door";
        door.AddComponent<SlidingDoor>();

        GameObject managerObj = new GameObject("Manager");
        manager = managerObj.AddComponent<AimGameManager>();
        manager.centerPoint = center.transform;
        manager.totalSpheresToPop = 5;

        spherePrefab = new GameObject("SpherePrefab");
        spherePrefab.AddComponent<MeshRenderer>();
        spherePrefab.AddComponent<MeshFilter>();
        spherePrefab.AddComponent<SphereCollider>();
        spherePrefab.tag = "Sphere";
        Object.DontDestroyOnLoad(spherePrefab);
        manager.spherePrefab = spherePrefab;

        GameObject cameraObj = new GameObject("MainCamera");
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.transform.position = new Vector3(0, 0, -10f);
        cam.transform.LookAt(Vector3.zero);
        cameraObj.tag = "MainCamera";

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.DestroyImmediate(center);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(door);
        Object.DestroyImmediate(manager.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator StartMiniGame_ResetsStateCorrectly()
    {
        player.transform.position = Vector3.one * 10f;
        
        manager.StartMiniGame();
        yield return null;

        Assert.AreEqual(center.transform.position, player.transform.position);
        Assert.AreEqual(manager.gameDuration, manager.currentTime, 0.1f);
        Assert.AreEqual(manager.maxActiveSpheres, manager.activeSpheres);
    }

    [UnityTest]
    public IEnumerator SpawnSphere_WithinCylindricalBounds()
    {
        manager.SpawnSphere();
        yield return null;

        GameObject sphere = GameObject.FindWithTag("Sphere");
        Vector3 pos = sphere.transform.position - center.transform.position;
        
        Assert.LessOrEqual(new Vector2(pos.x, pos.z).magnitude, manager.spawnRadius);
        Assert.GreaterOrEqual(pos.y, manager.cylinderMinY);
        Assert.LessOrEqual(pos.y, manager.cylinderMaxY);
        Assert.GreaterOrEqual(sphere.transform.localScale.x, manager.minSphereSize);
        Assert.LessOrEqual(sphere.transform.localScale.x, manager.maxSphereSize);
    }

    [UnityTest]
    public IEnumerator PopSphere_UpdatesGameState()
    {
        manager.maxActiveSpheres = 1;
        manager.StartMiniGame();
        yield return null;

        GameObject sphere = GameObject.FindWithTag("Sphere");
        Assert.IsNotNull(sphere, "Sphere not spawned");

        Camera.main.transform.position = sphere.transform.position + Vector3.back * 5f;
        Camera.main.transform.LookAt(sphere.transform.position);

        yield return null;

        Assert.AreEqual(1, manager.poppedSpheres, "Popped count mismatch");
        Assert.AreEqual(1, manager.activeSpheres, "New sphere should replace popped one");
    }

    [UnityTest]
    public IEnumerator Timer_CountsDownCorrectly()
    {
        manager.StartMiniGame();
        float initialTime = manager.currentTime;

        yield return new WaitForSeconds(0.1f);

        Assert.Less(manager.currentTime, initialTime);
    }

    [UnityTest]
    public IEnumerator WinCondition_TriggersOnTargetMet()
    {
        manager.totalSpheresToPop = 1;
        manager.maxActiveSpheres = 1;
        manager.gameDuration = 10f;
        manager.StartMiniGame();

        manager.enabled = false;
        yield return null;

        GameObject sphere = GameObject.FindWithTag("Sphere");
        Assert.IsNotNull(sphere, "No sphere was spawned.");
        Vector3 spherePosition = sphere.transform.position;

        Camera.main.transform.position = spherePosition + Vector3.back * 5f;
        Camera.main.transform.LookAt(spherePosition);

        manager.enabled = true;
        
        yield return new WaitForSeconds(0.5f);
        yield return null;

        Assert.IsFalse(manager.isMiniGameActive);
        LogAssert.Expect(LogType.Log, "You won!");
    }

    [UnityTest]
    public IEnumerator LoseCondition_TriggersOnTimeout()
    {
        manager.gameDuration = 0.1f;
        manager.StartMiniGame();

        yield return new WaitForSeconds(0.2f);

        Assert.IsFalse(manager.isMiniGameActive);
        LogAssert.Expect(LogType.Log, "Try again!");
        manager.EndMiniGame(false);
    }

    [UnityTest]
    public IEnumerator MaxActiveSpheres_Enforced()
    {
        manager.maxActiveSpheres = 2;
        manager.StartMiniGame();
        yield return null;

        manager.SpawnSphere();
        yield return null;

        Assert.AreEqual(2, manager.activeSpheres);
    }
}