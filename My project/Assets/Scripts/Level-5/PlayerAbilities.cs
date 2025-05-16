using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public enum Ability { LiftHeavyObjects, WalkThroughHotObjects }
    public Ability currentAbility = Ability.LiftHeavyObjects;

    public Image abilityIcon;
    public Sprite muscleSprite;
    public Sprite fireSprite;
    public GameObject levelFailedHUD;
    public GameObject player;

    private void Start()
    {
        Hints.instance.ShowHint("Press 'F' to switch the ability", 2);
        levelFailedHUD.SetActive(false);
        UpdateAbilityIcon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchAbility();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hot"))
        {
            if (currentAbility == Ability.WalkThroughHotObjects)
            {
                Debug.Log("Walked through hot object safely!");
                // galima tiesiog nieko nedaryt — leidþiam pereiti
            }
            else
            {
                Hints.instance.ShowHint("With this ability you cant walk through objects! Restarting", 2);
                Debug.Log("Ouch! It's hot!");
                levelFailedHUD.SetActive(true);
                // èia galima pridëti damage arba mirtá
            }
        }
    }

    void SwitchAbility()
    {
        if (currentAbility == Ability.LiftHeavyObjects)
        {
            currentAbility = Ability.WalkThroughHotObjects;
        }
        else
        {
            currentAbility = Ability.LiftHeavyObjects;
        }

        UpdateAbilityIcon();
    }

    void UpdateAbilityIcon()
    {
        if (currentAbility == Ability.LiftHeavyObjects)
        {
            abilityIcon.sprite = muscleSprite;
        }
        else if (currentAbility == Ability.WalkThroughHotObjects)
        {
            abilityIcon.sprite = fireSprite;
        }
    }
}
