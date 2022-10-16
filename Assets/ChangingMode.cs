using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangingMode : MonoBehaviour
{
  [SerializeField] public TextMeshProUGUI ButtonText;
 
    // Start is called before the first frame update
    void Start()
    {
        ButtonText.text = "IA dirigée par A*";
    }

    public void ChangeAlgorithm()
    {
        GameManager.Instance.ControlledByAstar = !GameManager.Instance.ControlledByAstar;

        if (GameManager.Instance.ControlledByAstar)
        {
            ButtonText.text = "IA dirigée par A*";

        }
        else
        {
            ButtonText.text = "IA dirigée par Dijkstra";

        }
    }
}
