using System.Collections;
using UnityEngine;

public class GunMaterial : MonoBehaviour
{
    public Material[] materials;
    private Renderer rend;
    private bool isReloading = false;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Renderer не найден на объекте! Убедись, что скрипт на модели с Renderer.");
            return;
        }
        rend.material = materials[0];
        Debug.Log("Скрипт инициализирован, материал: " + materials[0].name);
    }

    void Update()
    {
        // Отладка: выводим состояние каждый кадр (можно отключить, если слишком много)
        // Debug.Log("isReloading: " + isReloading);

        if (isReloading)
        {
            // Если пытаемся стрелять во время перезагрузки — выводим сообщение
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Попытка стрелять во время перезагрузки! Блокировка сработала.");
            }
            return;  // Выходим из Update, не позволяем стрелять
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (materials.Length > 1)
            {
                rend.material = materials[1];  // Выстрел
                Debug.Log("Выстрел! Переключили материал на: " + materials[1].name);
            }
            else
            {
                Debug.LogWarning("Массив материалов пуст или имеет недостаточно элементов!");
            }

            isReloading = true;
            Debug.Log("Перезагрузка начата: isReloading = true. Ждём 5 сек.");
            StartCoroutine(ReloadProcess(5f));
        }
    }

    private IEnumerator ReloadProcess(float delay)
    {
        Debug.Log("Корутина начата, ждём " + delay + " сек.");
        yield return new WaitForSeconds(delay);

        if (rend != null && materials.Length > 0)
        {
            rend.material = materials[0];  // Возвращаем материал
            Debug.Log("Перезагрузка завершена: возвратили материал на " + materials[0].name);
        }
        isReloading = false;  // Разблокируем
        Debug.Log("Перезагрузка завершена: isReloading = false. Можно стрелять снова.");
    }
}