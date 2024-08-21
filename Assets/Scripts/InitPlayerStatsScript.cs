using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitPlayerStatsScript : MonoBehaviour
{
    public UserData userData;
    public DataManager dataManager;
    public TextMeshProUGUI coinCount;
    public TextMeshProUGUI xpLevel;
    public Animator xpBarAnimator;
    
    void Start()
    {
        xpBarAnimator.Play("Idle");
        dataManager = DataManager.Instance;
        userData = dataManager.GetUserData();
        coinCount.text = userData.coins.ToString();
        xpLevel.text = userData.xpLevel.ToString();
    }
}
