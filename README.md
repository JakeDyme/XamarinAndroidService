 
# Dyme Xamarin Forms Android Service
## What is it?
- A background/foreground/service/notification thingy for Xamarin Android projects
(its what I would consider a background service, 
but its called a foreground service, and includes a notification that the user can interact with and monitor).
- A Nuget package for Xamarin Forms projects.

## Why have I done this?
If you've had a go at making an Android service,
then you're probably familiar with the spider web of management classes that you need to setup. 
After spending much time going through it myself, I decided to never let the journey end, 
and added yet another layer of management on top of that
(you can never have too much management).
Anyway I hope this saves you some time (and head-scratching).
I've tried to make it as simple as possible to use, and have successfully used it in one of my own projects.
Android is a continuously evolving beast though, so I cannot guarantee that it will work for all versions.
Good luck!

## Pre-Setup
You must add the *Foreground Service Permission* to your Android app's manifest...
```xml
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
```

## Implementations
There are several ways that you can implement the service. I'll start with the easiest.

### 1. The Easiest
This is if you just want a simple notification that lives beyond the lifetime of your application.
```C#
_service = await ServiceManager.Start("My Wacky Service", "Hello world!");
```
Note: These can't be dismissed by the user
