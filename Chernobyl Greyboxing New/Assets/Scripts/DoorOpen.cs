using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Orientation orientation; //creates a dropdown in the inspector using the below enum list, so we can choose in unity which direction we want the door to open
    private Quaternion closedRotation; 
    private Quaternion openRotation;
    private Quaternion targetRotation;

    public float rotationSpeed;

    private bool isOpen = false;
    private bool isMoving = false;
    
    public enum Orientation
    {
        Inward,
        Outward
    }
    // Start is called before the first frame update
    void Start()
    {
        closedRotation = transform.localRotation;

        Vector3 euler = closedRotation.eulerAngles; //creating a temporary variable of the door's current rotation and setting that as the default, or closed, rotation

        if (orientation == Orientation.Inward)
        {
            openRotation = Quaternion.Euler(euler.x, euler.y - 90f, euler.z); //using that closed rotation value to create new rotation data, this time adding or subtracting 90 degrees for when the door is opened
        }           
        else
        {
            openRotation = Quaternion.Euler(euler.x, euler.y + 90f, euler.z);
        }
          
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (isOpen) //uses the isOpen bool to check if the door should be moved to the open position or the closed position
            {
                targetRotation = openRotation;
            }
            else
            {
                targetRotation = closedRotation;
            }
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation,targetRotation, rotationSpeed * Time.deltaTime * 100f); //rotates the door to the new value using the rotation speed we set in the inspector

            if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f) //if the door's rotation is super close to the target rotation, snap it to the exact open or closed value and then mark that the door is no longer moving
            {
                transform.localRotation = targetRotation;
                isMoving = false;
            }
        }

    }

    public void Interact() //method gets called by the player when pressing E to interact with a door
    {
        if (!isMoving)
        {
            isOpen = !isOpen; //as long as the door is not already actively moving, reverse its state when we interact with it in game so that if it is closed, we can open it, and if it is open, we close it
            isMoving = true;
        }
    }


}
