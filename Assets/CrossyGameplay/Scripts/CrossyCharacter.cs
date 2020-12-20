using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrossyCharacter : MonoBehaviour
{
    #region Public Methods
    public void Move(Vector2 direction)
    {
        if (IsDead == true)
        {
            return;
        }

        if (isMoving == true)
        {
            return;
        }

        //Si hay un obstaculo, no mover
        if (CanMoveDirection(direction) == false)
        {
            return;
        }

        Vector3 position = transform.position;

        //Movimiento vertical
        if (direction.y > 0 || direction.y < 0)
        {
            currentMovingPlatform = null;
        }

        //Igual a position.x = position.x + direction.x
        position.x += direction.x * movementAmount;
        position.z += direction.y * movementAmount;

        isMoving = true;
        myAnimator.SetTrigger("move");

        //transform.position = position;
        Tween tween = transform.DOMove(position, movementTime);
        tween.SetEase(easeType);
        tween.OnComplete(OnCompleteMoving);

        ApplyRotation(direction);
    }
    #endregion

    #region Public Vars
    public float movementAmount = 2;
    public float movementTime = 0.7f;
    public Ease easeType = Ease.Linear;

    public float rotationTime = 0.2f;

    public float obstacleRayDistance = 1.5f;
    public LayerMask obstacleLayermask;

    public Transform groundRayOrigin;
    public float groundRayDistance = 3f;
    public LayerMask groundLayermask;

    public Animator myAnimator;
    public Transform visualElement;

    public CrossyBlockType currentGroundType = CrossyBlockType.Grass;
    public MovingObject currentMovingPlatform;
    public bool IsDead = false;
    #endregion

    #region Private Methods
    private void Start()
    {
        DetectGround();
    }

    private void OnCompleteMoving()
    {
        isMoving = false;

        DetectGround();
    }

    private void ApplyRotation(Vector2 direction)
    {
        Vector3 eulerAngles = Vector3.zero;
        if (direction == Vector2.up)
        {
            walkedRows++;
            eulerAngles = Vector3.zero;
        } else if (direction == Vector2.right)
        {
            eulerAngles = new Vector3(0, 90, 0);
        } else if (direction == Vector2.left)
        {
            eulerAngles = new Vector3(0, -90, 0);
        } else if (direction == Vector2.down)
        {
            eulerAngles = new Vector3(0, 180, 0);
            walkedRows--;
        }

        ScoreManager.Instance.SetPlayerPosition(walkedRows);
        Tween tween = visualElement.DORotate(eulerAngles, rotationTime);
    }

    private bool CanMoveDirection(Vector2 direction)
    {
        Vector3 worldDirection = new Vector3(direction.x, 0f, direction.y);

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, worldDirection, out hit, obstacleRayDistance, obstacleLayermask))
        {
            return false;
        } else
        {
            return true;
        }
    }
    
    private void DetectGround()
    {
        Vector3 rayDirection = new Vector3(0, -1f, 0f);

        RaycastHit hit;
        //Detectar un collider con ground layermask
        if (Physics.Raycast(groundRayOrigin.position, rayDirection, out hit, groundRayDistance, groundLayermask))
        {
            CrossyPlatform detectedPlatform = hit.collider.GetComponent<CrossyPlatform>();

            if (detectedPlatform != null)
            {
                currentGroundType = detectedPlatform.type;
                
                if (detectedPlatform.tag == "MovingPlatform")
                {
                    currentMovingPlatform = detectedPlatform.GetComponent<MovingObject>();
                }
                return;
            }
            
            CrossyBlock detectedBlock = hit.collider.GetComponentInParent<CrossyBlock>();

            if (detectedBlock != null)
            {
                currentGroundType = detectedBlock.type;

                if (currentGroundType == CrossyBlockType.Water)
                {
                    IsDead = true;

                    myAnimator.SetTrigger("Idle to DeathByDrowning");
                }
            }
        }
        else
        {
            Debug.Log("No existe suelo para detectar");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            IsDead = true;

            myAnimator.SetTrigger("Idle to DeathByCarAccident");
        }
    }

    private void Update()
    {
        //Handle moving platform
        if (currentMovingPlatform != null)
        {
            Vector3 currentPosition = transform.position;
            currentPosition += currentMovingPlatform.speed * Time.deltaTime;

            transform.position = currentPosition;
        }
    }
    #endregion

    #region Private Vars
    private bool isMoving = false;
    private int walkedRows = 0;
    #endregion
}
