using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class HandbookEmployeesBlurbs : MonoBehaviour
{
    public GameObject exemplar;
    public GameObject employPopup;
    private DataManager dataManager;
    private List<EmployeeData> employees;
    
    void Start()
    {
        dataManager = DataManager.Instance;
        employees = dataManager.GetEmployeesData();
        exemplar.SetActive(false);
        employPopup.transform.localScale = new Vector2(0, 0);
    }

    private void OnEmployClicked(EmployeeData employee, GameObject employeeBlurb) {
        employPopup.transform.localScale = new Vector2(1, 1);
        employPopup.transform.Find("YesButton").GetComponent<Button>()
            .onClick.AddListener(() => {print("close"); employPopup.transform.localScale = new Vector2(0, 0);});
        employPopup.transform.Find("CancelButton").GetComponent<Button>()
            .onClick.AddListener(() => {employPopup.transform.localScale = new Vector2(0, 0);});
        employPopup.transform.Find("EmployText").GetComponent<TextMeshProUGUI>()
            .text = $"Would you like to employ {employee.name} for {employee.cost} coins?";
    }

    private void UpdateField(GameObject employeeBlurb, string fieldName, string newFieldText) {
        employeeBlurb.transform.Find(fieldName).GetComponent<TextMeshProUGUI>().text = newFieldText;
    }

    public bool indexInRange(int index) {return index < employees.Count && index >= 0;}

    /*
        Create and return a GameObject containing a blurb for the
        employee with the given index in the employees list. This
        blurb is displayed in the employee handbook.
    */
    public GameObject generateEmployeeBlurb(int index) {
        try {
            EmployeeData employee = employees[index];
            GameObject employeeBlurb = Instantiate(exemplar);
            employeeBlurb.name = name;
            employeeBlurb.tag = "Untagged";

            // Update the text fields in the blurb.
            UpdateField(employeeBlurb, "NameText", employee.name);
            UpdateField(employeeBlurb, "DescText", employee.desc);
            UpdateField(employeeBlurb, "CostText", employee.cost == 0 ? "FREE" : employee.cost.ToString());

            // Update the animation loop for the character thumbnail.
            employeeBlurb.gameObject.SetActive(true);
            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>($"{employee.id}/{employee.anim}");
            Animator animator = employeeBlurb.transform.Find("CharacterImage").GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            print(employee.anim);
            animator.Play(employee.anim);

            // Add a listener to the blurb's employ button.
            employeeBlurb.transform.Find("EmployButton").GetComponent<Button>().onClick.AddListener(() => {OnEmployClicked(employee, employeeBlurb);});
            
            return employeeBlurb;
        } catch (Exception _) {
            return null;
        };
    }
}
