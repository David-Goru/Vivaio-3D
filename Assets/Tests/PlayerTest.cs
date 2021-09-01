using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
    Player player;

    [SetUp]
    public void Setup()
    {
        GameObject playerObject = Object.Instantiate(Resources.Load<GameObject>("Player"));
        player = playerObject.GetComponent<Player>();
    }

    [UnityTest]
    public IEnumerator AnimatorExists()
    {
        yield return new WaitForSeconds(0.1f);

        Assert.IsNotNull(player.Animator);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player.gameObject);
    }
}