using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public CameraController MainCamera;
    public CameraController Camera2;
    public Toggle CameraSempreSeparadaToggle; 

    public Slider HPSliderControl;
    public Text HPTextControl;
    public Text GoldTextControl;
    public Sprite Player1Sprite;
    public Sprite Player1SpriteUp;
    public SpriteRenderer Player1CurrentGunSpriteRenderer;

    public Slider HPSliderPlayer2Control;
    public Text HPTextPlayer2Control;
    public Text GoldTextPlayer2Control;
    public Sprite Player2Sprite;
    public Sprite Player2SpriteUp;
    public SpriteRenderer Player2CurrentGunSpriteRenderer;

    public Button Restart1PlayerButton;
    public Button Restart2PlayerButton;
    public GameObject PlayerSpawnPoint;
    public PlayerController PlayerPrefab;

    private PlayerController player1;
    private PlayerController player2;
    private bool isMulticamera;

    public bool isPlaying;

	// Use this for initialization
	void Start () {
        AdjustToOneCamera(true);
        HPSliderControl.maxValue = 1;
        HPSliderControl.value = 0;
        HPTextControl.text = "";
        GoldTextControl.text = "";
        Restart1PlayerButton.onClick.AddListener(OnRestart1PlayerClick);
        Restart2PlayerButton.onClick.AddListener(OnRestart2PlayerClick);
    }

    private void OnRestart1PlayerClick()
    {
        Restart1PlayerButton.gameObject.SetActive(false);
        Restart2PlayerButton.gameObject.SetActive(false);
        CameraSempreSeparadaToggle.gameObject.SetActive(false);

        HPSliderPlayer2Control.gameObject.SetActive(false);

        ClearEnemies();
        ClearLoot();

        player1 = Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        player1.controllerType = Enums.ControllerType.MouseKeyboard;
        player1.lookingDownSprite = Player1Sprite;
        player1.lookingUpSprite = Player1SpriteUp;

        AdjustToOneCamera();
        MainCamera.Follow(player1);

        player2 = null;
    }

    private void AdjustToOneCamera(bool forceUpdate = false)
    {
        if (forceUpdate || isMulticamera)
        {
            isMulticamera = false;
            Camera2.gameObject.SetActive(false);
            MainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }
    }

    private void AdjustToTwoCameras(bool forceUpdate = false)
    {
        if (forceUpdate || !isMulticamera)
        {
            isMulticamera = true;
            Camera2.gameObject.SetActive(true);
            MainCamera.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
            Camera2.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    private void OnRestart2PlayerClick()
    {
        Restart1PlayerButton.gameObject.SetActive(false);
        Restart2PlayerButton.gameObject.SetActive(false);
        CameraSempreSeparadaToggle.gameObject.SetActive(false);

        HPSliderPlayer2Control.gameObject.SetActive(true);


        ClearEnemies();
        ClearLoot();

        player1 = Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        player1.controllerType = Enums.ControllerType.MouseKeyboard;
        player1.lookingDownSprite = Player1Sprite;
        player1.lookingUpSprite = Player1SpriteUp;

        AdjustToTwoCameras();
        MainCamera.Follow(player1);

        player2 = Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        player2.controllerType = Enums.ControllerType.XBoxController;
        player2.lookingDownSprite = Player2Sprite;
        player2.lookingUpSprite = Player2SpriteUp;

        Camera2.Follow(player2);
    }

    void ClearEnemies()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        foreach(EnemyController enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    void ClearLoot()
    {
        var lootItems = FindObjectsOfType<BaseItemEntity>();
        foreach (BaseItemEntity lootItem in lootItems)
        {
            Destroy(lootItem.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (player1 == null)
        {
            ClearPlayer1Info();
        } else
        {
            UpdatePlayer1Status();
        }

        if (player2 == null)
        {
            ClearPlayer2Info();

            if(player1 != null)
            {
                AdjustToOneCamera();
                MainCamera.Follow(player1);
            }

        } else
        {
            UpdatePlayer2Status();

            if (player1 == null)
            {
                AdjustToOneCamera();
                MainCamera.Follow(player2);
            }
        }

        if (!CameraSempreSeparadaToggle.isOn)  // camera unica perto e separada quando longe
        {
            AdjustCameraToCurrentPlayersDistance();
        }

        if (player1 == null && player2 == null)
        {
            if (isPlaying)
            {
                AdjustToOneCamera();
                isPlaying = false;
                Restart1PlayerButton.gameObject.SetActive(true);
                Restart2PlayerButton.gameObject.SetActive(true);
                CameraSempreSeparadaToggle.gameObject.SetActive(true);

                ClearPlayer1Info();
                ClearPlayer2Info();

            }
        } else
        {
            if (!isPlaying)
            {
                isPlaying = true;
                Restart1PlayerButton.gameObject.SetActive(false);
                Restart2PlayerButton.gameObject.SetActive(false);
                CameraSempreSeparadaToggle.gameObject.SetActive(false);
            }
        }
	}

    private void AdjustCameraToCurrentPlayersDistance()
    {
        if (player1 != null && player2 != null)
        {
            float playerDistance = Vector3.Distance(player1.gameObject.transform.position, player2.gameObject.transform.position);
            if (playerDistance > 10)
            {
                AdjustToTwoCameras();
                MainCamera.Follow(player1);
                Camera2.Follow(player2);
            }
            else
            {
                AdjustToOneCamera();
                MainCamera.Follow(player1);
            }
        }
    }

    private void ClearPlayer1Info()
    {
        HPSliderControl.maxValue = 1;
        HPSliderControl.value = 0;
        HPTextControl.text = "Game Over";
    }

    private void ClearPlayer2Info()
    {
        HPSliderPlayer2Control.maxValue = 1;
        HPSliderPlayer2Control.value = 0;
        HPTextPlayer2Control.text = "Game Over";
    }

    private void UpdatePlayer1Status()
    {
        HPSliderControl.maxValue = player1.maxHP;
        HPSliderControl.value = player1.currentHP;
        HPTextControl.text = "HP: " + player1.currentHP + "/" + player1.maxHP;
        Player1CurrentGunSpriteRenderer.sprite = player1.myGun.GetComponent<SpriteRenderer>().sprite;

        int currentGold = 0;
        if (player1.inventory != null && player1.inventory.ContainsKey(Enums.Items.Type.Money))
        {
            currentGold = player1.inventory[Enums.Items.Type.Money];
        }

        GoldTextControl.text = "Gold: " + currentGold;
    }

    private void UpdatePlayer2Status()
    {
        HPSliderPlayer2Control.maxValue = player2.maxHP;
        HPSliderPlayer2Control.value = player2.currentHP;
        HPTextPlayer2Control.text = "HP: " + player2.currentHP + "/" + player2.maxHP;
        Player2CurrentGunSpriteRenderer.sprite = player2.myGun.GetComponent<SpriteRenderer>().sprite;

        int currentGold = 0;
        if (player2.inventory != null && player2.inventory.ContainsKey(Enums.Items.Type.Money))
        {
            currentGold = player2.inventory[Enums.Items.Type.Money];
        }

        GoldTextPlayer2Control.text = "Gold: " + currentGold;
    }
}
