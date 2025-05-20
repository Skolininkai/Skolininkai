using UnityEngine; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerAbilities : MonoBehaviour
{
    public enum Ability { LiftHeavyObjects, WalkThroughHotObjects }
    public Ability currentAbility = Ability.LiftHeavyObjects;

    [Header("UI")]
    public Image abilityIcon;
    public Sprite muscleSprite;
    public Sprite fireSprite;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip switchAbilitySound;

    [Tooltip("Start time (in seconds) of the sound segment to play")]
    public float startTime = 0f;

    [Tooltip("End time (in seconds) of the sound segment to play")]
    public float endTime = 3f;

    [Header("Lava Sound")]
    public AudioClip lavaBurnSound; // 🔥 Добавь звук ожога здесь

    [Header("Objects")]
    public GameObject levelFailedHUD;
    public GameObject player;

    private Coroutine soundCoroutine;

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
            }
            else
            {
                // 🔊 Воспроизведение звука ожога перед рестартом
                StartCoroutine(PlayLavaBurnAndRestart());
            }
        }
    }

    IEnumerator PlayLavaBurnAndRestart()
    {
        if (lavaBurnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(lavaBurnSound);
            Hints.instance.ShowHint("With this ability you can't walk through lava! Restarting...", 2);
            yield return new WaitForSeconds(lavaBurnSound.length); // Ждём пока звук проиграется
        }
        else
        {
            yield return new WaitForSeconds(1f); // Просто задержка, если звук не задан
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SwitchAbility()
    {
        currentAbility = (currentAbility == Ability.LiftHeavyObjects)
            ? Ability.WalkThroughHotObjects
            : Ability.LiftHeavyObjects;

        UpdateAbilityIcon();

        if (audioSource != null && switchAbilitySound != null)
        {
            if (soundCoroutine != null)
                StopCoroutine(soundCoroutine);

            soundCoroutine = StartCoroutine(PlaySoundSegment());
        }
    }

    void UpdateAbilityIcon()
    {
        abilityIcon.sprite = (currentAbility == Ability.LiftHeavyObjects)
            ? muscleSprite
            : fireSprite;
    }

    IEnumerator PlaySoundSegment()
    {
        audioSource.clip = switchAbilitySound;
        audioSource.time = Mathf.Clamp(startTime, 0, switchAbilitySound.length - 0.1f);
        audioSource.Play();

        float duration = Mathf.Clamp(endTime - startTime, 0.1f, switchAbilitySound.length);
        yield return new WaitForSeconds(duration);

        audioSource.Stop();
    }
}