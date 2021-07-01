using UnityEngine;
using System.Collections.Generic;

public class PlayerObject : MonoBehaviour
{
    public LayerMask PlowableLayer;
    public GameObject[] AppearanceElements;
    public Transform RightHand;
    public Transform LeftHand;
    public Transform PlayerModel;
    public Animator Animator;

    [System.NonSerialized] public Player Data;
    private string lastAnimation = "IDLE";

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

        Animator.SetTrigger(animation);
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
}