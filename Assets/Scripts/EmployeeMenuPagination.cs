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
    private int activeIndex; // The index of the employee currently in the top left.

    void Start()
    {
        activeIndex = 0;

        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);

        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);

        leftPage.gameObject.SetActive(true);
        rightPage.gameObject.SetActive(true);
    }

    public void GenerateSingleAndTransform(int i, int x, int y, GameObject page) {
        GameObject item = handbookEmployeesBlurbs.generateEmployeeBlurb(i);
        if (item == null) {
            throw new System.Exception("Item doesn't exist.");
        }
        RectTransform transform = item.transform as RectTransform;
        transform.SetParent(page.transform, false);
        Vector2 newPos = transform.anchoredPosition;
        newPos.x += x;
        newPos.y += y;
        transform.anchoredPosition = newPos;
    }

    private void ClearAllPages() {
        foreach (Transform child in leftPage.transform) {
            if (child.tag != "DontDelete") { // Don't delete the tagged exemplar.
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in rightPage.transform) {Destroy(child.gameObject);}
    }

    private void GenerateAllFromIndex() {
        ClearAllPages();
        
        try {
            GenerateSingleAndTransform(activeIndex, 0, 0, leftPage); // Top left
            GenerateSingleAndTransform(activeIndex+1, 0, 400, leftPage); // Bottom left
            GenerateSingleAndTransform(activeIndex+2, 300, 0, rightPage); // Top right
            GenerateSingleAndTransform(activeIndex+3, 300, 400, rightPage); // Bottom right
            return;
        } catch {return;}
    }

    public void Init() {
        activeIndex = 0;
        GenerateAllFromIndex();

        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    public void Close() {
        ClearAllPages();
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
    }

    // Flip to the left.
    private void OnLeftButtonClick() {
        if (handbookEmployeesBlurbs.indexInRange(activeIndex-4)) {
            activeIndex-=4;
            GenerateAllFromIndex();
        }
    }

    // Flip to the right.
    private void OnRightButtonClick() {
        if (handbookEmployeesBlurbs.indexInRange(activeIndex+4)) {
            activeIndex+=4;
            GenerateAllFromIndex();
        }
    }
}
