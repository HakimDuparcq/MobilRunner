using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationSystemPerso : MonoBehaviour
{
    AndroidNotification[] notification = new AndroidNotification[4];
    void Start()
    {
        // REmove notif that have been display
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notification",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        for (int i = 0; i < notification.Length; i++)
        {
            notification[i].Title = "Mobil Runner";
            notification[i].Text = "Come to Run";
            //notification.SmallIcon = "icon";
            notification[i].LargeIcon = "logo";
            notification[i].FireTime = System.DateTime.Now.AddSeconds(10*i+1); //Time
            var id =  AndroidNotificationCenter.SendNotification(notification[i], "channel_id");
        }
        



    }

    

}
