using System;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScripts
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }
        
        public GameObject[] arrayOfBlocks;
        public Material[] arrayOfColors;
        
        private static List<GameObject> _listOfGeneratedBlocks;

        #region Blocks
        public static GameObject GetRandomBlock()
        {
            GameObject block = Instantiate(Instance.arrayOfBlocks[Random.Range(0, Instance.arrayOfBlocks.Length)]);
            Block[] scripts = block.GetComponents<Block>();
            
            if (scripts.Length == 0)
            {
                //Debug.LogWarning("Block script not found");
                scripts = RecursiveBlockSearcher(block);
            }

            Material material = Instantiate(Instance.arrayOfColors[Random.Range(0, Instance.arrayOfColors.Length)]);
            foreach (Block script in scripts)
                script.SetMaterial(material);
            
            _listOfGeneratedBlocks ??= new List<GameObject>();
            _listOfGeneratedBlocks.Add(block);
            
            return block;
        }

        public static void DestroyGeneratedBlocks()
        {
            foreach (GameObject block in _listOfGeneratedBlocks)
                Destroy(block);
        }

        private static Block[] RecursiveBlockSearcher(GameObject block)
        {
            int childCount = block.transform.childCount;
            Block[] scripts = Array.Empty<Block>();
            //Debug.Log("Child count: " + childCount);
            if (childCount == 0)
            {
                return scripts;
            }
            
            scripts = block.GetComponents<Block>();
            for (int index = 0; index < childCount; index++)
            {
                Block[] childScripts = RecursiveBlockSearcher(block.transform.GetChild(index).gameObject);
                
                if (childScripts.Length == 0 )
                    continue;
                
                Block[] tmp = new Block[scripts.Length + childScripts.Length];
                scripts.CopyTo(tmp, 0);
                childScripts.CopyTo(tmp, scripts.Length);
                scripts = tmp;
            }

            return scripts;
        }
        #endregion
        #region Unity
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Random.InitState(DateTime.Now.Millisecond);
        }
        #endregion
    }
}
