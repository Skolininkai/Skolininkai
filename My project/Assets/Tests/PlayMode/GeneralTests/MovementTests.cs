using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MovementTests
{
    private GameObject playerObj;
    private PlayerMovement playerMovement;
    private Rigidbody rb;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        playerObj = new GameObject("Player");
        rb = playerObj.AddComponent<Rigidbody>();
        playerMovement = playerObj.AddComponent<PlayerMovement>();

        playerMovement.TestOrientation = new GameObject("Orientation").transform;
        playerMovement.TestPlayer = playerObj;
        playerMovement.TestCameraPosition = new GameObject("CameraPosition");
        playerMovement.TestGroundCheck = new GameObject("GroundCheck").transform;
        playerMovement.TestGroundMask = LayerMask.GetMask("Default");
        playerMovement.TestPlayerCollider = playerObj.AddComponent<CapsuleCollider>();

        playerMovement.jumpForce = 5f;
        playerMovement.TestPlayerCollider.height = 2f;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(playerMovement.TestOrientation.gameObject);
        Object.DestroyImmediate(playerMovement.TestCameraPosition);
        Object.DestroyImmediate(playerMovement.TestGroundCheck.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Jump_AppliesVerticalForce_WhenGrounded()
    {
        playerMovement.TestGroundCheck.position = Vector3.down * 0.1f;
        yield return new WaitForFixedUpdate();

        float initialY = rb.linearVelocity.y;
        playerMovement.Jump();
        yield return new WaitForFixedUpdate();

        Assert.Greater(rb.linearVelocity.y, initialY, "Jump force not applied");
    }

    [UnityTest]
    public IEnumerator Jump_Blocked_WhenNotGrounded()
    {
        playerObj.transform.position = new Vector3(0, 10f, 0);
        yield return new WaitForFixedUpdate();

        Vector3 initialVelocity = rb.linearVelocity;
        playerMovement.Jump();
        yield return null;

        Assert.AreEqual(initialVelocity, rb.linearVelocity, "Jump occurred while airborne");
    }

    [UnityTest]
    public IEnumerator IsGrounded_True_WhenNearGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = Vector3.zero;
        playerObj.transform.position = new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(playerMovement.TestIsGrounded, "Ground not detected");
        Object.DestroyImmediate(ground);
    }

    [UnityTest]
    public IEnumerator IsGrounded_False_AfterJump()
    {
        playerMovement.TestGroundCheck.position = Vector3.down * 0.1f;
        yield return new WaitForFixedUpdate();

        playerMovement.Jump();
        yield return new WaitForSeconds(0.2f);

        Assert.IsFalse(playerMovement.TestIsGrounded, "Player still grounded after jump");
    }

    [UnityTest]
    public IEnumerator Crouch_ReducesColliderHeightAndLowersCamera()
    {
        float originalHeight = playerMovement.TestPlayerCollider.height;
        float originalCameraY = playerMovement.TestCameraPosition.transform.localPosition.y;

        playerMovement.Crouch();
        yield return null;

        Assert.AreEqual(
            playerMovement.TestCrouchHeight, 
            playerMovement.TestPlayerCollider.height, 
            "Collider height not reduced"
        );
        Assert.AreEqual(
            playerMovement.TestCrouchCameraY, 
            playerMovement.TestCameraPosition.transform.localPosition.y, 
            0.01f, 
            "Camera not lowered"
        );
    }

    [UnityTest]
    public IEnumerator Uncrouch_Fails_IfObstructed()
    {
        playerMovement.Crouch();
        yield return null;

        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.transform.position = playerObj.transform.position + 
            Vector3.up * (playerMovement.TestCrouchHeight + 0.2f);
        yield return null;

        playerMovement.Uncrouch();
        yield return new WaitForFixedUpdate();

        Assert.IsTrue(
            playerMovement.TestPlayerCollider.height == playerMovement.TestCrouchHeight, 
            "Player uncrouched despite obstruction"
        );
        Object.DestroyImmediate(ceiling);
    }
}