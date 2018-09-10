using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public RoleConfig m_roleConfig;

    private static DataManager instance = null;

    public static DataManager SharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = MainComponentManager.AddMainComponent<DataManager>();
            }
            return instance;
        }
    }

}


