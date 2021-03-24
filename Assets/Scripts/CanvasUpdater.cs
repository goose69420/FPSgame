using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdater : MonoBehaviour
{
    public Transform respawnPosition;
    public GameObject PlayerPrefab;


    public GameObject Player;
    public PlayerMovement playerMovement;
    public GameObject gameMenu;
    public Slider HPSlider;
    public Text HPText;
    public GameObject deadPanel;
    public Button respawnButton;

    public GameObject gameControl;
    public Text fps;
    public Button options;
    public Button backFromOptions;
    public GameObject escMenu;

    public GameObject optionsMenu;
    public Toggle showFps;

    public GameObject Camera;
    public Slider mouseSensSliderHorizontal;
    public Text mouseSensHorizontal;

    public Slider mouseSensSliderVertical;
    public Text mouseSensVertical;

    public VolumetricLightRenderer VolumetricL;
    public Text Resolution;

    float deltaTime;

    float escPresses;

    public Text Ammo;
    playerPickup Pickup;
    // Start is called before the first frame update
    void Start()
    {
        VolumetricL = Camera.GetComponent<VolumetricLightRenderer>();
        playerMovement = Player.GetComponent<PlayerMovement>();

        options.onClick.AddListener(delegate () { onClickOptions(); });
        backFromOptions.onClick.AddListener(delegate () { backToEscMenu(); });
        respawnButton.onClick.AddListener(delegate () { respawn(); });
        optionsMenu.SetActive(false);
        gameMenu.SetActive(true);
        deadPanel.SetActive(false);
        Pickup = Camera.GetComponent<playerPickup>();
    }

    

    // Update is called once per frame
    void Update()
    {
        AmmoUpdate();
        manageHP();
        ManageVolumetricLight();

        if (mouseSensSliderHorizontal.value == 0) mouseSensSliderHorizontal.value = 0.01f;
        float sliderSensH = Mathf.Ceil(mouseSensSliderHorizontal.value * 100f);
        Camera.GetComponent<MouseLook>().mouseHSpeed = sliderSensH * 10f;
        mouseSensHorizontal.text = $"Horizontal mouse sensivity: {sliderSensH}";

        if (mouseSensSliderVertical.value == 0) mouseSensSliderVertical.value = 0.01f;
        float sliderSensV = Mathf.Ceil(mouseSensSliderVertical.value * 100f);
        Camera.GetComponent<MouseLook>().mouseVSpeed = sliderSensV * 10f;
        mouseSensVertical.text = $"Vertical mouse sensivity: {sliderSensV}";


        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float framesPerSecond = 1.0f / deltaTime;
        fps.text = $"FPS: {Mathf.Ceil(framesPerSecond)}";


        if (playerMovement.alive)
        {


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escPresses++;
            }


            //ЗАКРЫВАЕМ МЕНЮШКИ
            if (Input.GetKeyUp(KeyCode.Escape) && escPresses % 2 == 0)
            {
                gameControl.GetComponent<GameControl>().paused = false;
                escMenu.SetActive(false);
                optionsMenu.SetActive(false);
                gameMenu.SetActive(true);
            }

            //ОТКРЫВАЕМ МЕНЮШКИ
            if (Input.GetKeyDown(KeyCode.Escape) && escPresses % 2 == 1)
            {

                gameControl.GetComponent<GameControl>().paused = true;
                escMenu.SetActive(true);
                gameMenu.SetActive(false);
            }
            fps.gameObject.SetActive(showFps.isOn);

            escPresses = escPresses % 2;
        } else
        {
            escMenu.SetActive(false);
            gameMenu.SetActive(false);
            deadPanel.SetActive(true);
        }
    }
    public void onClickOptions()
    {
        escMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void backToEscMenu()
    {
        escMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    public void manageHP()
    {
        Mathf.Clamp(playerMovement.currentHP, 0, playerMovement.maxHP);
        HPSlider.maxValue = playerMovement.maxHP;
        HPSlider.value = playerMovement.currentHP;
        HPText.text = $"HP: {Mathf.Round(HPSlider.value)}/{HPSlider.maxValue}";
    }

    private VolumetricLightRenderer.VolumtericResolution FullHalfQuarterToVolumetricResolution(string label)
    {
        if (label == "Full") return VolumetricLightRenderer.VolumtericResolution.Full;
        if (label == "Half") return VolumetricLightRenderer.VolumtericResolution.Half;
        if (label == "Quarter") return VolumetricLightRenderer.VolumtericResolution.Quarter;
        return VolumetricLightRenderer.VolumtericResolution.Disabled;
    }

    private bool VolumetricLEnabled()
    {
        return FullHalfQuarterToVolumetricResolution(Resolution.text) != VolumetricLightRenderer.VolumtericResolution.Disabled;
    }

    public void ManageVolumetricLight()
    {
        if(VolumetricLEnabled())
        {
            VolumetricL.enabled = true;
            VolumetricL.Resolution = FullHalfQuarterToVolumetricResolution(Resolution.text);
        } else
        {
            VolumetricL.enabled = false;
        }
        
    }
    public void respawn()
    {
        gameControl.GetComponent<GameControl>().paused = false;
        
        Instantiate(Player, respawnPosition.position + new Vector3(0, 1.5f, 0), new Quaternion());
        Destroy(Player);
        //
        GameObject newPlayer = FindObjectOfType<PlayerMovement>().gameObject;
        FindObjectOfType<PlayerMovement>().gameManager = FindObjectOfType<GameControl>().gameObject;
        Player = newPlayer;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.currentHP = playerMovement.maxHP;
        playerMovement.alive = true;
        FindObjectOfType<MouseLook>().gameControl = FindObjectOfType<GameControl>().gameObject;
        Camera = FindObjectOfType<MouseLook>().gameObject;
        VolumetricL = Camera.GetComponent<VolumetricLightRenderer>();
        gameMenu.SetActive(true);
        deadPanel.SetActive(false);
        Pickup = Camera.GetComponent<playerPickup>();
    }
    
    private void AmmoUpdate()
    {
        if(Pickup != null && Pickup.item != null && Pickup?.PController?.isGun == true)
        {
            Ammo.enabled = true;
            Ammo.text = $"Ammo: {Pickup.PController.CurrentAmmoInMagazine} from {Pickup.PController.MaxAmmo}";
            if (Pickup.PController.reloading) Ammo.text += " RELOADING...";
        } else
        {
            Ammo.enabled = false;
        }
    }
}
