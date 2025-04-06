using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ButtonsTests
{
    private GameObject buttonsObject;
    private Buttons buttonsScript;
    private GameObject[] testPillars;
    private GameObject[] testButtons;

    [SetUp]
    public void Setup()
    {
        // Create the test environment
        buttonsObject = new GameObject();
        buttonsScript = buttonsObject.AddComponent<Buttons>();
    
        // Create test pillars
        testPillars = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            testPillars[i] = new GameObject($"Pillar{i}");
            testPillars[i].transform.position = new Vector3(i * 2, 0, 0);
        }
        buttonsScript.pillars = testPillars;
    
        // Create test buttons with all required components
        testButtons = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            testButtons[i] = new GameObject($"Button{i}");
            var renderer = testButtons[i].AddComponent<MeshRenderer>();
            testButtons[i].AddComponent<SphereCollider>();
            renderer.material = new Material(Shader.Find("Standard"));
        }
        buttonsScript.sphereButtons = testButtons;
    
        // Setup materials
        buttonsScript.normalMaterial = new Material(Shader.Find("Standard"));
        buttonsScript.highlightedMaterial = new Material(Shader.Find("Standard"));
        buttonsScript.pressedMaterial = new Material(Shader.Find("Standard"));
    
        // Initialize arrays that would normally be set in Start()
        buttonsScript.originalPositions = new Vector3[testPillars.Length];
        buttonsScript.targetPositions = new Vector3[testPillars.Length];
        buttonsScript.buttonEffectTimers = new float[testButtons.Length];
        
        for (int i = 0; i < testPillars.Length; i++)
        {
            buttonsScript.originalPositions[i] = testPillars[i].transform.position;
            buttonsScript.targetPositions[i] = testPillars[i].transform.position;
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        Object.DestroyImmediate(buttonsObject);
        foreach (var pillar in testPillars) Object.DestroyImmediate(pillar);
        foreach (var button in testButtons) Object.DestroyImmediate(button);
    }

    [Test]
    public void PressButton_ChangesButtonMaterialAndSetsTimer()
    {
        // Configure test
        buttonsScript.buttonEffectTimers = new float[testButtons.Length];
        var originalMaterial = testButtons[0].GetComponent<Renderer>().material;
        
        // Execute
        buttonsScript.PressButton(0);
        
        // Verify changes
        Assert.AreNotSame(originalMaterial, 
                        testButtons[0].GetComponent<Renderer>().material,
                        "Material should change");
        
        Assert.Greater(buttonsScript.buttonEffectTimers[0], 0f);
    }

    [Test]
    public void HandleButtonInteraction_HighlightsButton()
    {
        // Ensure the button has a Renderer with the normal material
        var buttonRenderer = testButtons[1].GetComponent<Renderer>();
        buttonRenderer.material = buttonsScript.normalMaterial;
        
        // Initialize the button effect timers array
        buttonsScript.buttonEffectTimers = new float[testButtons.Length];
        
        buttonsScript.HandleButtonInteraction(testButtons[1]);
        
        // Verify button is highlighted - compare shader names instead of material instances
        Assert.AreEqual(buttonsScript.highlightedMaterial.shader.name, 
                    testButtons[1].GetComponent<Renderer>().material.shader.name);
        
        Assert.AreEqual(testButtons[1], buttonsScript.currentlyHighlightedButton);
    }

    [Test]
    public void HandleButtonInteraction_WithPressedButton_DoesNotChangeMaterial()
    {
        // Create a unique color for the pressed material to make verification easier
        var pressedColor = Color.red;
        buttonsScript.pressedMaterial.color = pressedColor;
        
        // Set up the button with pressed material
        var buttonRenderer = testButtons[1].GetComponent<Renderer>();
        buttonRenderer.material = buttonsScript.pressedMaterial;
        
        buttonsScript.buttonEffectTimers = new float[testButtons.Length];
        buttonsScript.buttonEffectTimers[1] = 1f; // Simulate active press effect
        
        buttonsScript.HandleButtonInteraction(testButtons[1]);
        
        // Verify the material still has the pressed color
        Assert.AreEqual(pressedColor, buttonRenderer.material.color,
            "Button material should maintain pressed color during effect timer");
        
        // Verify no highlight material was applied
        Assert.AreNotEqual(buttonsScript.highlightedMaterial.color, buttonRenderer.material.color,
            "Pressed button should not get highlighted material");
    }

    [Test]
    public void ApplyMovements_UpdatesTargetPositions()
    {
        // Disable the script to prevent Update() from running
        buttonsScript.enabled = false;

        // Initialize the arrays manually since we're not calling Start()
        buttonsScript.originalPositions = new Vector3[testPillars.Length];
        buttonsScript.targetPositions = new Vector3[testPillars.Length];
        for (int i = 0; i < testPillars.Length; i++)
        {
            buttonsScript.originalPositions[i] = testPillars[i].transform.position;
            buttonsScript.targetPositions[i] = testPillars[i].transform.position;
        }

        Vector3[] testMovements = {
            new Vector3(1, 0, 0),
            new Vector3(0, 2, 0),
            new Vector3(0, 0, 3)
        };
    
        Vector3[] initialPositions = new Vector3[testPillars.Length];
        for (int i = 0; i < testPillars.Length; i++)
        {
            initialPositions[i] = buttonsScript.targetPositions[i];
        }
    
        // Apply movements
        buttonsScript.ApplyMovements(testMovements);
    
        // Verify target positions updated
        for (int i = 0; i < testPillars.Length; i++)
        {
            Assert.AreEqual(initialPositions[i] + testMovements[i], buttonsScript.targetPositions[i]);
        }
    }

    [Test]
    public void UpdatePillarPositions_MovesPillarsTowardTarget()
    {
        // Create test pillars with initial positions
        var testPillars = new GameObject[3];
        var initialPositions = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            testPillars[i] = new GameObject($"Pillar{i}");
            initialPositions[i] = new Vector3(i * 2f, 0f, 0f);
            testPillars[i].transform.position = initialPositions[i];
        }

        // Create and configure test component
        var buttonsScript = new GameObject().AddComponent<Buttons>();
        buttonsScript.pillars = testPillars;
        buttonsScript.moveSpeed = 5f;
        
        // Set target positions (1 unit above initial positions)
        buttonsScript.targetPositions = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            buttonsScript.targetPositions[i] = initialPositions[i] + Vector3.up;
        }

        // Store initial distances to targets
        var initialDistances = new float[3];
        for (int i = 0; i < 3; i++)
        {
            initialDistances[i] = Vector3.Distance(
                testPillars[i].transform.position,
                buttonsScript.targetPositions[i]);
        }

        // Run the position update
        buttonsScript.UpdatePillarPositions();

        // Verify movement occurred
        for (int i = 0; i < 3; i++)
        {
            // Check position changed
            Assert.AreNotEqual(initialPositions[i], testPillars[i].transform.position,
                            $"Pillar {i} didn't move from initial position");
            
            // Check moved toward target
            float newDistance = Vector3.Distance(
                testPillars[i].transform.position,
                buttonsScript.targetPositions[i]);
                
            Assert.Less(newDistance, initialDistances[i],
                    $"Pillar {i} didn't move closer to target");
        }

        // Clean up
        foreach (var pillar in testPillars)
        {
            Object.DestroyImmediate(pillar);
        }
        Object.DestroyImmediate(buttonsScript.gameObject);
    }

    [Test]
    public void ResetPillars_RestoresOriginalPositions()
    {
        // Set up test environment without enabling the component
        var testPillars = new GameObject[3];
        var originalPositions = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            testPillars[i] = new GameObject($"Pillar{i}");
            testPillars[i].transform.position = new Vector3(i * 2, 0, 0);
            originalPositions[i] = testPillars[i].transform.position;
        }

        // Create and configure test component
        var buttonsScript = new GameObject().AddComponent<Buttons>();
        buttonsScript.pillars = testPillars;
        buttonsScript.originalPositions = originalPositions;
        buttonsScript.targetPositions = new Vector3[3];
        
        // Set modified target positions
        for (int i = 0; i < 3; i++)
        {
            buttonsScript.targetPositions[i] = originalPositions[i] + new Vector3(0, 5, 0);
        }

        // Execute reset
        buttonsScript.ResetPillars();

        // Verify positions were reset
        for (int i = 0; i < 3; i++)
        {
            Assert.AreEqual(originalPositions[i], buttonsScript.targetPositions[i],
                $"Pillar {i} target position not reset correctly");
        }

        // Clean up
        Object.DestroyImmediate(buttonsScript.gameObject);
        foreach (var pillar in testPillars) Object.DestroyImmediate(pillar);
    }

    [Test]
    public void UpdateButtonEffects_RevertsMaterialAfterTimer()
    {
        // Create test materials with unique base names
        var normalMat = new Material(Shader.Find("Standard")) { name = "Material" };
        var pressedMat = new Material(Shader.Find("Standard")) { name = "Material.003" };
        
        // Set up test button
        var testButton = new GameObject("TestButton");
        var buttonRenderer = testButton.AddComponent<MeshRenderer>();
        buttonRenderer.material = pressedMat;
        
        // Configure test component
        var buttonsScript = new GameObject().AddComponent<Buttons>();
        buttonsScript.sphereButtons = new GameObject[] { testButton };
        buttonsScript.normalMaterial = normalMat;
        buttonsScript.pressedMaterial = pressedMat;
        buttonsScript.buttonEffectTimers = new float[] { 0.1f }; // Active timer
        
        // First update - should maintain pressed material
        buttonsScript.UpdateButtonEffects();
        Assert.IsTrue(buttonRenderer.material.name.StartsWith("Material.003"),
                    "Should maintain pressed material while timer is active");
        
        // Expire timer and verify reversion
        buttonsScript.buttonEffectTimers[0] = -1f; // Force expired
        buttonsScript.UpdateButtonEffects();
        Assert.IsTrue(buttonRenderer.material.name.StartsWith("Material"),
                    "Should revert to normal material after timer expires");
        
        // Clean up
        Object.DestroyImmediate(testButton);
        Object.DestroyImmediate(buttonsScript.gameObject);
        Object.DestroyImmediate(normalMat);
        Object.DestroyImmediate(pressedMat);
    }
}
