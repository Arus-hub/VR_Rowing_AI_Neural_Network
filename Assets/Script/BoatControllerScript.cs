using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(InputData))]
public class BoatControllerScript : MonoBehaviour
{
    // For Boat speed and everything
    public Vector3 COM;     
    [Space(15)]
    public float speed = 1.0f;  
    public float movementThresold = 10.0f;
    public float smoothTime = 0.1f;

   //For boat movement
    Transform m_COM;
    float verticalInput;
    float movementFactor;
    float horizontalInput;
    float steerFactor;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private InputData _inputData;
   // Animator anime;  // Reference to the Animator component
    public bool isMoving = false;
    public PredictDataScript predictDataScript;


    //for start and finish line

    public GameObject startLine;  
    public GameObject finishLine;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = new Vector3(50, 3, 478);  
        transform.position = startPosition;

        _inputData = GetComponent<InputData>();
       // anime = GetComponent<Animator>();
    }

    // Function for getting to the start position when the boat reaches end
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finishLine)
        {
            // When finish line is crossed, reset position to start
            ResetToStartPosition();
        }
    }

    private void ResetToStartPosition()
    {
        transform.position = startPosition;  // Reset position to the start
    }


    // Update is called once per frame
    void Update()
    {
        Balance();
       // Steer();
        Movement();
    }


    /* void Movement()
     {


         if (_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool lefttrigger) &&
             _inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool righttrigger))
         {
             if (lefttrigger && righttrigger)
             {
                 //UnityEngine.Debug.Log("Left Value" + lefttrigger);
                 verticalInput = 10f;
                 movementFactor = Mathf.Lerp(movementFactor, verticalInput, Time.deltaTime / movementThresold);
                 transform.Translate(Vector3.back * speed * Time.deltaTime);

                 // Calculate the target position for moving backward
                 targetPosition = transform.position - transform.forward * speed * Time.deltaTime;

                 // Smoothly move the object to the target position
                 transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
             }
         }  

     }
    */

    void Movement()
    {
        float deceleration = 0.9f;  // You can adjust this value for faster or slower deceleration
        int flag=0;
        int count = 0;
        /* if (_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool lefttrigger))
         {
             if (_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool righttrigger))
             {
                 if (lefttrigger == true && righttrigger == true)
                 {
                     UnityEngine.Debug.Log("Left Value" + lefttrigger);
                     verticalInput = 10f;
                     flag = 1;
                     isMoving = true;

                 }
                 else
                 {
                     // Slowly decrease the verticalInput 
                     verticalInput *= deceleration;
                     isMoving = false;
                     count += 1;
                 }

                 anime.SetBool("isMoving", isMoving);
             }
         }
         else
         {
             // Slowly decrease the verticalInput 
             verticalInput *= deceleration;
         }*/

        float[] myOutputs = predictDataScript.send_output;

        if (myOutputs[0] != 0)
        {
            //UnityEngine.Debug.Log("Left Value" + lefttrigger);
            verticalInput = 50f;
            flag = 1;
            isMoving = true;
            
            UnityEngine.Debug.Log("Output data From BoatControl: 1 ");
        }
        else
        {
            verticalInput *= deceleration;
            isMoving = false;
            count += 1;
           // float[] myOutputs = predictDataScript.send_output;
            UnityEngine.Debug.Log("Output data From BoatControl: " + myOutputs[0]);
        }
        //anime.SetBool("isMoving", isMoving);

        if (count < 5)
        {
            if (flag == 1)
            {
                movementFactor = Mathf.Lerp(movementFactor, verticalInput, Time.deltaTime / movementThresold);
                transform.Translate(Vector3.back * speed * Time.deltaTime);

                // Calculate the target position for moving 
                targetPosition = transform.position - transform.forward * speed * Time.deltaTime;

                // Smoothly move the object to the target position
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

            }
        }
        
        else
        {
            deceleration = 5.2f;
            flag = 0;
        }

        
    }


  
    void Balance()
    {
        if (!m_COM)
        {
            m_COM = new GameObject("COM").transform;
            m_COM.SetParent(transform);
        }

        m_COM.position = COM;
        GetComponent<Rigidbody>().centerOfMass = m_COM.position;
    }

}