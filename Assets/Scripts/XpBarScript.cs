using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class XpBarScript : MonoBehaviour
{
    public Image xPMask;
    // Start is called before the first frame update
    public void Init(float fillAmount)
    {
        xPMask.fillAmount = fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
