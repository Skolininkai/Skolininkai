using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraTest
{
    private GameObject player;
    private PlayerLook playerLook;
    private Transform testCam;
    private Transform testOrientation;

    [SetUp]
    public void Setup()
    {
        player = new GameObject("Player");
        playerLook = player.AddComponent<PlayerLook>();
        testCam = new GameObject("Camera").transform;
        testOrientation = new GameObject("Orientation").transform;
        typeof(PlayerLook).GetField("cam", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance)
            .SetValue(playerLook, testCam);            
        typeof(PlayerLook).GetField("orientation", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance)
            .SetValue(playerLook, testOrientation);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(testCam.gameObject);
        Object.DestroyImmediate(testOrientation.gameObject);
    }

    [Test]
    public void SimulateMouseInput_ClampsVerticalRotation()
    {
        #if UNITY_INCLUDE_TESTS
        playerLook.SimulateMouseInput(0f, -100f);
        Assert.AreEqual(90f, testCam.rotation.eulerAngles.x, 0.1f, 
            "Should clamp to 90 degrees");
        #endif
    }

    [Test]
    public void SimulateMouseInput_RotatesHorizontally()
    {
        #if UNITY_INCLUDE_TESTS
        float initialRotation = testOrientation.rotation.eulerAngles.y;
        playerLook.SimulateMouseInput(1f, 0f);
        float newRotation = testOrientation.rotation.eulerAngles.y;
        float expectedRotation = initialRotation + 1f;
        if (expectedRotation > 360) expectedRotation -= 360;
        if (newRotation > 360) newRotation -= 360;        
        Assert.AreEqual(expectedRotation, newRotation, 0.1f,
            "Should rotate right by 1 degree with sensitivity 100");
        Assert.AreEqual(newRotation, testCam.rotation.eulerAngles.y, 0.1f,
            "Camera should match orientation's y-rotation");
        #endif
    }
}