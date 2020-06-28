using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PaymentManager : MonoBehaviour
{

    public GameObject ApprovalCanvas;
    public GameObject PaymentCanvas;

    public GameObject havaleMessage; 
    
    public float ATMListItemOffset;

    public GameObject ATMList;
    public GameObject ATMScrollView;
    public GameObject ATMListItem;

    public GameObject bg1;
    public GameObject bg2;


    [System.Serializable]
    public class HavaleResponseDetails
    {

        public string return_;
        public string transactionId;
        public string transactionFee;

    }

    [System.Serializable]
    public class HavaleResponse
    {
        public string returnCode;
        public string returnMessage;
        public HavaleResponseDetails data;

        public static HavaleResponse CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<HavaleResponse>(jsonString);
        }

        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HavaleCoroutine(){


        
        string HavaleRequestString = "{\r\n\t\"fromAccountNo\":\"123654789012365445698741\",\r\n\t\"toAccountNo\":\"123654789012365445698749\",\r\n\t\"amount\":\"5000\",\r\n\t\"description\":\"okul taksidi\",\r\n\t\"receiverName\":\"Mehmet Aslan\"\r\n}";


        StartCoroutine(Havale("https://apigate.akbank.com/api/hackathon/intraTransferService", HavaleRequestString));

    }

    IEnumerator Havale(string url, string json)
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

            HavaleResponse hvlR = HavaleResponse.CreateFromJSON(uwr.downloadHandler.text);



            //GameObject go = Instantiate(ATMListItem, new Vector3(ATMList.transform.position.x, ATMList.transform.position.y + ATMListItemOffset,0) , Quaternion.identity , ATMList.transform );

            havaleMessage.SetActive(true);
            havaleMessage.GetComponent<Text>().text = hvlR.returnMessage;
        
            ApprovalCanvas.SetActive(true); 
            PaymentCanvas.SetActive(false); 
            
            /*
            Text t = go.transform.GetChild(0).GetComponent<Text>();
            t.text = "Return:\n" + hvlR.data.return_;
            t.text += "\n\nATM İsmi:\n" + hvlR.data.name;


            ATMListItemOffset += -300.0f;
            */
            /*if (i == 0)
            {
                bg1.SetActive(true);
                
            }else if(i == 1){
                bg2.SetActive(true);

            }*/

               

        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
