using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject farmObject;
    [SerializeField] private GameObject aiObject;
    [SerializeField] private GameObject uiObject;

    [HideInInspector] public GameData Data;
    [HideInInspector] public CameraController CameraController;
    [HideInInspector] public Player Player;
    [HideInInspector] public Farm Farm;
    [HideInInspector] public AI AI;
    [HideInInspector] public UI UI;

    public static Game Instance;

    private void Start()
    {
        Instance = this;

        Data = SaveSystem.GetGameData();
        initializeGameElements();
    }

    private void initializeGameElements()
    {
        CameraController = Instantiate(cameraObject).GetComponent<CameraController>();
        Player = Instantiate(playerObject).GetComponent<Player>();
        Player.Data = Data.Player;
        Farm = Instantiate(farmObject).GetComponent<Farm>();
        Farm.Data = Data.Farm;
        AI = Instantiate(aiObject).GetComponent<AI>();
        AI.Data = Data.AI;
        UI = Instantiate(uiObject).GetComponent<UI>();
    }
}