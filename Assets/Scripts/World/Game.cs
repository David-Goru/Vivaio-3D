using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject farmObject;
    [SerializeField] private GameObject aiObject;
    [SerializeField] private GameObject uiObject;

    private GameData data;
    private CameraController cameraController;
    private Player player;
    private Farm farm;
    private AI ai;
    private UI ui;

    public GameData Data { get => data; set => data = value; }
    public CameraController CameraController { get => cameraController; set => cameraController = value; }
    public Player Player { get => player; }
    public Farm Farm { get => farm; set => farm = value; }
    public UI Ui { get => ui; set => ui = value; }

    public static Game Instance;

    private void Start()
    {
        Instance = this;

        data = SaveSystem.GetGameData();
        initializeGameElements();
    }

    private void initializeGameElements()
    {
        cameraController = Instantiate(cameraObject).GetComponent<CameraController>();
        player = Instantiate(playerObject).GetComponent<Player>();
        player.Data = data.Player;
        farm = Instantiate(farmObject).GetComponent<Farm>();
        farm.Data = data.Farm;
        ai = Instantiate(aiObject).GetComponent<AI>();
        ai.Data = data.AI;
        ui = Instantiate(uiObject).GetComponent<UI>();
    }
}