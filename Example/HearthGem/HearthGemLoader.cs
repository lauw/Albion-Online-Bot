using UnityEngine;

namespace HearthGem
{
    public class HearthGemLoader
    {
        public static void Load()
        {
            ApplicationMgr.Get().gameObject.AddComponent<ZConsole>();
			ApplicationMgr.Get().gameObject.AddComponent<HearthGem>();
        }
		
        public static void UnLoad()
        {
			Object.Destroy(ApplicationMgr.Get().gameObject.GetComponent<HearthGem>());
			Object.Destroy(ApplicationMgr.Get().gameObject.GetComponent<ZConsole>());
        }
    }
}
