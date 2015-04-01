using UnityEngine;
using System.Collections;
using System;
using BestHTTP;
using System.Security.Cryptography;
using cn.bmob.io;

public class UserManager : Singleton<UserManager>
{
    public static User TheUser;


    private int resultCode;

    public void Init()
    {
        TheUser = new User();
        resultCode = 0;
    }


    public string GetUserName()
    {
        return TheUser.username;
    }

    public void Signup(string name, string pass, string email)
    {

        TheUser.username = name;
        //MD5 md5 = new MD5CryptoServiceProvider();
        //byte[] result = md5.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(pass));
        //TheUser.password = System.Text.UTF8Encoding.UTF8.GetString(result);
        TheUser.password = pass;
        //邮箱用于找回密码
        TheUser.email = email;
        CloudManager.bmobUnity.Signup(TheUser, (resp, exception) =>
        {
            if (exception != null)
            {
                Debug.Log("注册失败, 失败原因为： " + exception.Message);
                return;
            }
            Debug.Log("注册成功");
        });
    }

    public int Login(string name,string pass)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            resultCode = (int)CommonDef.ResultCodes.EmptyUserName;
        else
        {
            //HttpManager.Instance.SendRequest(new Uri(HttpManager.baseUrl + "/login"), onLoginResponse);
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] result = md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(pass));
            //pass = System.Text.Encoding.Unicode.GetString(result);
            CloudManager.bmobUnity.Login<User>(name, pass, (resp, exception) =>
                {
                    if (exception != null)
                    {
                        Debug.Log("登录失败, 失败原因为： " + exception.Message);
                        return;
                    }

                    Debug.Log("登录成功, @" + resp.username + "(" + resp.objectId + ")$[" + resp.sessionToken + "]");
                    Debug.Log("登录成功, 当前用户对象Session： " + BmobUser.CurrentUser.sessionToken);

                });
          

        }
        return resultCode;
    }

    public int Logout()
    {
        //HttpManager.Instance.SendRequest(new Uri(HttpManager.baseUrl + "/logout"), onLoginResponse);
        return resultCode;
    }

    private void onLoginResponse(HTTPRequest request,HTTPResponse response)
    {
        
    }
}


