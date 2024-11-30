using System.Collections.Generic;
using UnityEngine;

public class MinerLogistics : MonoBehaviour
{
    private void Start()
    {
        ObjectStorage.Instance.MinerLogistics = this;
    }

    private Ore FindBestOre(List<Ore> ores)
    {
        Ore bestOre = null;
        

        foreach (Ore ore in ores)
        {
            int minerCount = GetMinerCountForOre(ore);
            
            if (minerCount < 3 && ore.Amount > 0)
                bestOre = ore;                
        }

        return bestOre;
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
        List<Miner> miners = ObjectStorage.Instance.Miners.FindAll(x => x.TargetOre == null);
        List<Ore> ores = ObjectStorage.Instance.Ores;

        foreach (Miner miner in miners)
        {
            Ore targetOre = FindBestOre(ores);
            if (targetOre != null)
            {
                miner.Target = targetOre.transform;
                miner.TargetOre = targetOre;
            }
        }
    }
}
