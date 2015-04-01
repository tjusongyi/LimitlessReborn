using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginPanel : PanelBase 
{
    private InputField name;
    private InputField pass;
    private InputField email;
    private Button submit;

    void Awake()
    {
        name = ObjectHelper.GetChildComponent<InputField>(gameObject, "name");
        pass = ObjectHelper.GetChildComponent<InputField>(gameObject, "pass");
        email = ObjectHelper.GetChildComponent<InputField>(gameObject, "email");
        submit = ObjectHelper.GetChildComponent<Button>(gameObject, "submit");

    }
    // Use this for initialization
    void Start()
    {
        submit.onClick.AddListener(OnSubmit);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnShow(object param)
    {
        throw new System.NotImplementedException();
    }

    void OnSubmit()
    {
        UserManager.Instance.Signup(name.text, pass.text, email.text);
    }
}
