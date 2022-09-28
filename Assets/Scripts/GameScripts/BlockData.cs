using UnityEngine;

namespace GameScripts
{
    public enum BlockType
    {
        OneByOne = 0,
        TwoByOne,
        TwoByTwo,
        ThreeByTwo,
    }

    public class BlockData : MonoBehaviour
    {
        public BlockType Type;
    }
}
