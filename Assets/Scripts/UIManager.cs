using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Slider HPSliderControl;
    public Text HPTextControl;
    public Text GoldTextControl;
    public Sprite Player1Sprite;
    public Sprite Player1SpriteUp;

    public Slider HPSliderPlayer2Control;
    public Text HPTextPlayer2Control;
    public Text GoldTextPlayer2Control;
    public Sprite Player2Sprite;
    public Sprite Player2SpriteUp;

    public Button Restart1PlayerButton;
    public Button Restart2PlayerButton;
    public GameObject PlayerSpawnPoint;
    public PlayerController PlayerPrefab;

    private PlayerController player1;
    private PlayerController player2;

    public bool isPlaying;

	// Use this for initialization
	void Start () {
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
        HPSliderPlayer2Control.gameObject.SetActive(false);

        ClearEnemies();
        ClearLoot();

        player1 = Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        player1.controllerType = Enums.ControllerType.MouseKeyboard;
        player1.lookingDownSprite = Player1Sprite;
        player1.lookingUpSprite = Player1SpriteUp;

        SetCameraTarget(player1);

        player2 = null;
    }

    private void SetCameraTarget(PlayerController player)
    {
        var camera = FindObjectOfType<CameraController>();
        if (camera != null)
        {
            camera.Follow(player1);
        }
        else
        {
            Debug.LogError("CANT FIND CAMERA");
        }
    }

    private void OnRestart2PlayerClick()
    {
        Restart1PlayerButton.gameObject.SetActive(false);
        Restart2PlayerButton.gameObject.SetActive(false);
        HPSliderPlayer2Control.gameObject.SetActive(true);

        ClearEnemies();
        ClearLoot();

        player1 = Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        player1.controllerType = Enums.ControllerType.MouseKeyboard;
        player1.lookingDownSprite = Player1Sprite;
        player1.lookingUpSprite = Player1SpriteUp;

        SetCameraTarget(player1);

        player2 = Instantiate(PlayerPrefab, new Vector3(PlayerSpawnPoint.transform.position.x + 3, PlayerSpawnPoint.transform.position.y, PlayerSpawnPoint.transform.position.z), PlayerSpawnPoint.transform.rotation);
        player2.controllerType = Enums.ControllerType.XBoxController;
        player2.lookingDownSprite = Player2Sprite;
        player2.lookingUpSprite = Player2SpriteUp;
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
        } else
        {
            UpdatePlayer2Status();
            if (player1 == null )
            {
                SetCameraTarget(player2);
            }
        }

        if (player1 == null && player2 == null)
        {
            if (isPlaying)
            {
                isPlaying = false;
                Restart1PlayerButton.gameObject.SetActive(true);
                Restart2PlayerButton.gameObject.SetActive(true);

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

        int currentGold = 0;
        if (player2.inventory != null && player2.inventory.ContainsKey(Enums.Items.Type.Money))
        {
            currentGold = player2.inventory[Enums.Items.Type.Money];
        }

        GoldTextPlayer2Control.text = "Gold: " + currentGold;
    }
}
