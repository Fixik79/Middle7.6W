using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjrctile;
    [SerializeField] private Transform spawnBulletPosition;


    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private float reloadTime;
    private Animator animator;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);  // Исправил опечатку: screenCenterPoin
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        // Логика прицеливания 
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;  
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10));

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10));
        }

        // Логика стрельбы с таймером перезагрузки
        // Уменьшаем таймер каждый кадр (если >0)
        if (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
            // Опционально: визуализация - перемещаем debugTransform для индикации таймера
            if (debugTransform != null)
            {
                debugTransform.position = mouseWorldPosition + Vector3.up * (reloadTime / 5f * 2f);  // Поднимаем индикатор
            }

            // Блокировка: если пытаешься стрелять во время перезагрузки
            if (starterAssetsInputs.shoot)
            {
                starterAssetsInputs.shoot = false;  // Сбрасываем input, чтобы не стрелять
                Debug.Log("Попытка стрелять во время перезагрузки! Осталось: " + reloadTime.ToString("F2") + " сек. Блокировка сработала.");
                return;
            }
        }
        else
        {
            // Визуализация: сбрасываем индикатор, когда таймер 0
            if (debugTransform != null)
            {
                debugTransform.position = mouseWorldPosition;  // Возвращаем на позицию курсора
            }

            // Разрешаем стрелять только если таймер <=0
            if (starterAssetsInputs.shoot)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfBulletProjrctile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                starterAssetsInputs.shoot = false;

                // Стартуем перезагрузку
                reloadTime = 5f;
                Debug.Log("Выстрел произведён! Перезагрузка начата на 5 сек.");
            }
        }
    }
}