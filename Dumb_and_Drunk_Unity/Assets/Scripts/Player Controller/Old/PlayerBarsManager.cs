using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarsManager : MonoBehaviour
{

    [SerializeField]
    Slider BalanceBarSlider, NauseaBarSlider;

    [SerializeField]
    GameObject[] PlayerImages;

    float BalanceBarValue = 0f, NauseaBarValue = 0f, BalanceBarSpeed = 7.5f, NauseaBarSpeed = 0.75f;
    float WalkingTime = 0f, MovingCooldown = 1.25f, StandingTime = 0f, MinBalaceBarSpeed = 5f, MaxBalanceBarSpeed = 15f, MinChangeTime = 3.5f, MaxChangeTime = 6f, DecayMultiplayer = 12.5f, NauseaBalanceMultiplier = 1f;

    [SerializeField]
    bool Player1;
    bool BalanceDirection = false, ThrowingUp = false, PlayerMoving = false, BlockedBalanceBar = false, BlockedNauseaBar = false, AlmostFellTrigger = false;

    Vector3 PreviousPosition;

    void Start()
    {
        PreviousPosition = transform.position;
        Invoke("ChangeDirection", Random.Range(MinChangeTime, MaxChangeTime));

        if (Player1 && GameSettings.SettingsInstance.Player1Difficulty == GameSettings.DifficultyLevel.Easy ||
          !Player1 && GameSettings.SettingsInstance.Player2Difficulty == GameSettings.DifficultyLevel.Easy)
        {
            MinBalaceBarSpeed = 5f;
            MaxBalanceBarSpeed = 10f;
        }

    }

    void Update()
    {
        CheckMovement();
        UpdateNausea();
        UpdateBalance();
    }

    void UpdateNausea()
    {
        if (!BlockedNauseaBar)
        {
            if (ThrowingUp)
            {
                NauseaBarValue -= Time.deltaTime * 5f * NauseaBarSpeed;

            }
            else if (PlayerMoving)
            {
                NauseaBarValue += Time.deltaTime * NauseaBarSpeed;

            }

            //SOUND SET RTPC TIME
            AkSoundEngine.SetRTPCValue("NauseaBar", NauseaBarValue, gameObject);

            NauseaBarSlider.value = NauseaBarValue;

            if (NauseaBarValue <= 25f)
            {
                NauseaBalanceMultiplier = 1f;
                ChangePlayerImage(0);
            }
            if (NauseaBarValue > 25f && NauseaBarValue <= 50f)
            {
                NauseaBalanceMultiplier = 1f;
                ChangePlayerImage(1);
            }
            if (NauseaBarValue > 50f && NauseaBarValue <= 75f)
            {
                NauseaBalanceMultiplier = 1.75f;
                ChangePlayerImage(2);
            }
            if (NauseaBarValue > 75f)
            {
                NauseaBalanceMultiplier = 2.25f;
                ChangePlayerImage(3);
            } 

            if (NauseaBarValue < 0f) GetComponent<PlayerSpecialStatusManager>().RecoverFromThrowUp();
        }

    }

    void UpdateBalance()
    {
        if (!BlockedBalanceBar)
        {

            //SOUND SET RTPC TIME
            AkSoundEngine.SetRTPCValue("Balance", BalanceBarValue, gameObject);

            if (BalanceBarSpeed > MinBalaceBarSpeed) BalanceBarSpeed -= Time.deltaTime * DecayMultiplayer;
            if (BalanceBarSpeed < MinBalaceBarSpeed) BalanceBarSpeed = MinBalaceBarSpeed;

            if (BalanceDirection)
            {
                BalanceBarValue += Time.deltaTime * BalanceBarSpeed * NauseaBalanceMultiplier;
            }
            else
            {
                BalanceBarValue -= Time.deltaTime * BalanceBarSpeed * NauseaBalanceMultiplier;
            }

            if (BalanceBarValue > -60f && BalanceBarValue < 60f)
            {
                if (AlmostFellTrigger)
                {
                    AkSoundEngine.PostEvent("RecoverFalling", gameObject);
                }
                AlmostFellTrigger = false;
            }

            if (!AlmostFellTrigger && (BalanceBarValue < -60f || BalanceBarValue > 60f))
            {
                AlmostFellTrigger = true;
                AkSoundEngine.PostEvent( "AlmostFall", gameObject);
            }

            if (BalanceBarValue >= 100f || BalanceBarValue <= -100f)
            {
                GetComponent<PlayerSpecialStatusManager>().Fall();
            }

            BalanceBarSlider.value = BalanceBarValue;

        }

    }

    void CheckMovement()
    {
        if (Vector3.Distance(PreviousPosition, transform.position) > 0.01f)
        {
            WalkingTime += Time.deltaTime;
            StandingTime = 0f;
        }
        else
        {
            StandingTime += Time.deltaTime;
            if(StandingTime >= MovingCooldown) WalkingTime = 0f;
        } 

        if (WalkingTime > MovingCooldown) PlayerMoving = true;
        else PlayerMoving = false;

        PreviousPosition = transform.position;

        /*
        if (Vector3.Distance(PreviousPosition, transform.position) > 0.01f)
        {
            PlayerMoving = true;
            StandingTime = 0f;
        }
        else
        {
            PlayerMoving = false;
            StandingTime += Time.deltaTime;
        }

        if (StandingTime > StandingCooldown) NauseaMoving = false;
        else NauseaMoving = true;
        */

    }

    void ChangePlayerImage(int i)
    {
        if (i < PlayerImages.Length)
        {
            for (int count = 0; count < PlayerImages.Length; count++)
            {
                if (count == i) PlayerImages[count].SetActive(true);
                else PlayerImages[count].SetActive(false);
            }
        }
    }

    // Radomly change the BalanceBar direction
    void ChangeDirection()
    {
        CancelInvoke();
        BalanceDirection = !BalanceDirection;
        Invoke("ChangeDirection", Random.Range(MinChangeTime, MaxChangeTime));

    }

    public void ThrowUp()
    {
        GetComponent<PlayerSpecialStatusManager>().StartThrowingUp();
    }

    public bool SetThrowingUp(bool ToSet)
    {
        if (( ToSet && NauseaBarValue >= 50f) || (!ToSet && NauseaBarValue <= 0f))
        {
            ThrowingUp = ToSet;
            return true;
        }
        else return false;
    }

    public void AddBalance(bool direction)
    {
        BalanceBarSpeed = MaxBalanceBarSpeed;
        if ((BalanceDirection && !direction) || (!BalanceDirection && direction)) ChangeDirection();

    }

    public void BlockBar(bool BalanceBar, bool ToSet)
    {
        if (BalanceBar)
        {
            BlockedBalanceBar = ToSet;
        }
        else
        {
            if(ToSet) NauseaBarSlider.gameObject.SetActive(true);                       // DA CONTROLLARE NEL TUTORIAL < ------------------------------------------------------------------------------
            BlockedNauseaBar = ToSet;
        }
    }

    public void BalanceBarSpeedModifier(bool ToActivate)
    {
        if (ToActivate)
        {
            MinBalaceBarSpeed *= 1.5f;
            MaxBalanceBarSpeed *= 1.5f;
        }
        else
        {
            MinBalaceBarSpeed /= 1.5f;
            MaxBalanceBarSpeed /= 1.5f;
        }
    }

    public void EmptyNauseaBar()
    {
        NauseaBarValue = 0f;
    }

    public void EmptyBalanceBar()
    {
        BalanceBarValue = 0f;

    }

    public float GetBalanceValue()
    {
        return BalanceBarValue;
    }

    public float GetNauseaValue()
    {
        return NauseaBarValue;
    }

}