using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class XpBarScript : MonoBehaviour
{
    public Image xPMask;
    private DataManager dataManager;
    private UserData userData;
    void Start() {
        dataManager = DataManager.Instance;
        userData = dataManager.GetUserData();
        SetFillAmount(userData.xpPoints);
    }

    public void SetFillAmount(float xpPoints)
    {
        xPMask.fillAmount = xpPoints / 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
