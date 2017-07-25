using System;
using System.IO;
using UnityEngine;

namespace UnityGameObject
{
	public class Menu : MonoBehaviour
	{
        /*  */
        private bool hackEnabled = false;
        private bool Menub = false;
        private float deltaTime;
        private bool hidedebug = true;

        private string debug = "";
        private Texture2D Texture;

        private Vector3 tp = new Vector3(0, 0, 0);


        /* Hack settings stuff */
        private bool buyeverytime = false;
        private bool unammo = false;
        private bool esp = false;
        private bool noClip = false;
        private float speedHack = 2f;

        /* Fly CAM */
        private float lookSpeed = 2.0F;
        private float moveSpeed = 8.0F;
        private float rotationX = 0.0F;
        private float rotationY = 0.0F;
        /* */

        [System.Runtime.InteropServices.DllImport("Dx11DrawingTools.dll")]
		public static extern void InitDx11();

		[System.Runtime.InteropServices.DllImport("Dx11DrawingTools.dll")]
		public static extern void DrawRect(float x, float y, float w, float h);

        /* Draw text on screen from worldspace */
		private void DrawLabel(Vector3 point, string label, Color color)
		{
            Vector3 vector = Camera.main.WorldToScreenPoint(point);
			Vector3 value = vector;
			Vector2 vector2 = GUIUtility.ScreenToGUIPoint(value);
			vector2.y = (float)Screen.height - (vector2.y + 1f);
			GUI.color = color;
			GUI.Label(new Rect(vector2.x - 64f, vector2.y - 12f, 500f, 48f), label);
        }

