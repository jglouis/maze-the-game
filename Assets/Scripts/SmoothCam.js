/*
        This script is used to average the mouse input over x
        amount of frames in order to create a smooth mouselook.
*/
//Mouse rotation input
private var rotationX : float = 0;
private var rotationY : float = 0;
 
//Mouse look sensitivity
public var sensitivityX : float = 2;
public var sensitivityY : float = 2;
 
//Default mouse sensitivity
public var defaultSensX : float = 2;
public var defaultSensY : float = 2;
 
//Used to calculate the rotation of this object
private var xQuaternion : Quaternion;
private var yQuaternion : Quaternion;
private var originalRotation : Quaternion;
 
//Minimum angle you can look up
public var minimumY : float = -60;
public var maximumY : float = 60;
 
//Number of frames to be averaged, used for smoothing mouselook
public var frameCounterX : int = 35;
public var frameCounterY : int = 35;
 
//Array of rotations to be averaged
private var rotArrayX = new Array ();
private var rotArrayY = new Array ();
///////////////////////////////////////////////////////
 
function Start()
{
//Lock/Hide cursor
       // Screen.lockCursor = true;
    if (rigidbody) rigidbody.freezeRotation = true;
        originalRotation = transform.localRotation;
}
///////////////////////////////////////////////////////
 
function Update()
{
//Mouse/Camera Movement Smoothing:     
//Average rotationX for smooth mouselook
        var rotAverageX : float = 0;
       
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
       
    //Add the current rotation to the array, at the last position
    rotArrayX[rotArrayX.length] = rotationX;
 
    //Reached max number of steps?  Remove the oldest rotation from the array
    if (rotArrayX.length >= frameCounterX)
    {
        rotArrayX.RemoveAt(0);
    }
 
    //Add all of these rotations together
    for (var i_counterX = 0; i_counterX < rotArrayX.length; i_counterX++)
    { //Loop through the array
        rotAverageX += rotArrayX[i_counterX];
    }
 
    //Now divide by the number of rotations by the number of elements to get the average
    rotAverageX /= rotArrayX.length;
   
//Average rotationY, same process as above
        var rotAverageY : float = 0;   
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;  
    rotArrayY[rotArrayY.length] = rotationY;
    if (rotArrayY.length >= frameCounterY)
    {
        rotArrayY.RemoveAt(0);
    }
    for (var i_counterY = 0; i_counterY < rotArrayY.length; i_counterY++)
    {
        rotAverageY += rotArrayY[i_counterY];
    }
    rotAverageY /= rotArrayY.length;
 
//Set Rotation Limits for vertical    
    if ((rotAverageY >= -360) && (rotAverageY <= 360))
    {
        rotAverageY = Mathf.Clamp (rotAverageY, minimumY, maximumY);
    }
    else if (rotAverageY < -360)
    {
        rotAverageY = Mathf.Clamp (rotAverageY+360, minimumY, maximumY);
    }
    else
    {
        rotAverageY = Mathf.Clamp (rotAverageY-360, minimumY, maximumY);
    }
 
//Apply and rotate this object
    xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
    yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
 
    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
}