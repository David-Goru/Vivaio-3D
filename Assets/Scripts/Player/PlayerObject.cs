using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

public class PlayerObject : MonoBehaviour
{
    public LayerMask PlowableLayer;
    public AppearanceElementSelector[] AppearanceElementSelectors;
    public Transform RightHand;
    public Transform LeftHand;
    public Transform PlayerModel;

    [System.NonSerialized] public Player Data;
    private string lastAnimation = "IDLE";
    private Animator animator;
    private int handLayer;
    private bool mainHandIsLeft;

    private void Start()
    {
        animator = PlayerModel.GetComponent<Animator>();
        handLayer = animator.GetLayerIndex("MainHandInUse");
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Translate(Vector3 direction)
    {
        transform.Translate(direction);
    }

    public void LookAt(Vector3 direction)
    {
        Vector3 position = transform.position + direction * 1000;
        position = new Vector3(position.x, PlayerModel.position.y, position.z);
        PlayerModel.LookAt(position);
    }

    public void SetAnimation(string animation)
    {
        if (lastAnimation == animation) return;

        animator.SetTrigger(animation);
        lastAnimation = animation;
    }

    public float DistanceFrom(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    public void SetModel(List<KeyValuePair<string, string>> bodyElements)
    {
        foreach (KeyValuePair<string, string> bodyElement in bodyElements)
        {
            if (bodyElement.Value == "None") continue;
            PlayerModel.Find(bodyElement.Key).Find(bodyElement.Value).gameObject.SetActive(true);
        }
    }

    public void HideBodyElement(KeyValuePair<string, string> bodyElement)
    {
        if (bodyElement.Value == "None") return;

        Transform model = transform.Find("Player model");
        model.Find(bodyElement.Key).Find(bodyElement.Value).gameObject.SetActive(false);
    }

    public void ShowBodyElement(KeyValuePair<string, string> bodyElement)
    {
        if (bodyElement.Value == "None") return;

        Transform model = transform.Find("Player model");
        model.Find(bodyElement.Key).Find(bodyElement.Value).gameObject.SetActive(true);
    }

    public void SetHandInUse()
    {
        changeHandWeight(1.0f);
    }

    public void UnSetHandInUse()
    {
        changeHandWeight(0.0f);
    }

    public void ChangeMainHand(string type = "Default")
    {
        if (type == "Left") mainHandIsLeft = true;
        else if (type == "Right") mainHandIsLeft = false;
        else mainHandIsLeft = !mainHandIsLeft;

        animator.SetBool("LeftHand", mainHandIsLeft);
    }

    private void changeHandWeight(float weightValue)
    {
        animator.SetLayerWeight(handLayer, weightValue);
    }
}