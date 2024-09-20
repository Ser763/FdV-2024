using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float thrustForce = 10f;
    public float rotationSpeed = 120f;

    public GameObject gun, bulletPrefab;
    private Rigidbody _rigid;

    public static float xBorderLimit, yBorderLimit;

    public static int SCORE = 0;
    private int isPaused = 0;
    // Start is called before the first frame update
    void Start()
    {
        xBorderLimit = Camera.main.orthographicSize+1;
        yBorderLimit = (Camera.main.orthographicSize+1)* Screen.width / Screen.height;
        _rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Input.GetAxis("Rotate") * Time.deltaTime;
        float thrust  = Input.GetAxis("Thrust") * Time.deltaTime;

        Vector3 thrustDirection = transform.right;
        _rigid.AddForce(thrustDirection * thrust * thrustForce);

        transform.Rotate(Vector3.forward, -rotation * rotationSpeed);

        var newPos = transform.position;
        if(newPos.x > xBorderLimit)
        newPos.x = -xBorderLimit+1;
        else if(newPos.x < -xBorderLimit)
        newPos.x = xBorderLimit-1;
        else if(newPos.y > yBorderLimit)
        newPos.y = -yBorderLimit+1;
        else if(newPos.y < -yBorderLimit)
        newPos.y = yBorderLimit-1;
        transform.position = newPos;

        if(Input.GetKeyDown(KeyCode.Space) && isPaused == 0){
            GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);

            Bullet balaScript = bullet.GetComponent<Bullet>();
            balaScript.targetVector = transform.right;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused == 0){
                PauseGame();
                isPaused = 1;
            }
            else{
                ResumeGame();
                isPaused = 0;
            } 
        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Enemy")){
            SCORE = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else{
            Debug.Log("He colisionado con otra cosa");
        }
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("Atravesamiento");
    }

    private void PauseGame(){
        Time.timeScale = 0;
    }

    private void ResumeGame(){
        Time.timeScale = 1;
    }
}
