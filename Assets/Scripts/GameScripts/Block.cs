using UnityEngine;

namespace GameScripts
{
    public class Block : MonoBehaviour
    {
        private const float BLOCK_HEIGHT = 0.5f;

        public void SetMaterial(Material material)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length < 1)
            {
                Debug.Log("No renderers found");
                foreach (Block child in GetComponentsInChildren<Block>())
                    child.SetMaterial(material);
                
                return;
            }

            foreach (Renderer renderer in renderers)
                renderer.material = material;
        }

        #region Unity

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Block"))
                return;
            
            GameObject block = collision.gameObject;
            Vector3 blockLocalPosition = block.transform.localPosition;
            
            if (!(blockLocalPosition.y < transform.localPosition.y)) 
                return;
            
            transform.localPosition = new Vector3(blockLocalPosition.x, blockLocalPosition.y + BLOCK_HEIGHT, blockLocalPosition.z);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, block.transform.localRotation.z));
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (!collisionInfo.gameObject.CompareTag("Block"))
                return;

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        private void OnCollisionExit(Collision other)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        #endregion
    }
}