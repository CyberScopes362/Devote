AndNotify

Version: 1.0.3
Created by: Q Virtuality
E-mail: q.virtuality@gmail.com
Asset Store link: https://www.assetstore.unity3d.com/en/#!/content/31493
Short link: http://u3d.as/cuS

/**************************************************************************************************/

Description:

Simple and easy implementation for showing offline notifications on Android devices. Simply drag and drop a Unity prefab included in the package and fill up the required parameters to set up a notification for Android device!

/**************************************************************************************************/

Features:

1. Simply drag and drop the provided Unity prefab to the current scene and change the parameters!

2. You can show notifications multiple times within a certain interval that you can set!

3. Notifications will contain your app icon, Heading text and content text.

4. Every notification will also create a notification sound.

5. Pending Notifications will be resumed if the device is rebooted.

6. Clicking on a Notification will launch your game.

7. Tested and works on Android API 26 Oreo devices.

/**************************************************************************************************/

Instructions:

1. Install the package from Unity Asset Store and import all the files and folders.

2. Go to Assets/Plugins/Android folder on the Project panel. Select the prefab named "AndNotifyObject". In the Inspector window, if you see the warning message "The associated script can not be loaded. Please fix any compile errors and assign a valid script.", that means you are using some Unity version whitch doesn't match with the package Unity version. If you don't see the warning message, skip to step 6.

3. From the same folder, select the file named "AndNotify" (it's a *.dll file). After selection, in the Inspector window, you will see "Select platforms for plugin". If you don't see it, skip to step 5.

4. From "Select platforms for plugin", Make sure to check both Editor and Android, then click "Apply". You will see an arrow on the right side of the "AndNotify" file. Select the "AndNotifyObject" prefab again. If you don't see the warning message anymore, skip to step 6.

5. Now click the arrow on "AndNotify" file, you will see another file (script class) inside it named "Notification". Drag only the "Notification" file on the inspector missing script empty input box and drop it. 

6. Now you will see some fields for your notification on the inspector window under Notification (Script) component. Their purpose are as follows:

Title: The title of the notification.
Content: The content description of the notification.
Notification ID: Unique ID (integer) for each notification. If you create multiple notifications with same ID, the latest launched notification will be shown.
Days: After how many days you want the notification to appear. 0 for today.
Hours: After how many hours you want the notification to appear. This hour value is added with the day.
Minutes: After how many minutes you want the notification to appear. Added with days and hours values.
Seconds: After how many seconds you want the notification to appear. Added with days, hours and minutes values.
Count: How many times the notification will be repeated. put 0 to show the notification only once. The repetition interval is the sum of Days, Hours, Minutes and Seconds.

7. You know how to work with prefabs. So drag the prefab to the Hierarchy panel to create a Game Object. You can also create an empty Game Object and add the "Notification" script to it. (Check step 5). You can turn the notification on and off by Activating and deactivating the Game Object (can be from your own script). Once a notification is started by the game, it can not be turned off but can be replaced by another notification. (Hopefully more to come in later versions). There is a scene provided in the Assets folder, which may not be very useful for Unity versions greater than 4.0.

8. To be able to manipulate from a script, an example script is provided named "Test.cs". This script needs to be added to an empty Game Object and the notification Game Object (from step 7) needs to be added to the script as a reference.

9. There is an AndroidManifest.xml file provided, which will be automatically merged with the Unity Game/Application. But make sure the minimum Android SDK version of your game is 14. Otherwise Notifications may not work. Greater than 14 is fine.

10. (Optional) If you want to reduce some extra permission, you can prevent notifications be resumed after the device is rebooted by removing the following tags from AndroidManifest.xml file:

<receiver android:name="com.q.andnotify.BReceiver">
    <intent-filter>
        <action android:name="android.intent.action.BOOT_COMPLETED"/>
        <action android:name="android.intent.action.QUICKBOOT_POWERON" />
    </intent-filter>
</receiver>

And also remove the permission:

<uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED" />

11. (Optional) If you know how to use one script class from another, you can also use the "Notification" class and create a notification like this:

using AndNotify;

......

Notification n = new Notification(Title, Content)
                        .setID(notificationID)
                        .setInterval(Days * 24 * 60 * 60 + Hours * 60 * 60 + Minutes * 60 + Seconds)
                        .setCount(Count)
                        .start();

Function definitions:

	a. public Notification(string Title, string Content) // Constructor, returns itself

	b. public Notification setID(int notificationID) // sets the notification ID. Optional to use. default notification ID is 0.

	c. public Notification setInterval(int Seconds) // sets the notification repeat Interval in seconds. Optional to use. default Interval value is 0.

	d. public Notification setCount(int Count) // sets the notification repeat counter (Total number of notificatons). Optional to use. default counter value is 0 (1 notification).

	e. public Notification start(void) //  starts the Notification. Use this function anywhere to start new or override existing notification.

/**************************************************************************************************/

Version 1.0.3 change log:

1. Tested and works with Unity version 4.0.0f7, 5.0.0f4, 5.6.4f1 and 2017.2.0f3.
2. Added Notification ID support for sending multiple notifications.
3. Some internal changes and bug fixes.

/**************************************************************************************************/

Version 1.0.2 change log:

1. This plug-in is tested and works with Unity version 2017.1.1f1.
2. Minimum supported Unity version is 4.0.0f7.
3. This plug-in is tested and works on Android version 8 (API level 26) (Oreo) devices.
4. Minimum supported Android SDK version is API level 14 (Adroid 4.0 IceCreamSandwitch).

/**************************************************************************************************/

Version 1.0.1 change log:

1. Notifications will be resumed even after device reboot.
2. Application Package name is not required as a parameter any more.
3. This plug-in will also work on Unity 5.0.1f1 32bit.