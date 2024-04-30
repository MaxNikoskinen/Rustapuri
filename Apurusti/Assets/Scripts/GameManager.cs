using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [System.Serializable]
    public class ItemData
    {
        public string ItemName;
        public Sprite ItemIcon;
        public bool IsRecyclable = true;
        public bool RecyclableResult = false;
        public bool DontMultiplyResult = false;
        public int RecycledScrap;
        public int RecycledFrags;
        public int RecycledTechTrash;
        public int RecycledHQMetal;
        public int RecycledRope;
        public int RecycledFabric;
    }

    public List<ItemData> Items = new List<ItemData>();

    public Dictionary<string, int> recyclerItems = new Dictionary<string, int>();

    [System.NonSerialized] public bool recycleAll = true;

    [System.NonSerialized] public float resultMultiplier = 1.0f;

    private void Start()
    {
        foreach(ItemData data in Items)
        {
            if(data.IsRecyclable)
            {
                recyclerItems.Add(data.ItemName, 0);
            }
        }
    }
}
