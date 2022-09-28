using UnityEngine;

using GameScripts;
using PlayerScripts;
using TMPro;


namespace UIScripts
{
    public class BlockDataDisplay : MonoBehaviour
    {
        public TextMeshProUGUI blockDataText;
        
        private PlayerController _playerController;

        private string GetBlockType()
        {
            GameObject block = _playerController.PickedUpBlock;
            if (!_playerController || !block)
                return "";

            BlockType blockType = block.GetComponent<BlockData>().Type;

            return blockType switch
            {
                BlockType.OneByOne => "1x1",
                BlockType.TwoByOne => "2x1",
                BlockType.TwoByTwo => "2x2",
                BlockType.ThreeByTwo => "3x2",
                _ => ""
            };
        }
        
        #region Unity
        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }
        private void Update()
        {
            blockDataText.text = "Block Type:" + GetBlockType();
        }
        #endregion
    }
}