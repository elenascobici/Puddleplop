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
    private DataManager dataManager;
    private List<EmployeeData> employees;
    
    void Start()
    {
        dataManager = DataManager.Instance;
        employees = dataManager.GetEmployeesData();
        exemplar.SetActive(false); 
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
            UpdateField(employeeBlurb, "CostText", employee.cost.ToString());

            // Update the animation loop for the character thumbnail.
            employeeBlurb.gameObject.SetActive(true);
            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>($"{employee.id}/{employee.anim}");
            Animator animator = employeeBlurb.transform.Find("CharacterImage").GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            print(employee.anim);
            animator.Play(employee.anim);
            
            return employeeBlurb;
        } catch (Exception _) {
            return null;
        };
    }
}
