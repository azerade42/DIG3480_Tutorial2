using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{

    public GameObject target;

    [SerializeField] float minY, maxY;


    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = new Vector3(target.transform.position.x, Mathf.Clamp(target.transform.position.y, minY, maxY), this.transform.position.z);
    }
}