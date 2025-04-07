using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ButtonTests
{
    [UnityTest]
    public IEnumerator Button_StartsGame_OnPlayerEnter()
    {
        GameObject cameraObj = new GameObject("MainCamera");
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.transform.position = new Vector3(0, 0, -10f);
        cam.transform.LookAt(Vector3.zero);
        cameraObj.tag = "MainCamera";

        GameObject managerObj = new GameObject("Manager");
        AimGameManager manager = managerObj.AddComponent<AimGameManager>();
        
        manager.centerPoint = new GameObject("Center").transform;
        manager.spherePrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        manager.spherePrefab.tag = "Sphere";

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        BoxCollider playerCollider = player.AddComponent<BoxCollider>();
        playerCollider.size = Vector3.one * 2f;
        Rigidbody playerRb = player.AddComponent<Rigidbody>();
        playerRb.isKinematic = true;

        GameObject buttonObj = new GameObject("Button");
        Button button = buttonObj.AddComponent<Button>();
        BoxCollider buttonCollider = buttonObj.AddComponent<BoxCollider>();
        buttonCollider.isTrigger = true;
        buttonCollider.size = Vector3.one * 2f;

        player.transform.position = buttonObj.transform.position;

        yield return new WaitForFixedUpdate();
        yield return null;

        LogAssert.Expect(LogType.Log, "trying to start minigame");

        Object.DestroyImmediate(managerObj);
        Object.DestroyImmediate(buttonObj);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(manager.spherePrefab);
        Object.DestroyImmediate(manager.centerPoint.gameObject);
    }

    [UnityTest]
    public IEnumerator Button_IgnoresNonPlayerObjects()
    {
        GameObject buttonObj = new GameObject("Button");
        Button button = buttonObj.AddComponent<Button>();
        buttonObj.AddComponent<BoxCollider>().isTrigger = true;

        GameObject npc = new GameObject("NPC");
        npc.AddComponent<BoxCollider>();

        npc.transform.position = buttonObj.transform.position;
        yield return new WaitForFixedUpdate();

        LogAssert.NoUnexpectedReceived();

        Object.DestroyImmediate(buttonObj);
        Object.DestroyImmediate(npc);
    }
}