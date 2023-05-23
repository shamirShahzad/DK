using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager character;
        public LayerMask characterLayer;
        public float criticalAttackRange = 0.7f;

        public int pendingCriticalDamage;
        WaitForSeconds backStabTimer = new WaitForSeconds(0.05f);

        [Header("Combat Transform")]
        public Transform backStabReceiverTransform;

        public string lastAttack;

        [Header("Attack Animations")]
        public string OH_Light_Attack_01 = "OH_Light_Attack_01";
        public string OH_Light_Attack_02 = "OH_Light_Attack_02";
        public string OH_Heavy_Attack_01 = "OH_Heavy_Attack_01";
        public string OH_Heavy_Attack_02 = "OH_Heavy_Attack_02";
        public string OH_Running_Attack_01 = "OH_Running_Attack_01";
        public string OH_Jumping_Attack_01 = "OH_Jumping_Attack_01";
        public string OH_Charge_Attack_01 = "OH_Charging_Attack_Charge";
        public string TH_Heavy_Attack_01 = "TH_Heavy_Attack_01";
        public string TH_Heavy_Attack_02 = "TH_Heavy_Attack_02";
        public string TH_Light_Attack_01 = "TH_Light_Attack_01";
        public string TH_Light_Attack_02 = "TH_Light_Attack_02";
        public string TH_Jumping_Attack_01 = "TH_Jumping_Attack_01";
        public string TH_Running_Attack_01 = "TH_Running_Attack_01";
        public string TH_Charge_Attack_01 = "TH_Charging_Attack_Charge";
        public string weaponArtShield = "Parry";

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        [Header("Attack Type")]
        public AttackType currentAttackType;


        public virtual void DrainStaminaBasedOnAttackTypes()
        {
                 //If you want Ai to have stamina as well put code in here, however fck that AI IS superior and full of Stamina 
        }

        public virtual void SetBlockingAbsorbtionsFromBlockingWeapon()
        {
            if (character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorbtion = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingMagicDamageAbsorbtion = character.characterInventoryManager.rightWeapon.magicBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingFireDamageAbsorbtion = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingLightningDamageAbsorbtion = character.characterInventoryManager.rightWeapon.lightningBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingDarkDamageAbsorbtion = character.characterInventoryManager.rightWeapon.darkBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingStability = character.characterInventoryManager.rightWeapon.stability;
            }
            else if (character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorbtion = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingMagicDamageAbsorbtion = character.characterInventoryManager.leftWeapon.magicBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingFireDamageAbsorbtion = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingLightningDamageAbsorbtion = character.characterInventoryManager.leftWeapon.lightningBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingDarkDamageAbsorbtion = character.characterInventoryManager.leftWeapon.darkBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingStability = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void AttempBlock(DamageCollider attackingWeapon,float physicalDamage,float magicDamage,
            float fireDamage,float lightningDamage,float darkDamage,string blockAnimation)
        {
            float staminaDamageAbsorbtion = ((physicalDamage + magicDamage + fireDamage + lightningDamage + darkDamage) * attackingWeapon.guardBreakModifier) * 
                (character.characterStatsManager.blockingStability / 100);
            float staminaDamage = ((physicalDamage + magicDamage + fireDamage + lightningDamage + darkDamage) * attackingWeapon.guardBreakModifier) - staminaDamageAbsorbtion;
            character.characterStatsManager.currentStamina -=  staminaDamage;
            if(character.characterStatsManager.currentStamina <= 0)
            {
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Guard Break", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }

        }

        public void AttemptBackStabOrRiposte()
        {
            if (character.isInteracting)
                return;
            if (character.characterStatsManager.currentStamina <= 0)
                return;
            RaycastHit hit;
            if (Physics.Raycast(character.criticalAttackRaycastStartPoint.transform.position, character.transform.TransformDirection(Vector3.forward), out hit, criticalAttackRange, characterLayer))
            {
                CharacterManager enemyCharacter;
                if(hit.transform.tag == "Player")
                {
                    enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                }
                else
                {
                    enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                }
                Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
                float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);
                if (enemyCharacter.canBeRiposted)
                {
                    if(dotValue<=1.2f && dotValue >= 0.6f)
                    {
                        AttemptRiposte(enemyCharacter);
                        return;
                    }
                }
                if(dotValue >= -0.9f && dotValue <= -0.8f)
                {
                    AttemptBackstab(enemyCharacter);
                }
            }
        }

        IEnumerator ForceMoveCharcterToEnemyBackStabPosition(CharacterManager charcterPerforingBackStabbed)
        {
            for (float timer = 0.05f; timer < 0.05f; timer+=0.05f)
            {
                Quaternion backStabRotaion = Quaternion.LookRotation(charcterPerforingBackStabbed.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backStabRotaion, 1);
                transform.parent = charcterPerforingBackStabbed.characterCombatManager.backStabReceiverTransform;
                transform.localPosition = charcterPerforingBackStabbed.characterCombatManager.backStabReceiverTransform.localPosition;
                transform.parent = null;
                yield return backStabTimer;
            }
        }

        IEnumerator ForceMoveCharcterToEnemyRipostePosition(CharacterManager charcterPerformingRiposted)
        {
            for (float timer = 0.05f; timer < 0.05f; timer += 0.05f)
            {
                Quaternion riposteRotation = Quaternion.LookRotation(-charcterPerformingRiposted.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, riposteRotation, 1);
                transform.parent = charcterPerformingRiposted.characterCombatManager.backStabReceiverTransform;
                transform.localPosition = charcterPerformingRiposted.characterCombatManager.backStabReceiverTransform.localPosition;
                transform.parent = null;
                yield return backStabTimer;
            }
        }
        private void AttemptBackstab(CharacterManager enemyCharacter)
        {
            if (enemyCharacter != null)
            {
                if(!enemyCharacter.isBeingBackStabbed || !enemyCharacter.isBeingRiposted)
                {
                    EnableIsInvulnerable();
                    character.isPerformingBackStab = true;
                    character.characterAnimatorManager.EraseHandIKfromWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    

                    int criticalDamage = Mathf.RoundToInt((character.characterInventoryManager.rightWeapon.criticalDamageModifier * (character.characterInventoryManager.rightWeapon.physicalDamage)));
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = criticalDamage;
                    enemyCharacter.characterCombatManager.GetBackStabbed(character);
                }
            }
            
        }

        private void AttemptRiposte(CharacterManager enemyCharacter)
        {
            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackStabbed || !enemyCharacter.isBeingRiposted)
                {
                    EnableIsInvulnerable();
                    character.isPerformingRiposte = true;
                    character.characterAnimatorManager.EraseHandIKfromWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);


                    int criticalDamage = Mathf.RoundToInt((character.characterInventoryManager.rightWeapon.criticalDamageModifier * (character.characterInventoryManager.rightWeapon.physicalDamage)));
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = criticalDamage;
                    enemyCharacter.characterCombatManager.GetRiposted(character);
                }
            }
        }

        private void GetRiposted(CharacterManager characterBeingRiposted)
        {
            characterBeingRiposted.characterSFXManager.audioSource.PlayOneShot(characterBeingRiposted.characterSFXManager.backStabOrRiposte);
            character.isBeingRiposted = true;
            StartCoroutine(ForceMoveCharcterToEnemyRipostePosition(characterBeingRiposted));
            character.characterAnimatorManager.PlayTargetAnimationWithRootrotation("Riposted", true);
        }

        private void GetBackStabbed(CharacterManager characterGettingBackstabed)
        {
            character.characterSFXManager.audioSource.PlayOneShot(character.characterSFXManager.backStabOrRiposte);
            character.isBeingBackStabbed = true;
            StartCoroutine(ForceMoveCharcterToEnemyBackStabPosition(characterGettingBackstabed));
            character.characterAnimatorManager.PlayTargetAnimationWithRootrotation("Back Stabbed", true);
        }

        public void ApplyPendingDamage()
        {
            character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage, 0, 0, 0, 0);
        }

        private void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }

        public void EnableCanBeParried()
        {
            character.canBeParried = true;
        }
        public void DisableCanBeParried()
        {
            character.canBeParried = false;
        }

        private void SuccessfullycastedSpell()
        {
            character.characterInventoryManager.currentSpell.SuccessfullyCastedSpell(character);
            character.animator.SetBool("isFiringSpell", true);
        }

        
    }
}
