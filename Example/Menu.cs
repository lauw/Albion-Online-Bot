using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;

namespace UnityGameObject
{
    public class Menu : MonoBehaviour
    {
        void Start()
        {
            
        }
        
        void Update()
        {
            //DrawRect(0, 0, 100, 100);
        }
        void OnGUI()
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(0, 0, 200, 40), "Working?");
        }
    }
}