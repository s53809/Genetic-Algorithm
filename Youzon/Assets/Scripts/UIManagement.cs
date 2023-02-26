using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    private static Text _gene;
    private static Text _result;
    private void Awake()
    {
        _gene = transform.GetChild(0).GetComponent<Text>();
        _result = transform.GetChild(1).GetComponent<Text>();
    }

    public static void ViewGene(int N)
    {
        _gene.text = "Generation : " + N.ToString();
    }

    public static void ViewResult(EntityController ec)
    {
        string txt = "Best Result : ";
        if (ec.Result == -1) txt += "Not Arrived\n";
        else txt += ec.Result.ToString() + "\n";

        for (int i = 0; i < ec.Actions.Length; i++)
        {
            txt += ec.Actions[i].ToString() + " ";
        }

        txt += "\n" + ec.DisPoint;

        _result.text = txt;
    }
}
