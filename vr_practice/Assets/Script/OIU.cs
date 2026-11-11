using System;
using System.Collections;
using System.Globalization;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class OIU : MonoBehaviour
{
    [Header("Настройки сети")]
    public string url = "https://cnc.kovalev.team/get/3"; // URL для GET
    public string commandUrl = "https://cnc.kovalev.team/post"; // URL для POST (уточните адрес у преподавателя)
    public int channel = 1;
    public float interval = 0.5f;

    [Header("Узлы станка")]
    public Transform nodeX;
    public Transform nodeY;
    public Transform nodeZ;

    [Header("Интерфейс")]
    public TMP_Text panel; // Ссылка на панель
   
    private Vector3 startX;
    private Vector3 startY;
    private Vector3 startZ;

    private IEnumerator Start()
    {
        if (nodeX == null || nodeY == null || nodeZ == null)
        {
            Debug.LogError("Назначьте три узла станка в Inspector.");
            yield break;
        }

        if (panel == null)
        {
            Debug.LogError("Назначьте панель (MachinePanel) в Inspector.");
        }

        startX = nodeX.localPosition;
        startY = nodeY.localPosition;
        startZ = nodeZ.localPosition;

        // Цикл опроса сервера
        while (true)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.timeout = 10;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        // 1. Двигаем узлы (ваш старый код)
                        MoveNodes(request.downloadHandler.text);
                        
                        // 2. Обновляем панель (новый код)
                        if (panel != null)
                        {
                            // Создаем временный объект для панели, парся JSON
                            JObject data = JObject.Parse(request.downloadHandler.text);
                            float x = GetCoordinate(data, "X");
                            float y = GetCoordinate(data, "Y");
                            float z = GetCoordinate(data, "Z");
                            
                            // В методичке не указано, откуда брать running, 
                            // поэтому ставим true или false в зависимости от вашей логики.
                            // Пока оставим заглушку.
                            //MachineState state = new MachineState { running = true, x = x, y = y, z = z };
                            //panel.ShowState(state);
                            
                            // Убираем ошибку связи, если она была
                            //panel.ShowMessage("Связь установлена");
                        }
                    }
                    catch (Exception error)
                    {
                        Debug.LogWarning("Ошибка чтения JSON: " + error.Message);
                        //if (panel != null) panel.ShowMessage("Ошибка данных: " + error.Message);
                    }
                }
                else
                {
                    Debug.LogWarning("Ошибка запроса: " + request.error);
                    // Вывод ошибки на панель (Стр. 19)
                    //ShowMessageif (panel != null) panel.ShowMessage("Нет связи: " + request.error);
                }
            }

            yield return new WaitForSecondsRealtime(Mathf.Max(0.1f, interval));
        }
    }

    // --- Новые методы для кнопок ---

    public void StartMachine()
    {
        StartCoroutine(SendCommand("start"));
    }

    public void StopMachine()
    {
        StartCoroutine(SendCommand("stop"));
    }

    private IEnumerator SendCommand(string command)
    {
        // Показываем сообщение перед отправкой
        //if (panel != null) panel.ShowMessage("Отправка команды...");

        // Формируем JSON для отправки (зависит от вашего API)
        string jsonBody = "{\"command\":\"" + command + "\"}";
        
        using (UnityWebRequest request = new UnityWebRequest(commandUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                // Ошибка команды (Стр. 18)
                //if (panel != null) panel.ShowMessage("Ошибка команды: " + request.error);
            }
            else
            {
                // Команда принята (Стр. 18)
                //if (panel != null) panel.ShowMessage("Команда принята. Ожидание...");
                Debug.Log("POST успешен: " + request.downloadHandler.text);
            }
        }
    }

    // --- Ваши старые методы (без изменений) ---

    private float GetCoordinate(JObject data, string axis)
    {
        foreach (JToken section in data["data"])
        {
            if ((string)section[0] != "Оси Канала " + channel)
                continue;

            foreach (JToken row in section[1])
            {
                if ((string)row[0] == "Перемещение по оси " + axis)
                {
                    string text = ((string)row[1]).Replace("мм", "").Trim();
                    text = text.Replace(',', '.');
                    float value = float.Parse(text, CultureInfo.InvariantCulture);

                    if (float.IsNaN(value) || float.IsInfinity(value))
                        throw new FormatException("Некорректная координата " + axis);

                    return value;
                }
            }
        }
        throw new FormatException("Не найдена координата " + axis);
    }

    private void MoveNodes(string json)
    {
        JObject data = JObject.Parse(json);
        float x = GetCoordinate(data, "X") * 0.001f;
        float y = GetCoordinate(data, "Y") * 0.001f;
        float z = GetCoordinate(data, "Z") * 0.001f;

        nodeX.localPosition = startX + Vector3.right * x;
        nodeY.localPosition = startY + Vector3.forward * y;
        nodeZ.localPosition = startZ + Vector3.up * z;
        panel.text = ""+x;
       
    
    }
}