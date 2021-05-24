 
# Dyme Xamarin Forms Android Service
## What is it?
- A background/foreground/service/notification for Xamarin Android projects
(its what you might consider a background service, 
but its called a foreground service, and includes a notification that the user can interact with and monitor).
- A Nuget package for Xamarin Forms projects.

## Pre-Setup
You must add the *Foreground Service Permission* to your Android app's manifest...
```xml
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
```

## Implementations
There are several ways that you can implement the service.

### Basic
This is if you just want a simple notification that lives beyond the lifetime of your application.  

```C#
// Create and start a service...
_service = await ServiceManager.Start("My Service", "Hello world!");
```

```C#
// Stop the service...
_simpleService.StopService(removeNotification: true);
```

### Advanced
If you want to perform tasks from within the service.
1. Implement the ISimpleService interface.
```C#
public class MyService : ISimpleService
{
    public void OnExecuteAction(string actionName, IList<string> args)
    {
        // Event handler for actions declared in the "SimpleServiceOptions"
    }

    public void OnStart(SimpleServiceCore core)
    {
        // Called when the service starts
    }

    public void OnStop(IList<string> args)
    {
        // Called when the service is stopped
    }

    public void OnTapNotification()
    {
        // Called when the user taps the notification
    }
}
```
2. Pass your service class into with the Start method

```C#
// Create and start a service...
_service = await ServiceManager.Start<MyService>("My Service", "Hello world!");
```

## Sample Apps
*SampleApp* shows you a simple way to start the service, and to update the notification. 

*SampleApp2* demonstrates a full implementation, and shows communication from the app to the service, and from the service to the App. It also shows examples of how to start the service automatically when the user navigates away from your application.


1. Click "Start Service", notice the notification created:  
![Start service image](SampleApp2/SampleApp2/ex1.png)  
2. Expand notification   
![Expand notification image](SampleApp2/SampleApp2/ex2.png)  
3. Use notification buttons, notice the app state in the background.  
![Notification buttons image](SampleApp2/SampleApp2/ex3.png)  