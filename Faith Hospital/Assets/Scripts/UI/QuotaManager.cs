using TMPro;
using UnityEngine;

public class QuotaManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMoney;
    
    private static int money;
    private static int quota;

    public void UpdateMoney(int pMoneyWin)
    {
        money += pMoneyWin;
        string text = money.ToString() + "/" + quota;
        TextMoney.text = text;
    }

    public void SetQuota(int pQuota)
    {
        quota = pQuota;
        money = 0;
        UpdateMoney(money);
    }

    public bool QuotaMet()
    {
        return (money >= quota ||  quota == 0);   
    }
}
