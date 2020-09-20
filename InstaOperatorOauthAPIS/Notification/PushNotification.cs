using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace InstaOperatorOauthAPIS.Notification
{
    public class PushNotification
    {
        public void SendPushNotification(NotificationContent notificationContent)
        {
            try
            {
                string serverKey = "AAAAsqQfsm8:APA91bFLwCKKGgQVWm3_UpptkNkeJla_ZNI4BoaKxLoc2LB6I6NFdY7EKKlowaZLs1_wDRrSMz4IkzjP5j0N4AskU1Ors8H6PEu93R8jWv-yRWU8GX_iHE6VXZCVAJiZ_XVh3gL04AbK";
                string senderId = "767257719407";
                int badgeCounter = 1;

                //Create the web request with fire base API  
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = notificationContent.DeviceID,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = notificationContent.TextMessage,
                        title = notificationContent.Title.Replace(":", ""),
                        sound = "sound.caf",
                        badge = badgeCounter
                    },
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(payload);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //var serializer = new JavaScriptSerializer();
                //Byte[] byteArray = Encoding.UTF8.GetBytes(payload);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    string str = sResponseFromServer;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public void PushNotification2()
        {
            try
            {
                var applicationID = "AAAAsqQfsm8: APA91bFLwCKKGgQVWm3_UpptkNkeJla_ZNI4BoaKxLoc2LB6I6NFdY7EKKlowaZLs1_wDRrSMz4IkzjP5j0N4AskU1Ors8H6PEu93R8jWv - yRWU8GX_iHE6VXZCVAJiZ_XVh3gL04AbK";
                var senderId = "767257719407";
                string deviceId = "dEFMvuXG5mA:APA91bFT9jIMJnHMIfuH9uUqqmT4Lxbif7K1hDwhHHy8Gey5rR7iWoPjQpStWl4F5ah3TaOcsSQTwQ1que86Mo_ScuxngK_Hdib25iWRVwVW2jvuTr7nx7lZvrMsN-YLka3pAriXKrOE";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "Messageeeeee",
                        title = "Tag Message",
                        icon = "myicon"
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}