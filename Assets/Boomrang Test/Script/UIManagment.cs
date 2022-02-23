using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagment : MonoBehaviour
{
    [SerializeField]
    Button reloadBTn;
    // Start is called before the first frame update
    void Start()
    {
        //reload current scene 
        reloadBTn.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

}
