using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationSystemPerso : MonoBehaviour
{
    AndroidNotification notification = new AndroidNotification();
    void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notification",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        
        notification.Title = "Mobil Runner";
        notification.Text = "Come to Run";
        //notification.SmallIcon = "icon";
        notification.LargeIcon = "logo";

        notification.FireTime = System.DateTime.Now.AddMinutes(10); //Time
        AndroidNotificationCenter.SendNotification(notification, "channel_id");




    }

    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {

    }


}
