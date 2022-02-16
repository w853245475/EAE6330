using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField]
    private InputField input1;
    [SerializeField]
    private InputField input2;
    [SerializeField]
    private InputField input3;

    // Start is called before the first frame update
    void Start()
    {
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitWave1);
        input1.onEndEdit = se;

        var se2 = new InputField.SubmitEvent();
        se2.AddListener(SubmitWave2);
        input2.onEndEdit = se2;

        var se3 = new InputField.SubmitEvent();
        se3.AddListener(SubmitWave3);
        input3.onEndEdit = se3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SubmitWave1(string arg)
    {
        //print(arg);
        LevelGeneration.wave1.seed =int.Parse(arg);
    }

    void SubmitWave2(string arg)
    {
        //print(arg);
        LevelGeneration.wave2.seed = int.Parse(arg);
    }

    void SubmitWave3(string arg)
    {
        //print(arg);
        LevelGeneration.wave3.seed = int.Parse(arg);
    }

    public void ReLoadMap()
    {
        // Reload Map
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
