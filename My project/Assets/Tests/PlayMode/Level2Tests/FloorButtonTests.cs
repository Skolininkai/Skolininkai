using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FloorButtonTests
{
    private GameObject buttonObject;
    private GameObject playerObject;
    private FloorButton floorButton;
    private TestLevel2Manager testManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create and configure TestLevel2Manager
        var managerGO = new GameObject("Level2Manager");
        testManager = managerGO.AddComponent<TestLevel2Manager>();

        // Create FloorButton
        buttonObject = new GameObject("FloorButton");
        var collider = buttonObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        floorButton = buttonObject.AddComponent<FloorButton>();
        floorButton.buttonIndex = 2;

        // Create Player
        playerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playerObject.tag = "Player";
        playerObject.AddComponent<Rigidbody>();
        playerObject.transform.position = Vector3.zero;
        buttonObject.transform.position = Vector3.zero;

        testManager.Reset();
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerStepsOnButton_WhenInputAllowed_CallsPlayerPress()
    {
        testManager.Reset();
        testManager.allowInput = true;
        testManager.showPattern = false;
        testManager.waitForStart = false;

        yield return MovePlayerOntoButton();

        Assert.IsTrue(testManager.wasCalled);
        Assert.AreEqual(2, testManager.indexPressed);
    }

    [UnityTest]
    public IEnumerator PlayerStepsOnButton_WhenPatternShowing_DoesNotCallPlayerPress()
    {
        testManager.Reset();
        testManager.allowInput = true;
        testManager.showPattern = true;
        testManager.waitForStart = false;

        yield return MovePlayerOntoButton();

        Assert.IsFalse(testManager.wasCalled);
    }

    [UnityTest]
    public IEnumerator PlayerStepsOnButton_WhenStartButtonWaiting_DoesNotCallPlayerPress()
    {
        testManager.Reset();
        testManager.allowInput = true;
        testManager.showPattern = false;
        testManager.waitForStart = true;

        yield return MovePlayerOntoButton();

        Assert.IsFalse(testManager.wasCalled);
    }

    [UnityTest]
    public IEnumerator PlayerStepsOnButton_WhenNotWaitingForInput_DoesNotCallPlayerPress()
    {
        testManager.Reset();
        testManager.allowInput = false;
        testManager.showPattern = false;
        testManager.waitForStart = false;

        yield return MovePlayerOntoButton();

        Assert.IsFalse(testManager.wasCalled);
    }

    [UnityTest]
    public IEnumerator NonPlayerObject_DoesNotTrigger()
    {
        testManager.Reset();
        playerObject.tag = "Untagged";

        testManager.allowInput = true;
        testManager.showPattern = false;
        testManager.waitForStart = false;

        yield return MovePlayerOntoButton();

        Assert.IsFalse(testManager.wasCalled);
    }

    private IEnumerator MovePlayerOntoButton()
    {
        // Move player away first
        playerObject.transform.position = Vector3.up * 5;
        yield return null;

        // Then move back onto button
        playerObject.transform.position = buttonObject.transform.position;
        yield return new WaitForFixedUpdate();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(buttonObject);
        Object.Destroy(playerObject);
        Object.Destroy(testManager.gameObject);
        yield return null;
    }

    private class TestLevel2Manager : Level2Manager
    {
        public bool allowInput = true;
        public bool showPattern = false;
        public bool waitForStart = false;

        public bool wasCalled = false;
        public int indexPressed = -1;

        public override bool IsShowingPattern()
        {
            return showPattern;
        }
        public override bool IsWaitingForStartButton()
        {
            return waitForStart;        
        }
        public override bool IsWaitingForInput()
        {
            return allowInput;
        }

        public override void Start() { }

        public override void PlayerPress(int index)
        {
            wasCalled = true;
            indexPressed = index;
        }

        public void Reset()
        {
            wasCalled = false;
            indexPressed = -1;
            allowInput = true;
            showPattern = false;
            waitForStart = false;
        }
    }
}
