using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class VehicleCamera : MonoBehaviour
{



    public Transform target;

    public float smooth = 0.3f;
    public float distance = 5.0f;
    public float height = 1.0f;
    public float Angle = 20;
    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    public LayerMask lineOfSightMask = 0;


    public CarUIClass CarUI;


    [HideInInspector]
    public int Switch;

    private int gearst = 0;
    private float thisAngle = -150;
    private float restTime = 0.0f;
    private Rigidbody myRigidbody;



    private VehicleControl carScript;



    [System.Serializable]
    public class CarUIClass
    {

        public Image tachometerNeedle;
        public Image barShiftGUI;

        public Text speedText;
        public Text GearText;

    }


    public void RestCar()
    {

        if (restTime == 0)
        {
            myRigidbody.AddForce(Vector3.up * 500000);
            myRigidbody.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y, 0));
            restTime = 2.0f;
        }

    }



    public void ShowCarUI()
    {



        gearst = carScript.currentGear;
        CarUI.speedText.text = ((int)carScript.speed).ToString();




        if (carScript.carSetting.automaticGear)
        {

            if (gearst > 0 && carScript.speed > 1)
            {
                CarUI.GearText.color = Color.green;
                CarUI.GearText.text = gearst.ToString();
            }
            else if (carScript.speed > 1)
            {
                CarUI.GearText.color = Color.red;
                CarUI.GearText.text = "R";
            }
            else
            {
                CarUI.GearText.color = Color.white;
                CarUI.GearText.text = "N";
            }

        }
        else
        {

            if (carScript.NeutralGear)
            {
                CarUI.GearText.color = Color.white;
                CarUI.GearText.text = "N";
            }
            else
            {
                if (carScript.currentGear != 0)
                {
                    CarUI.GearText.color = Color.green;
                    CarUI.GearText.text = gearst.ToString();
                }
                else
                {

                    CarUI.GearText.color = Color.red;
                    CarUI.GearText.text = "R";
                }
            }

        }





        thisAngle = (carScript.motorRPM / 20) - 175;
        thisAngle = Mathf.Clamp(thisAngle, -180, 90);

        CarUI.tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
        CarUI.barShiftGUI.rectTransform.localScale = new Vector3(carScript.powerShift / 100.0f, 1, 1);

    }

    X

    void Start()
    {

        carScript = (VehicleControl)target.GetComponent<VehicleControl>();

        myRigidbody = target.GetComponent<Rigidbody>();


    }




    void Update()
    {

        if (!target) return;
        carScript = (VehicleControl)target.GetComponent<VehicleControl>();

        if (Input.GetKeyDown(KeyCode.G))
        {
            RestCar();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (restTime != 0.0f)
            restTime = Mathf.MoveTowards(restTime, 0.0f, Time.deltaTime);

        ShowCarUI();

        GetComponent<Camera>().fieldOfView = Mathf.Clamp(carScript.speed / 10.0f + 60.0f, 60, 90.0f);
        // Damp angle from current y-angle towards target y-angle

        float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
       target.eulerAngles.x + Angle, ref xVelocity, smooth);

        float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
        target.eulerAngles.y, ref yVelocity, smooth);

        // Look at the target
        transform.eulerAngles = new Vector3(xAngle, yAngle, 0.0f);

        var direction = transform.rotation * -Vector3.forward;
        var targetDistance = AdjustLineOfSight(target.position + new Vector3(0, height, 0), direction);


        transform.position = target.position + new Vector3(0, height, 0) + direction * targetDistance;
    }



    float AdjustLineOfSight(Vector3 target, Vector3 direction)
    {


        RaycastHit hit;

        if (Physics.Raycast(target, direction, out hit, distance, lineOfSightMask.value))
            return hit.distance;
        else
            return distance;

    }


}