        /* from Box not working ... */
        public void BoxRect(float x, float y, float w, float h)
        {
            if (Texture != null)
            {
                GUI.DrawTexture(new Rect(x, y, w, h), Texture);
            }
        }
        /* Box (for ESP) not working ... */
        public void Box(float x, float y, float w, float h, float thick)
        {
            BoxRect(x, y, w, thick);
            BoxRect(x, y, thick, h);
            BoxRect(x + w, y, thick, h);
            BoxRect(x, y + h, w + thick, thick);
        }
        /* Calc distance between player and worldspace */
        private float distanceCalc(Vector3 point)
		{
			return Vector3.Distance(Camera.main.transform.position, point);
		}
        /* Add message to debuglog */
        private void debugLog(string txt)
        {
            debug = txt + debug;
        }
        /* Called on Start*/
        private void Start()
		{
			InitDx11();
        }
        /* Called every frame */
        private void Update()
        {
            GameObject LocalPlayer = GameObject.Find("LocalPlayer");

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            /* Check pressed keys */
            if (Input.GetKeyDown(KeyCode.Home))
            {
                hackEnabled = !hackEnabled;
            }
            if (hackEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Insert))
                {
                    Menub = !Menub;
                }
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Y))
                {
                    esp = !esp;
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    unammo = !unammo;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Award.SetMoney(5000);
                    Award.SetCustomPoints(100, 0);
                    Award.SetPoints(100);
                    Award.SetAward(500);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    string tempd = "";

                    Component[] c = LocalPlayer.GetComponents(typeof(Component));
                    foreach (Component co in c)
                    {
                        tempd += " -" + co.GetType().FullName + "\n";
                    }
                    foreach (Transform child in LocalPlayer.transform)
                    {
                        tempd += " -> " + child.name + "\n";
                        Component[] ci = child.GetComponents(typeof(Component));
                        foreach (Component ca in ci)
                        {
                            if (ca.GetType().FullName != "UnityEngine.Transform")
                                tempd = "    - " + ca.GetType().FullName + "\n";
                        }
                        foreach (Transform child2 in child)
                        {
                            tempd += "  -> " + child2.name + "\n";
                            Component[] ci2 = child2.GetComponents(typeof(Component));
                            foreach (Component ca2 in ci2)
                            {
                                if (ca2.GetType().FullName != "UnityEngine.Transform")
                                    tempd += "    - " + ca2.GetType().FullName + "\n";
                            }
                            foreach (Transform child3 in child2)
                            {
                                tempd += "   -> " + child3.name + "\n";
                                Component[] ci3 = child3.GetComponents(typeof(Component));
                                foreach (Component ca3 in ci3)
                                {
                                    if (ca3.GetType().FullName != "UnityEngine.Transform")
                                        tempd += "     - " + ca3.GetType().FullName + "\n";
                                }
                            }
                        }
                    }
                    debugLog(tempd);
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    hidedebug = !hidedebug;
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    string savePath = (Environment.SpecialFolder.Desktop + @"\debug.txt");

                    if (!File.Exists(savePath))
                    {
                        File.Create(savePath).Dispose();
                        File.WriteAllText(savePath, debug);
                    }
                }




                if (Input.GetKeyDown(KeyCode.H))
                {

                }
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    vp_FPInput.cs.Player.SetWeaponByName.Try(BasePlayer.weapondata[22].selectname);
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    tp = LocalPlayer.transform.position;
                }
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    LocalPlayer.transform.position = tp;
                }
                if (Input.GetKeyDown(KeyCode.F4))
                {
                    LocalPlayer.transform.position = LocalPlayer.transform.position + new Vector3(0, 3, 0);
                }

                /* Numeric keys 4,5,6,7,8,9,0 */
                /* Delete top row from debuglog */
                if (Input.GetKey(KeyCode.Alpha4))
                {
                    debug = debug.Substring(debug.IndexOf('\n') + 1);
                }
                /* NoClip */
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    LocalPlayer.GetComponent<vp_FPController>().enabled = !LocalPlayer.GetComponent<vp_FPController>().enabled;
                    noClip = !LocalPlayer.GetComponent<vp_FPController>().enabled;
                    debugLog("vp_FPController: " + (LocalPlayer.GetComponent<vp_FPController>().enabled ? "enabled" : "disabled") + "\n");
                    debugLog("vp_FPCamera: " + (LocalPlayer.transform.GetChild(0).GetComponent<vp_FPCamera>().enabled ? "enabled" : "disabled") + "\n");
                    debugLog("NoClip: " + (noClip ? "turned on" : "turned off") + "\n");
                }
                /* Shitty Unity Speed Hack 
                   - affects Animations, reloadtime, spawntime ...
                */
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    Time.timeScale = speedHack;
                    debugLog("SpeedHack: set " + speedHack);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {

                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {

                }

                if (buyeverytime)
                {
                    BuyMenu.inbuyzone = true;
                    BuyMenu.canbuy = true;
                }
                if (unammo)
                {
                    BasePlayer.currweapon.clip = 100;
                }

                if (noClip)
                {
                    rotationX += Input.GetAxis("Mouse X") * lookSpeed;
                    rotationY += Input.GetAxis("Mouse Y") * lookSpeed / 8;
                    rotationY = Mathf.Clamp(rotationY, -90, 90);
                    LocalPlayer.transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                    //LocalPlayer.transform.transform.GetChild(0).localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
                    if (Input.GetKey(KeyCode.W))
                    {
                        LocalPlayer.transform.position += LocalPlayer.transform.forward * moveSpeed;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        LocalPlayer.transform.position += LocalPlayer.transform.forward * moveSpeed * -1f;
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        LocalPlayer.transform.position += LocalPlayer.transform.right * moveSpeed;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        LocalPlayer.transform.position += LocalPlayer.transform.right * moveSpeed * -1f;
                    }
                    if (Input.GetKey(KeyCode.Space))
                    {
                        LocalPlayer.transform.position += LocalPlayer.transform.up * moveSpeed;
                    }
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        LocalPlayer.transform.position += LocalPlayer.transform.up * moveSpeed * -1f;
                    }
                }
            }
        }
        /* GUI stuff */
		private void OnGUI()
		{
            if (hackEnabled)
            {
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUI.color = Color.green;

                Vector3 pos = Camera.main.transform.position;
                GUI.Label(new Rect(1f, 0f, 600f, 40f), "X: " + Mathf.Round(pos.x) + ", Y: " + Mathf.Round(pos.y) + ", Z: " + Mathf.Round(pos.z));
                
                /* Debug window */
                if (!hidedebug)
                    GUI.Box(new Rect(1520f, 0f, 400f, 1080f), debug);
                
                /* ESP */
                for (int i = 0; i < 16 && esp; i++)
                {
                    if (PlayerControll.Player[i] == null || PlayerControll.Player[i].go == null || PlayerControll.Player[i].DeadFlag == 1 || PlayerControll.Player[i].currPos.x > 50 || PlayerControll.Player[i].currPos.y > 50 || PlayerControll.Player[i].currPos.x < -50 || PlayerControll.Player[i].currPos.y < -50)
                        continue;

                    string space = "";
                    for (int a = 0; a < PlayerControll.Player[i].Name.Length / 2; a++)
                    {
                        space += " ";
                    }

                    if (PlayerControll.Player[i].Team == PlayerControll.Player[Client.ID].Team)
                        DrawLabel(PlayerControll.Player[i].currPos + new Vector3(0, 2, 0), PlayerControll.Player[i].Name + "\n" + space + " [" + Mathf.Round(distanceCalc(PlayerControll.Player[i].currPos) / 2) + "m]", Color.green);
                    else
                        DrawLabel(PlayerControll.Player[i].currPos + new Vector3(0, 2, 0), PlayerControll.Player[i].Name + "\n" + space + " [" + Mathf.Round(distanceCalc(PlayerControll.Player[i].currPos) / 2) + "m]", Color.red);
                }

                /* Hack menu */
                if (Menub)
                {
                    GUI.Box(new Rect(50f, 50f, 500f, 300f), "Warmode ist ein Hurensohn - FPS: " + 1f / this.deltaTime);

                    speedHack = GUI.HorizontalSlider(new Rect(60f, 110f, 200f, 20f), 1f, 1f, 10f);

                    if (GUI.Button(new Rect(60f, 130f, 100f, 20f), "Disable Buy Zone"))
                    {
                        buyeverytime = !buyeverytime;
                    }
                    if (GUI.Button(new Rect(60f, 150f, 100f, 20f), "Da"))
                    {
                        Client.cs.send_chat(1, "Da");
                    }
                    if (GUI.Button(new Rect(60f, 170f, 100f, 20f), "suka"))
                    {
                        Client.cs.send_chat(0, "suka");
                    }
                }
            }
		}
	}
}
