using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject farmObject;
    [SerializeField] private GameObject aiObject;
    [SerializeField] private GameObject uiObject;
    [SerializeField] private Vector3 spawnPosition;

    [HideInInspector] public GameData data;
    [HideInInspector] public CameraController cameraController;
    [HideInInspector] public Player player;
    [HideInInspector] public Farm farm;
    [HideInInspector] public AI ai;
    [HideInInspector] public UI ui;

    public static Game Instance;

    private void Start()
    {
        Instance = this;

        data = SaveSystem.GetGameData();
        InitializeGameElements();
    }

    private void InitializeGameElements()
    {
        cameraController = Instantiate(cameraObject, spawnPosition, Quaternion.identity).GetComponent<CameraController>();
        player = Instantiate(playerObject, spawnPosition, Quaternion.identity).GetComponent<Player>();
        player.data = data.Player;
        farm = Instantiate(farmObject).GetComponent<Farm>();
        farm.data = data.Farm;
        ai = Instantiate(aiObject).GetComponent<AI>();
        ai.data = data.AI;
        ui = Instantiate(uiObject).GetComponent<UI>();
    }
}