using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBarUI : MonoBehaviour
{
    [Header("Hp bar setting")]
    [SerializeField]
    private Character character;
    private Image HpBarImage;
    private Image HpBarEffectImage;
    [SerializeField] private float hpBarEffectDuration = 0.5f;
    private Coroutine updateCoroutine;
    private Coroutine disableCoroutine;

    private void Update()
    {
        transform.localScale = new Vector3(transform.parent.localScale.x, 1, 1);
    }
    private void OnEnable()
    {
        //get components from children
        HpBarImage = HpBarImage ?? transform.Find("HpBar").Find("HpBarImage").GetComponent<Image>();
        HpBarEffectImage = HpBarEffectImage ?? transform.Find("HpBar").Find("HpBarEffectImage").GetComponent<Image>();
        character = character ?? transform.parent.GetComponent<Character>();
        HpBarImage.fillAmount = (float)character.hp / (float)character.maxHp;
        HpBarEffectImage.fillAmount = HpBarImage.fillAmount;
    }

    public void Start()
    {
        character.UIUpdateEvent.AddListener(() => gameObject.SetActive(true));
        character.UIUpdateEvent.AddListener(UpdateHpDisplay);
        gameObject.SetActive(false);
    }

    public void UpdateHpDisplay()
    {
        //TODO: Update hp bar, need character states script.
        HpBarImage.fillAmount = (float)character.hp / (float)character.maxHp;
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }
        updateCoroutine = StartCoroutine(UpdateHpEffect());
    }

    private IEnumerator UpdateHpEffect()
    {
        float effectLength = HpBarEffectImage.fillAmount - HpBarImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < hpBarEffectDuration && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            HpBarEffectImage.fillAmount = Mathf.Lerp(HpBarImage.fillAmount + effectLength, HpBarImage.fillAmount, elapsedTime / hpBarEffectDuration);
            yield return null;
        }
        HpBarEffectImage.fillAmount = HpBarImage.fillAmount;
        if (HpBarEffectImage.fillAmount <= 0)
        {
            gameObject.SetActive(false);
            yield break;
        }
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
        disableCoroutine = StartCoroutine(DisableSelf());
    }

    private IEnumerator DisableSelf()
    {
        yield return new WaitForSeconds(15f);
        gameObject.SetActive(false);
    }
}
