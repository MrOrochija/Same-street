using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MarketTrigger : MonoBehaviour
{
    public Image fadeImage;
    private PlayerMovement playerMovement;

    private bool isPlayerInside = false;
    private bool isInteracting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerMovement = null;
        }
    }

    private void Update()
    {
        if (isPlayerInside && !isInteracting && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartCoroutine(InteractionRoutine());
            }
        }
    }

    private IEnumerator InteractionRoutine()
    {
        isInteracting = true;

        if (playerMovement != null)
        {
            playerMovement.currentState = PlayerState.Frozen;
        }

        // Отвязываем триггер и Canvas от родителей и спасаем их от удаления,
        // чтобы корутина не оборвалась и экран не остался черным.
        // ИГРОКА МЫ БОЛЬШЕ НЕ СПАСАЕМ (пусть умирает вместе со сценой).
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
        {
            Transform canvasRoot = fadeImage.transform.root;
            canvasRoot.SetParent(null);
            DontDestroyOnLoad(canvasRoot.gameObject);
        }

        // 1. Начинаем затемнение
        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 1f));

        // 2. Загружаем сцену
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("market");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 3. Ищем ИГРОКА УЖЕ НА НОВОЙ СЦЕНЕ и точку телепортации
        GameObject newScenePlayer = GameObject.FindGameObjectWithTag("Player");
        GameObject tpPos = GameObject.Find("MarketTpPos");
        
        if (newScenePlayer != null && tpPos != null)
        {
            // Тепаем нового игрока на нужную точку
            newScenePlayer.transform.position = tpPos.transform.position;
            
            PlayerMovement newMovement = newScenePlayer.GetComponent<PlayerMovement>();
            if (newMovement != null)
            {
                newMovement.currentState = PlayerState.Free;
            }
        }
        else
        {
            Debug.LogWarning("Игрок или точка MarketTpPos не найдены на новой сцене!");
        }

        // 4. Осветляем экран
        yield return StartCoroutine(FadeModule.FadeRoutine(fadeImage, 0f));

        isInteracting = false;

        // Удаляем этот скрипт и Canvas с затемнением, они больше не нужны
        if (fadeImage != null) Destroy(fadeImage.transform.root.gameObject);
        Destroy(gameObject);
    }
}