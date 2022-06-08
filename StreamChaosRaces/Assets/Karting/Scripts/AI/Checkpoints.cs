using System.Collections.Generic;
using UnityEngine;

namespace KartGame.AI
{
    public class Checkpoints : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> chk = new List<GameObject>();

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).CompareTag("CheckPoint") == true)
                    chk.Add(transform.GetChild(i).gameObject);
            }
        }
        public int ExtractNumberFromString(string s1)
        {
            //  chk (10) -> 10 -> int 10
            return System.Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(s1, "[^0-9]", ""));
        }
        public string GetNextCheckpointName(string CurrentCheckpointName)
        {
            int CurrentNumber = ExtractNumberFromString(CurrentCheckpointName);
            int NextNumber = CurrentNumber + 1 > chk.Count - 1 ? 0 : CurrentNumber + 1;
            string NextCheckpointName = string.Format("CheckPoint ({0})", NextNumber);
            return NextCheckpointName;
        }
    }
}