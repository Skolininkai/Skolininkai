using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DoorTest
{
    [UnityTest]
    public IEnumerator TestOnlyOneCoroutineRuns()
    {
        var doorObject = new GameObject();
        var slidingDoor = doorObject.AddComponent<SlidingDoor>();
        slidingDoor.openPosition = new Vector3(5, 0, 0);
        slidingDoor.openSpeed = 1f;
        var originalCoroutine = slidingDoor.SlideDoor(slidingDoor.openPosition);
        var coroutineRunner = doorObject.AddComponent<CoroutineTracker>();
        coroutineRunner.StartTrackedCoroutine(originalCoroutine);
        slidingDoor.OpenDoor();
        yield return null;
        Assert.AreEqual(1, coroutineRunner.RunningCoroutinesCount);
        slidingDoor.OpenDoor();
        yield return null;
        Assert.AreEqual(1, coroutineRunner.RunningCoroutinesCount);
        coroutineRunner.StopAllCoroutines();
    }
    public class CoroutineTracker : MonoBehaviour
    {
        private int runningCoroutines = 0;
        public int RunningCoroutinesCount => runningCoroutines;
        public Coroutine StartTrackedCoroutine(IEnumerator routine)
        {
            runningCoroutines++;
            return StartCoroutine(WrapCoroutine(routine));
        }
        private IEnumerator WrapCoroutine(IEnumerator routine)
        {
            yield return routine;
            runningCoroutines--;
        }
    }
    [UnityTest]
    public IEnumerator TestDoorOpensToTargetPosition()
    {
        var door = new GameObject().AddComponent<SlidingDoor>();
        door.openPosition = new Vector3(5, 0, 0);
        door.openSpeed = 1f;
        
        door.OpenDoor();
        yield return new WaitForSeconds(1.1f); // Slightly more than needed
        
        Assert.AreEqual(door.openPosition, door.transform.position);
    }
}

