
public abstract class ClientHandler//child is createclient
{
    public ClientFactory ClientFactory { get; set; }
    public abstract Client CreateClient(string type, string name);//by default no body inside
}

public class RetailClientHandle : ClientHandler
{
    public RetailClientHandle()
    {
        ClientFactory = new ClientFactory();
    }

    public override Client CreateClient(string type, string name)
    {
        return ClientFactory.CreateClient(type, name);
    }
}

public class EnterpriseClientHandler : ClientHandler
{
    AccessBehavior AccessBehavior { get; set; }
    public EnterpriseClientHandler()
    {
        ClientFactory = new ClientFactory();
        AccessBehavior = new CheckString();
    }
    public override Client CreateClient(string type, string name)
    {
        return ClientFactory.CreateClient(type, name);
    }
}
public class ClientFactory
{
    public Client CreateClient(string type,string name)//if type is admin, factory can change to admin
    {
        Client newClient = new User(name);

        if (type == "Manager")
        {
            newClient = new Manager(name);
        }
        else if (type == "Admin")
        {
            newClient = new Admin(name);
        }
        newClient.BuildAuthString(name);
        return newClient;
    }
}
public interface Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }
    public bool HasAccess { get; set; }
    public void BuildAuthString(string UserName);
}

public class User : Client
{

    public string UserName { get; set; }
    public string UserAuthString { get; set; }
    public bool HasAccess { get; set; }

    public User(string name)
    {
        HasAccess = false;
    }

    public void BuildAuthString(string UserName)
    {
        UserAuthString = UserName;
    }
}

public class Manager : Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }
    public bool HasAccess { get; set; }

    public Manager(string name)
    {
        UserName = name;
        HasAccess = true;
    }
    public void BuildAuthString(string UserName)
    {
        UserAuthString = UserName+"ADMIN";
    }
}


public class Admin : Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }
    public bool HasAccess { get; set; }

    public Admin(string name)
    {
        UserName = name;
        HasAccess = true;
    }
    public void BuildAuthString(string UserName)
    {
        UserAuthString = UserName + "ADMIN";
    }
}

public interface AccessBehavior
{
    public Client Client { get; set; }

    public bool HandleAccess(Client User);

}

public class CheckString : AccessBehavior
{
    public Client Client { set; get; }

    public CheckString()
    {
        
    }
    public bool HandleAccess(Client user)
    {
        var original = user.UserAuthString;
        bool result = false;
        if (original.Substring(original.Length - 5) == "ADMIN")
            result = true;
        return result;
    }
    
}

public class SwitchAuth: AccessBehavior
{
    public Client Client { set; get; }
    public bool HandleAccess(Client user)
    {
        if(user.HasAccess)
        {
            user.HasAccess = false;
        } else if(!user.HasAccess)
        {
            user.HasAccess = true;
        }
        return user.HasAccess;
    }
}