using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    Transform cameraTransform;
    float range = 100f;

    [SerializeField]

    float rawDamage = 10f;

    PlayerInput playerInput;
    InputAction attackAction;

    void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();

        var map = playerInput.currentActionMap;

        attackAction = map.FindAction("Attack", true);

    }
    void Update()
    {
        FireWeapon();
    }

    void FireWeapon()
    {
        if (attackAction.triggered)
        {
            LayerMask mask = ~LayerMask.GetMask("Player");
            cameraTransform = Camera.main.transform;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit raycastHit;
            LayerMask mask = ~LayerMask.GetMask("Player");
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 4, Color.blueViolet, 1f);
           
            
            
            
            
            
            
            
            
            if (Physics.Raycast(ray, out raycastHit, range))
            {
                if (raycastHit.transform != null)
                {
                    raycastHit.collider.SendMessageUpwards("Hit", rawDamage, SendMessageOptions.DontRequireReceiver);
                }
                
                    
                
            }
            else
            {
                Debug.Log("NO RAYCAST");
            }
        }
    }
}