using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{

    public GameObject WeaponBack;
    public GameObject WeaponAttack;

    public void ShowAttack()
    {
        WeaponAttack.SetActive(true);
        WeaponBack.SetActive(false);
    }

    public void HideAttack()
    {
        WeaponAttack.SetActive(false);
        WeaponBack.SetActive(true);
    }
}
