using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPTest : MonoBehaviour
{
    [System.Serializable]
    public class ATMData
    {
        public string city;
        public string district;
        public string address;
        public string name;
        public string latitude;
        public string longitude;
        public string deviceType;
        public string exchangeAvailable;
    }

    [System.Serializable]
    public class ATMLokasyonResponse
    {
        public string returnCode;
        public string returnMessage;
        public List<ATMData> data;

        public static ATMLokasyonResponse CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ATMLokasyonResponse>(jsonString);
        }

        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }




    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetText());
        //StartCoroutine(Upload());

        //TestResponse tr = new TestResponse();
        //Debug.Log(ATMLokasyonResponse.CreateFromJSON("{\"name\":\"Dr Charles\",\"lives\":3,\"health\":0.8}"));



        string rawJson1 = "{\r\n\t\"latitude\":\"1236547893\",\r\n\t\"longitude\":\"98765432139\",\r\n\t\"radius\":\"5\",\r\n\t\"city\":\"Istanbul\",\r\n\t\"district\":\"Ataşehir\",\r\n\t\"searchText\":\"akbank\"\r\n}\r\n";
        string rawJson2 =  "{\"latitude\":\"1236547893\",\"longitude\":\"98765432139\",\"radius\":\"5\",\"city\":\"Istanbul\",\"district\":\"Ataşehir\",\"searchText\":\"akbank\"}";
        StartCoroutine(PostRequest("https://apigate.akbank.com/api/hackathon/findATM", rawJson1));
    }

    IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("apikey", "l7xxc94da960bd0a40ac87b31f39e872518c");
        

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            Debug.Log(ATMLokasyonResponse.CreateFromJSON(uwr.downloadHandler.text).data[1].latitude);


            //JObject json = JObject.Parse(uwr.downloadHandler.text);
            //Debug.Log("Ret Code " + uwr.downloadHandler.text.returnCode);
        }
    }

    IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("http://www.postman-echo.com/get");
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
