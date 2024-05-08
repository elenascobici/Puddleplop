using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitPlayerStatsScript : MonoBehaviour
{
    public UserData userData;
    public TextMeshProUGUI coinCount;
    
    void Start()
    {
        coinCount.text = userData.data["coins"];
    }
}
