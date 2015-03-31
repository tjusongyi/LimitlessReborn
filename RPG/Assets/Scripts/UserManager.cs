using UnityEngine;
using System.Collections;
using System;
using BestHTTP;

public class UserManager : Singleton<UserManager>
{
    public static User TheUser;


    private int resultCode;

    public void Init()
    {
        TheUser = new User();
        resultCode = 0;
    }

    public void SetUserName(string name)
    {
        TheUser.Name = name;
    }

    public void SetUserPass(string pass)
    {
        TheUser.Pass = pass;
    }

    public string GetUserName()
    {
        return TheUser.Name;
    }

    public int Login()
    {
        if (string.IsNullOrEmpty(TheUser.Name) || string.IsNullOrEmpty(TheUser.Pass))
            resultCode = (int)CommonDef.ResultCodes.EmptyUserName;
        else
        {
            HttpManager.Instance.SendRequest(new Uri(HttpManager.baseUrl + "/login"), onLoginResponse);
        }
        return resultCode;
    }

    public int Logout()
    {
        HttpManager.Instance.SendRequest(new Uri(HttpManager.baseUrl + "/logout"), onLoginResponse);
        return resultCode;
    }

    private void onLoginResponse(HTTPRequest request,HTTPResponse response)
    {
        
    }
}


