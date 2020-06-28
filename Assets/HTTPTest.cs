using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class HTTPTest : MonoBehaviour
{

    public float ATMListItemOffset;

    public GameObject ATMList;
    public GameObject ATMScrollView;
    public GameObject ATMListItem;

    public GameObject bg1;
    public GameObject bg2;


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


    public void GetATMLocation(){

        string ATMRequestString = "{\r\n\t\"latitude\":\"1236547893\",\r\n\t\"longitude\":\"98765432139\",\r\n\t\"radius\":\"5\",\r\n\t\"city\":\"Istanbul\",\r\n\t\"district\":\"Ataşehir\",\r\n\t\"searchText\":\"akbank\"\r\n}\r\n";

        StartCoroutine(FindATM("https://apigate.akbank.com/api/hackathon/findATM", ATMRequestString));
            
        //bg1.SetActive(true);
        //bg2.SetActive(true);
        


    }





    // Start is called before the first frame update
    void Start()
    {
        ATMListItemOffset = 0.0f;

        //StartCoroutine(GetText());
        //StartCoroutine(Upload());

        //TestResponse tr = new TestResponse();
        //Debug.Log(ATMLokasyonResponse.CreateFromJSON("{\"name\":\"Dr Charles\",\"lives\":3,\"health\":0.8}"));

        //bg1.SetActive(false);
        //bg2.SetActive(false);


        string rawJson1 = "{\r\n\t\"latitude\":\"1236547893\",\r\n\t\"longitude\":\"98765432139\",\r\n\t\"radius\":\"5\",\r\n\t\"city\":\"Istanbul\",\r\n\t\"district\":\"Ataşehir\",\r\n\t\"searchText\":\"akbank\"\r\n}\r\n";
        string rawJson2 =  "{\"latitude\":\"1236547893\",\"longitude\":\"98765432139\",\"radius\":\"5\",\"city\":\"Istanbul\",\"district\":\"Ataşehir\",\"searchText\":\"akbank\"}";
        //StartCoroutine(FindATM("https://apigate.akbank.com/api/hackathon/findATM", rawJson1));
    }

    IEnumerator FindATM(string url, string json)
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

            ATMLokasyonResponse atmR = ATMLokasyonResponse.CreateFromJSON(uwr.downloadHandler.text);

            Debug.Log(atmR.data[0].district);

            for (int i = 0; i < atmR.data.Count; i++)
            {
                GameObject go = Instantiate(ATMListItem, new Vector3(ATMList.transform.position.x, ATMList.transform.position.y + ATMListItemOffset,0) , Quaternion.identity , ATMList.transform );
                
                Text t = go.transform.GetChild(0).GetComponent<Text>();
                t.text = "ADRES:\n" + atmR.data[i].address;
                t.text += "\n\nATM İsmi:\n" + atmR.data[i].name;


                ATMListItemOffset += -300.0f;

                /*if (i == 0)
                {
                    bg1.SetActive(true);
                    
                }else if(i == 1){
                    bg2.SetActive(true);

                }*/

               
            }

            bg1.GetComponent<Image>().color = new Color(1,1,1,0.5f); 
            bg2.GetComponent<Image>().color = new Color(1,1,1,0.5f); 



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
