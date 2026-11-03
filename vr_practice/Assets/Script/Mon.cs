using UnityEngine;

public class Mon : MonoBehaviour
{ 
    public string Angel;
    public int age;
    public int Balance;

    public int GetInfo(){
        return Balance;
    }
    public string GetName(){
        return "Пользователь"+Angel;
    }
    public void Deposit(int amount)
    {
        Balance += GetInfo();         
    }

    public void Withdraw(int amount)
    {
        if (Balance >= amount)
            Balance -= amount;
    }

    /*{
        return $"{Angel}: {Balance}";   
    }*/

    void Start()
    { 
        Balance=100;
        print  (Angel);
        print (GetName());
        print(Balance);
        Deposit(20);
        print(Balance);
    }

    void Update()
    {
        
    }
}
