using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadIntro : MonoBehaviour
{  [SerializeField] public TextMeshProUGUI intro;

    // Start is called before the first frame update
    private bool willStart;
    void Start()
    {
        willStart = false;
        intro.text = "Bienvenue sur Quest For Cofee! \n  Aidez Nils à rendre son projet d'IA à l'heure \n Pour ce faire, récupérez un maximum de mugs! \n \n Attention à son frère, le grand buveur de café !\n (espace pour continuer)";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !willStart)
        {
            intro.text =  "Clic droit pour poser un mur  \n  Clic gauche pour rendre une tuile attrayante \n Espace pour lancer la simulation \n R pour relancer le jeu \n Vous pouvez controller les joueurs avec A* ou Dijckstra \n N'enfermez pas les participants!";
            willStart = true;
        }
        if (Input.GetKeyDown("space") && willStart)
        {
            SceneManager.LoadScene(1);
            

        }
    }
}
