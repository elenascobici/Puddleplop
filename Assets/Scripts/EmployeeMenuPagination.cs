using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMenuPagination : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public GameObject leftPage;
    public GameObject rightPage;
    public HandbookEmployeesBlurbs handbookEmployeesBlurbs;
    // public static Button leftButtonStatic;
    // public static Button rightButtonStatic;
    private static List<TextMeshProUGUI> pages;
    private static int activeLeft; // The page number of the active left page.

    void Start()
    {
        activeLeft = 1;

        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);

        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);

        leftPage.gameObject.SetActive(true);
        rightPage.gameObject.SetActive(true);
    }

    public void Init() {
        handbookEmployeesBlurbs.generateEmployeeBlurb(0).transform.SetParent(leftPage.transform, false);
        RectTransform secondItem = handbookEmployeesBlurbs.generateEmployeeBlurb(1).transform as RectTransform;
        secondItem.SetParent(leftPage.transform, false);
        Vector2 newPos = secondItem.anchoredPosition;
        newPos.y += 400;
        secondItem.anchoredPosition = newPos;

        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    public void Close() {
        // SetPageActivity(activeLeft, false);
        if (activeLeft < pages.Count) {
            // SetPageActivity(activeLeft+1, false);
        }
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
    }

    // private static void SetPageActivity(int pageNum, bool activity) {
    //     pages[pageNum-1].enabled = activity;
    //     pages[pageNum-1].gameObject.SetActive(activity);
    // }

    // Flip to the left.
    private void OnLeftButtonClick() {
        if (activeLeft == 1) {
            return;
        }
        // SetPageActivity(activeLeft, false);
        if (activeLeft < pages.Count) {
            // SetPageActivity(activeLeft+1, false);
        }
        // SetPageActivity(activeLeft-2, true);
        // SetPageActivity(activeLeft-1, true);
        activeLeft-=2;
    }

    // Flip to the right.
    private void OnRightButtonClick() {
        if (activeLeft >= pages.Count - 1) {
            return;
        }
        // SetPageActivity(activeLeft, false);
        // SetPageActivity(activeLeft+1,false);
        // SetPageActivity(activeLeft+2, true);
        if (activeLeft+3 <= pages.Count) {
            // SetPageActivity(activeLeft+3, true);
        }
        activeLeft+=2;
    }
}
