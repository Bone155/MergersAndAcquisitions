using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Control : MonoBehaviour
{
    [Header("Main")]
    public List<Vector3> offsetFromObject;
    [Header("Platform Tag")]
    public string platformTag; // used to get the platform tag for the object
    [Header("Buildings Tag")]
    public string buildingsTag; // used to see if the object that was clicked has that tag.
    [Header("Camera")]
    public Camera camera; // used to get the camera that needs to be used
    private Vector3 orgPoint; // orginial point of the object used if there is no match.
    private Transform orgParent; // gets the orginial parent
    private RaycastHit2D target; // this would be the building that we are targeting that we want to move.
    private Transform targetTransform; // this will be our targets transform
    private bool validTarget = false; // checks if we have a valid target to touch and move
    private Touch touch; // used for the touch
    private Vector2 touchPosition; // position for the touch
    private RaycastHit2D parent; // will be the targets parent
    private Transform parentTransform; // will be the parents transform
    private Vector3 offset; // offset is to offset the touchpos to give it a nice drag effect
    private static Control instance; // gets a instance of control
    private bool cancelTouch = false; // this will cancel the touch if its true and you can set it to be canceled
    private Transform selectedObject; // will be the object you select
    private bool isSelected = false; // will be the selected object
    AudioSource audioSource;
    public AudioClip dropClip;
    public AudioClip pickupClip;
    [HideInInspector]
    public static List<Vector3> offsetFromParent;
  
    private void Start()
    {
        instance = this;
        offsetFromParent = offsetFromObject;
    }
    private void Update()
    {
        if(Input.touchCount > 0 && !camera.Equals(null) && !cancelTouch)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            // if the target is not valid will do a raycast for it
            if (!validTarget)
            {
                DoRayCast();
            }
        }
    }
    private void LateUpdate()
    {
        if (Input.touchCount > 0 && !camera.Equals(null) && !cancelTouch)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            // if the target is found will do this type of action
            if (validTarget)
            {
                DoAction();
            }
        }
    }
    // Gets a instance of the class
    public static Control Instance()
    {
        if (instance == null) { return null; }
        else { return instance; }
    }
    public static Vector3 GetOffSet(GameObject target)
    {
        PositionTracker tracker;
        if(offsetFromParent != null && target != null && target.TryGetComponent<PositionTracker>(out tracker))
        {
            int value = (int)tracker.tier;
            return offsetFromParent.Count > 0 ? offsetFromParent[value]: Vector3.zero;
        }
        return Vector3.zero;
    }
    // Will set it to cancel to true
    public void SetCancel()
    {
        cancelTouch = true;
    }
    // sets the cancel to false
    public void RemoveCancel()
    {
        cancelTouch = false;
    }
    // gets the current target
    public Transform GetTarget()
    {
        return isSelected ? selectedObject : null;
    }
    // returns is the touch at the beginning
    public bool isTouchBegan()
    {
        return touch.phase == TouchPhase.Began ? true : false;
    }
    // returns if the touch is moving
    public bool isTouchMoving()
    {
        return touch.phase == TouchPhase.Moved ? true : false;
    }
    // returns if the touch has ended
    public bool isTouchEnded()
    {
        return touch.phase == TouchPhase.Ended ? true : false;
    }
    //returns the touch position
    public Vector3 TouchPos()
    {
        return touchPosition;
    }
    // this will check what phase the touch is currently on
    private void DoAction()
    {
        if (targetTransform == null) { validTarget = false; return; }
        switch (touch.phase)
        {
            case TouchPhase.Began:
                offset = targetTransform.position - (Vector3)touchPosition;
                break;
            case TouchPhase.Moved:
                targetTransform.position =  (Vector3)touchPosition + offset;
                break;
            case TouchPhase.Ended:
                validTarget = false;
                bool foundParent = false;
                RaycastHit2D[] parents = Physics2D.RaycastAll(targetTransform.position, Vector3.zero, Mathf.Infinity);
                // looping through to see if it can find the correct object it needs to parent with
                for(int i = 0; i < parents.Length; i++)
                {
                    parent = parents[i];
                    parentTransform = parent.transform;
                    if (parent && parentTransform.CompareTag(platformTag))
                    {
                        foundParent = true;
                        // checking if the parent already has a building on it
                        foreach (Transform child in parentTransform)
                        {
                            if (child.CompareTag(buildingsTag))
                            {
                                foundParent = false;
                                break;
                            }
                        }
                        if (foundParent)
                        {
                            // Found it's new parent and it has now dropped.
                            targetTransform.parent = parentTransform;
                            targetTransform.position = parentTransform.position + GetOffSet(targetTransform.gameObject);
                            // Any sounds can be played here - this is when it is successful
                            audioSource = GetComponent<AudioSource>();
                            audioSource.clip = dropClip;
                            audioSource.Play();
                            PositionTracker tracker;
                            if (targetTransform.gameObject.TryGetComponent<PositionTracker>(out tracker))
                            {
                                tracker.bufferPos = true;
                            }
                        }
                        // This just  breaks out of it
                        break;
                    }
                }
                if (!foundParent)
                {
                    // Can play any other sounds if you want it but it failed to find a object
                    // settings the orginial position since it could not parent with anything
                    targetTransform.position = orgPoint;
                    targetTransform.parent = orgParent;
                    PositionTracker tracker;
                    if(targetTransform.gameObject.TryGetComponent<PositionTracker>(out tracker))
                    {
                        //tracker.bufferPos = true;
                    }
                }
                break;
        }
    }
    // does a raycast than loops through to find the right object that needs to be selected for the building
    private void DoRayCast()
    {
        bool foundTarget = false;
        RaycastHit2D[] targets = Physics2D.RaycastAll(touchPosition,Vector2.zero, Mathf.Infinity);
        // loops through to find all the targets it can find that would match with the building tag
        for(int i = 0; i < targets.Length; i++)
        {
            target = targets[i];
            targetTransform = target.transform;
            if (target && targetTransform.gameObject.CompareTag(buildingsTag))
            {
                isSelected = true;
                validTarget = true;
                orgParent = targetTransform.parent;
                orgPoint = targetTransform.position;
                foundTarget = true;
                target.transform.parent = null;
                selectedObject = target.transform;
                // Any sounds can be played here
                audioSource = GetComponent<AudioSource>();
                audioSource.clip = pickupClip;
                audioSource.Play();
                PositionTracker tracker;
                if (targetTransform.gameObject.TryGetComponent<PositionTracker>(out tracker))
                {
                    //tracker.bufferPos = true;
                }
                break;
            }
        }
        if(!foundTarget)
        {
            validTarget = false;
            isSelected = false;
        }
    }
}
