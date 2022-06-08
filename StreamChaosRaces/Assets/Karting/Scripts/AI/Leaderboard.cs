using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

namespace KartGame.AI
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField]
        private List<KartAgent> car = new List<KartAgent>();
        private TextMeshProUGUI tmpro = null;
        private StringBuilder sb = new StringBuilder();
        private string focuscar = "";

        private void Start()
        {
            //get reference to the leaderboard text component
            tmpro = GameObject.Find("Leaderboard").GetComponent<TextMeshProUGUI>();
            tmpro.text = "Leaderboard";

            Transform lookat = GameObject.FindGameObjectWithTag("CmCam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow;

            //get reference to all the cars
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("Player"))
            {
                car.Add(i.gameObject.GetComponent<KartAgent>());
                if (lookat == i)
                    focuscar = car[car.Count - 1].gameObject.name;
            }

        }
        public int DoLeaderboard(string DriverName)
        {
            int ret = -1;
            sb.Clear();

            //sort the cars by number of checkpoints passed (descending=most to least)
            car = car.OrderByDescending(x => x.checkpoints_passed).ToList();

            //compose the text list of cars
            for (int i = 0; i < car.Count; i++)
            {
                if (car[i].gameObject.name == focuscar)
                    sb.AppendLine(string.Format("{0} {1} <--", i + 1, car[i].gameObject.name));
                else
                    sb.AppendLine(string.Format("{0} {1}", i + 1, car[i].gameObject.name));

                if (car[i].gameObject.name == DriverName)
                    ret = i + 1;
            }
            tmpro.text = sb.ToString();

            return ret;
        }
    }
}