using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class ManagerTests
{
    private GameObject gameObj;
    private Level2Manager levelManager;

    [SetUp]
    public void Setup()
    {
        gameObj = new GameObject();
        levelManager = gameObj.AddComponent<Level2Manager>();


        // Fake assign arrays so it doesn't crash
        levelManager.screenPanels = new GameObject[4];
        levelManager.floorButtons = new GameObject[4];
        levelManager.stepLights = new Light[5];

        for (int i = 0; i < 4; i++)
        {
            levelManager.screenPanels[i] = new GameObject();
            levelManager.floorButtons[i] = new GameObject();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject lightObj = new GameObject();
            levelManager.stepLights[i] = lightObj.AddComponent<Light>();
        }
    }

    [UnityTest]
    public IEnumerator PatternIsGeneratedOnStart()
    {
        levelManager.Start();
        yield return null;

        var sequenceField = typeof(Level2Manager).GetField("sequence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var sequence = (int[])sequenceField.GetValue(levelManager);

        Assert.NotNull(sequence);
        Assert.AreEqual(5, sequence.Length);
    }

    [UnityTest]
    public IEnumerator StepCompleted_AdvancesStepAndUpdatesLight()
    {
        levelManager.Start();

        // Simulate private state
        var sequenceField = typeof(Level2Manager).GetField("sequence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        sequenceField.SetValue(levelManager, new int[] { 0, 1, 2, 3, 0 });

        var stepField = typeof(Level2Manager).GetField("step", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        stepField.SetValue(levelManager, 1);

        var currentIndexField = typeof(Level2Manager).GetField("currentIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        currentIndexField.SetValue(levelManager, 0);

        var waitingForInputField = typeof(Level2Manager).GetField("waitingForInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        waitingForInputField.SetValue(levelManager, true);

        var playerTurnField = typeof(Level2Manager).GetField("playerTurn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        playerTurnField.SetValue(levelManager, true);

        levelManager.PlayerPress(0);

        yield return new WaitForSeconds(1.1f); // wait for cooldown to end

        var newStep = (int)stepField.GetValue(levelManager);
        Assert.AreEqual(2, newStep);

        var light = levelManager.stepLights[0];
        Assert.AreEqual(Color.green, light.color);
    }

    [UnityTest]
    public IEnumerator WrongInput_ResetsGame()
    {
        levelManager.Start();

        var sequenceField = typeof(Level2Manager).GetField("sequence", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        sequenceField.SetValue(levelManager, new int[] { 0, 1, 2, 3, 0 });

        typeof(Level2Manager).GetField("step", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(levelManager, 1);
        typeof(Level2Manager).GetField("currentIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(levelManager, 0);
        typeof(Level2Manager).GetField("waitingForInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(levelManager, true);
        typeof(Level2Manager).GetField("playerTurn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(levelManager, true);

        levelManager.PlayerPress(1); // Wrong input

        yield return null;

        var step = (int)typeof(Level2Manager).GetField("step", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(levelManager);
        Assert.AreEqual(1, step);
    }
}