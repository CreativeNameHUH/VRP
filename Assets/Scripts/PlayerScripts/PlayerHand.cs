#if DEBUG
    //#define LOCK_HAND
#endif

using UnityEngine;

namespace PlayerScripts
{
    public class PlayerHand : MonoBehaviour
    {
        public GameObject playerHand;
        
        public GameObject block;
        public bool IsTouchingBlock { get; private set; }
#if LOCK_HAND        
        private Vector3 _basePosition;
        private Vector3 _baseRotation;
#endif
        public Vector3 GetPosition()
        {
            return playerHand.transform.localPosition;
        }

        public Vector3 GetRotation()
        {
            return playerHand.transform.localRotation.eulerAngles;
        }
        public void SetPosition(in Vector3 position)
        {
            playerHand.transform.localPosition = position;
        }

        public void SetRotation(Vector3 rotation)
        {
            playerHand.transform.localRotation = Quaternion.Euler(rotation);
        }

        #region Unity
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Block"))
            {
                IsTouchingBlock = true;
                block = collision.gameObject;
            }
            else
            {
                IsTouchingBlock = false;
                block = null;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            IsTouchingBlock = false;
            block = null;
        }
#if LOCK_HAND
        private void Awake()
        {
            _basePosition = GetPosition();
            _baseRotation = GetRotation();
        }

        private void Update()
        {
            if (IsTouchingBlock)
            {
                //Debug.Log("Touching block");
                return;
            }

            if (_basePosition != GetPosition())
                SetPosition(_basePosition);
            
            if (_baseRotation != GetRotation())
                SetRotation(_baseRotation);

        }
#endif
        #endregion
    }
}