using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLogin : MonoBehaviour {

    public Text txtServerState;
    public Button btSceneChanger;

    public void Awake() {
        
    }

    private IEnumerator _coSetTextState;
    private IEnumerator CoSetTextState() {
        yield return null;
    }

    public void OnChangeScene() {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}