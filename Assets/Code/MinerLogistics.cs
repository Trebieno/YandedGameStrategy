using System.Collections.Generic;
using UnityEngine;

namespace CodeBase
{
    public class MinerLogistics : MonoBehaviour
    {
        private void Start()
        {
            ObjectStorage.Instance.MinerLogistics = this;
        }

        private Ore FindBestOre(List<Ore> ores, Transform transformObj)
        {
            ores = SortOresByDistance(ores, transformObj);


            foreach (Ore ore in ores)
            {
                int minerCount = GetMinerCountForOre(ore);

                if (minerCount < 3 && ore.Amount > 0)
                    return ore;
            }

            return null;
        }

        private List<Ore> SortOresByDistance(List<Ore> ores, Transform transformObj)
        {
            // —оздаем копию списка, чтобы не измен€ть оригинал
            List<Ore> sortedList = new List<Ore>(ores);

            sortedList.Sort((ore1, ore2) =>
            {
                float distanceToOre1 = Vector3.Distance(transformObj.position, ore1.transform.position);
                float distanceToOre2 = Vector3.Distance(transformObj.position, ore2.transform.position);
                return distanceToOre1.CompareTo(distanceToOre2);
            });

            return sortedList; // ¬озвращаем отсортированный список
        }

        private int GetMinerCountForOre(Ore ore)
        {
            int count = 0;
            foreach (Miner miner in ObjectStorage.Instance.Miners)
                if (miner.TargetOre == ore)
                    count++;            
        
            return count;
        }

        public void Initialization()
        {
            List<Miner> miners = ObjectStorage.Instance.Miners; //.FindAll(x => x.TargetOre == null);
            List<Ore> ores = ObjectStorage.Instance.Ores;

            foreach (Miner miner in miners)
            {
                Ore targetOre = FindBestOre(ores, miner.transform);
                if (targetOre != null)
                {
                    if (miner.Target == ObjectStorage.Instance.Base.transform)
                        return;
                    miner.Target = targetOre.transform;
                    miner.TargetOre = targetOre;
                }
            }
        }

        public void FindOre(Miner miner)
        {
            List<Ore> ores = ObjectStorage.Instance.Ores;
            Ore targetOre = FindBestOre(ores, miner.transform);

            if (targetOre != null)
            {
                miner.Target = targetOre.transform;
                miner.TargetOre = targetOre;                
            }
        }

        public void RemoveTargetOnUnits(Ore ore)
        {
            foreach (Miner miner in ObjectStorage.Instance.Miners)
                if (miner.TargetOre == ore)
                    miner.TargetOre = null;
        }
    }
}

