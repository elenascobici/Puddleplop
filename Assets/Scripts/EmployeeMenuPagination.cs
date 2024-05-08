using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMenuPagination : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    // public Button closeButton;
    public static Button leftButtonStatic;
    public static Button rightButtonStatic;
    private static List<TextMeshProUGUI> pages;
    private static int activeLeft; // The page number of the active left page.

    void Start()
    {
        pages = new List<TextMeshProUGUI>();
        activeLeft = 1;

        leftButtonStatic = leftButton;
        rightButtonStatic = rightButton;

        // Get all of the existing pages.
        int i = 1;
        TextMeshProUGUI? page = GameObject.Find("Page 1")?.GetComponent<TextMeshProUGUI>();
        while (page) {
            page.gameObject.SetActive(false); // Hide all pages
            pages.Add(page);
            i += 1;
            page = GameObject.Find("Page " + i)?.GetComponent<TextMeshProUGUI>();
        }

        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);

        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
    }

    public static void Init() {
        SetPageActivity(1, true);
        SetPageActivity(2, true);
        leftButtonStatic.gameObject.SetActive(true);
        rightButtonStatic.gameObject.SetActive(true);
    }

    public static void Close() {
        SetPageActivity(activeLeft, false);
        if (activeLeft < pages.Count) {
            SetPageActivity(activeLeft+1, false);
        }
        leftButtonStatic.gameObject.SetActive(false);
        rightButtonStatic.gameObject.SetActive(false);
    }

    private static void SetPageActivity(int pageNum, bool activity) {
        pages[pageNum-1].enabled = activity;
        pages[pageNum-1].gameObject.SetActive(activity);
    }

    // Flip to the left.
    private void OnLeftButtonClick() {
        if (activeLeft == 1) {
            return;
        }
        SetPageActivity(activeLeft, false);
        if (activeLeft < pages.Count) {
            SetPageActivity(activeLeft+1, false);
        }
        SetPageActivity(activeLeft-2, true);
        SetPageActivity(activeLeft-1, true);
        activeLeft-=2;
    }

    // Flip to the right.
    private void OnRightButtonClick() {
        if (activeLeft >= pages.Count - 1) {
            return;
        }
        SetPageActivity(activeLeft, false);
        SetPageActivity(activeLeft+1,false);
        SetPageActivity(activeLeft+2, true);
        if (activeLeft+3 <= pages.Count) {
            SetPageActivity(activeLeft+3, true);
        }
        activeLeft+=2;
    }
}
