using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public SoundController SoundController;
    public Animator General;
    public Transform StartPosition;
    private Vector3 TargetPosition;
    public string Action;

    void Start()
    {
        TargetPosition = new Vector3(StartPosition.transform.position.x, General.transform.position.y, StartPosition.transform.position.z);
        Action = "WALK";
        General.SetTrigger("Walk");
    }


    void Update()
    {
        if (Action == "WALK")
        {
            General.transform.position = General.transform.position + General.transform.forward * 2 * Time.deltaTime;
            General.transform.rotation = Utils.LookAt(General.transform, TargetPosition, 1f);

            if (Vector3.Distance(General.transform.position, TargetPosition) < 0.5f)
            {
                General.SetTrigger("Greet");
                Action = "";
            }
        }
    }


    public void StartGame()
    {
        SoundController.PlaySound("Click");
        SceneManager.LoadScene("Game");
    }
}
