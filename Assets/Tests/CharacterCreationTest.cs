using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterCreationTest
{
    CharacterCreation characterCreation;

    [SetUp]
    public void Setup()
    {
        GameObject characterCreationObject = Object.Instantiate(Resources.Load<GameObject>("Character Creation"));
        characterCreation = characterCreationObject.GetComponent<CharacterCreation>();
    }

    [Test]
    public void CharacterCreationExists()
    {
        Assert.IsNotNull(characterCreation);
    }

    [Test]
    public void StartsWithIdle()
    {
        Assert.AreEqual(AnimationType.IDLE, characterCreation.LastAnimation);
    }

    [UnityTest]
    public IEnumerator HasAnInstance()
    {
        yield return new WaitForSeconds(0.1f);
        Assert.IsNotNull(CharacterCreation.Instance);
    }

    [UnityTest]
    public IEnumerator ModelExists()
    {
        yield return new WaitForSeconds(0.1f);
        Assert.IsNotNull(characterCreation.Model);
    }

    [UnityTest]
    public IEnumerator AnimatorExists()
    {
        yield return new WaitForSeconds(0.1f);
        Assert.IsNotNull(characterCreation.Animator);
    }

    [UnityTest]
    public IEnumerator SelectedAppearanceListExists()
    {
        yield return new WaitForSeconds(0.1f);
        Assert.IsNotNull(CharacterCreation.SelectedAppearance);
    }

    [UnityTest]
    public IEnumerator AllElementsHaveAtLeastOneOption()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (AppearanceElementSelector selector in characterCreation.CharacterModel.AppearanceElementSelectors)
        {
            Assert.Greater(selector.Options.Count, 0);
        }
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(characterCreation.gameObject);
    }
}