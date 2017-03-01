using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Slider HPSliderControl;
    public Text HPTextControl;
    public Text GoldTextControl;
    public Button RestartButton;
    public GameObject PlayerSpawnPoint;
    public PlayerController PlayerPrefab;

    private BaseEntity targetToFollow;

    public bool isPlaying;

	// Use this for initialization
	void Start () {
        HPSliderControl.maxValue = 1;
        HPSliderControl.value = 0;
        HPTextControl.text = "";
        GoldTextControl.text = "";
        RestartButton.onClick.AddListener(OnRestartClick);
	}

    private void OnRestartClick()
    {
        RestartButton.gameObject.SetActive(false);

        ClearEnemies();
        ClearLoot();

        Instantiate(PlayerPrefab, PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
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
		if (targetToFollow != null)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                RestartButton.gameObject.SetActive(false);
            }

            HPSliderControl.maxValue = targetToFollow.maxHP;
            HPSliderControl.value = targetToFollow.currentHP;
            HPTextControl.text = "HP: " + targetToFollow.currentHP + "/" + targetToFollow.maxHP;

            int currentGold = 0;
            if(targetToFollow.inventory.ContainsKey(Enums.Items.Type.Money))
            {
                currentGold = targetToFollow.inventory[Enums.Items.Type.Money];
            }

            GoldTextControl.text = "Gold: " + currentGold;
        } else
        {
            if (isPlaying)
            {
                isPlaying = false;
                RestartButton.gameObject.SetActive(true);

                HPSliderControl.maxValue = 1;
                HPSliderControl.value = 0;
                HPTextControl.text = "Game Over";
            }
        }
	}

    public void Follow(BaseEntity target)
    {
        targetToFollow = target;
    }

    public void Unfollow(BaseEntity target)
    {
        targetToFollow = null;
    }
}
