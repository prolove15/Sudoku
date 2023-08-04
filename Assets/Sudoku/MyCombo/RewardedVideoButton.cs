using UnityEngine;

public class RewardedVideoButton : MonoBehaviour
{
    public void OnClick()
    {
#if UNITY_EDITOR
        OnUserEarnedReward();
#else
        if (IsAdAvailable())
        {
            AdmobController.onUserEarnedReward = OnUserEarnedReward;
            AdmobController.instance.ShowRewardedAd();
        }
        else
        {
            Toast.instance.ShowMessage("Ad is not available now, please wait..");
        }
#endif
    }

    public void OnUserEarnedReward()
    {
        int amount = GameConfig.instance.rewardedVideoAmount;
        GameManager.Hints += amount;
        SudokuManager.intance.UpdateUI();
        Toast.instance.ShowMessage("You've received " + amount + " hints", 2);
    }

    private bool IsAdAvailable()
    {
        return AdmobController.instance.rewardedAd.IsLoaded();
    }
}
