using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicShooting : MonoBehaviour
{
    [SerializeField] private Sprite[] handgunSprites;
    [SerializeField] private Sprite[] handChargeSprites;
    [SerializeField] private Image handImage;
    [SerializeField] private Image runeImage;

    [Header("Handgun")]
    [SerializeField] private bool canShoot = true;
    
    [Header("Spells")]
    private int activeSpellIndex;
    [SerializeField] private Spell[] spells;

    [Header("Charge")] [SerializeField] private GameObject chargeFXContainer;
    private float chargingTime;
    private bool canCharge;
    
    private ParticleSystem chargeParticles;
    private Camera mainCam;

    private void Awake() {
        mainCam = Camera.main;
        canCharge = true;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
        {
            activeSpellIndex++;
            if (activeSpellIndex >= spells.Length)
                activeSpellIndex = 0;
        }
        else if (scroll < 0)
        {
            activeSpellIndex--;
            if (activeSpellIndex < 0)
                activeSpellIndex = spells.Length - 1;
        }

        if (scroll != 0) {
            chargeParticles = null;
            runeImage.sprite = null;
            SetHandVisuals();
        }
        
        if (spells[activeSpellIndex].spellType == SpellType.Projectile)
        {
            CheckProjectile();
        } else if (spells[activeSpellIndex].spellType == SpellType.Charge)
        {
            CheckCharge();
        }
        

    }

    private void CheckProjectile()
    {
        if (!canShoot)
            return;

        if (Input.GetMouseButtonDown(0))
            Cast();
    }

    private void CheckCharge() {
        if (!runeImage.sprite){
            runeImage.sprite = spells[activeSpellIndex].spellIcon;
        }
        
        if (!chargeParticles) {
            chargeParticles = Instantiate(spells[activeSpellIndex].spellChargeParticles, chargeFXContainer.transform.position, Quaternion.identity, chargeFXContainer.transform);
        }

        if (!canCharge) {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            handImage.sprite = handChargeSprites[1];
            chargeParticles.Play();
        }
        
        if (Input.GetMouseButton(0))
        {
            chargingTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (chargingTime >= spells[activeSpellIndex].spellChargeTime) {
                Cast();
                chargeParticles.Stop();
                runeImage.gameObject.SetActive(false);
            }
            else
            {
                handImage.sprite = handChargeSprites[0];
                chargeParticles.Stop();
            }
        }

        if (chargingTime >= spells[activeSpellIndex].spellChargeTime)
        {
            runeImage.gameObject.SetActive(true);
        }
    }

    private void SetHandVisuals()
    {
        switch (spells[activeSpellIndex].spellType)
        {
            case SpellType.Projectile:
                handImage.sprite = handgunSprites[0];
                runeImage.gameObject.SetActive(false);
                break;
            case SpellType.Charge:
                if (!canCharge) {
                    break;
                }
                handImage.sprite = handChargeSprites[0];
                break;
            default:
                break;
        }
    }

    private void Cast() {
        switch (spells[activeSpellIndex].spellType) {
            case SpellType.Projectile:
                StartCoroutine(ShootProjectile());
                break;
            case SpellType.Charge:
                StartCoroutine(ShootCharge());
                break;
            default:
                break;  
        }

    }
    
    //Shooting
    private IEnumerator ShootCharge()
    {
        canCharge = false;
        handImage.sprite = handChargeSprites[2];
        chargingTime = 0;
        Instantiate(spells[activeSpellIndex].spellPrefab, mainCam.transform.position + (mainCam.transform.forward * 3f), mainCam.transform.rotation); //Use main camera so shot is straight
        yield return new WaitForSeconds(.3f);
        handImage.sprite = handChargeSprites[0];
        canCharge = true;
    }

    private IEnumerator ShootProjectile()
    {
        handImage.sprite = handgunSprites[1];
        canShoot = false;
        yield return new WaitForSeconds(0.02f);
        handImage.sprite = handgunSprites[2];
        Instantiate(spells[activeSpellIndex].spellPrefab, mainCam.transform.position + (mainCam.transform.forward * 3f), mainCam.transform.rotation);
        yield return new WaitForSeconds(spells[activeSpellIndex].spellCooldown - 0.02f);
        handImage.sprite = handgunSprites[0];
        canShoot = true;
    }
}
