using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMenuPagination : MonoBehaviour
{
    private static List<TextMeshProUGUI> pages;

    void Start()
    {
        pages = new List<TextMeshProUGUI>();

        // Get all of the existing pages.
        int i = 1;
        TextMeshProUGUI? page = GameObject.Find("Page 1")?.GetComponent<TextMeshProUGUI>();
        while (page) {
            page.gameObject.SetActive(false); // Hide all pages
            pages.Add(page);
            i += 1;
            page = GameObject.Find("Page " + i)?.GetComponent<TextMeshProUGUI>();
        }
    }

    public static void Init() {
        SetPageActivity(1, true);
        SetPageActivity(2, true);
    }

    private static void SetPageActivity(int pageNum, bool activity) {
        pages[pageNum-1].enabled = activity;
        pages[pageNum-1].gameObject.SetActive(activity);
    }
}
