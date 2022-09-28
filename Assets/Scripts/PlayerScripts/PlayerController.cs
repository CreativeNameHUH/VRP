#if DEBUG
    #define USE_KB
    //#define FORCE_XR
#endif

using UnityEngine;

using GameScripts;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        public Transform player;
        public Camera playerCamera;
        public float playerSpeed = 1.5f;

        private XRHandler _xrHandler;
        private PlayerHand _rightHand;

        private bool _isTouchingBlock;
        
        public GameObject PickedUpBlock { get; private set; }

        #region Movement
        private void Move()
        {
            Vector2 input = _xrHandler.GetLeftThumbstickValue() * playerSpeed;
            Vector3 headPosition = _xrHandler.GetHeadPosition();
            player.Translate(input.x + headPosition.x, headPosition.y, input.y + headPosition.z);

#if USE_KB
            Vector3 kbInput = new Vector3(Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime);
            player.Translate(kbInput);
#endif
        }

        private void Rotate()
        {
#if !USE_KB || FORCE_XR
            playerCamera.transform.rotation = _xrHandler.GetHeadRotation();
            
            if (_xrHandler.GetRightThumbstickValue().x == 0.0f)
                return;
            
            if (_pickedUpBlock)
            {
                _pickedUpBlock.transform.Rotate(0, _xrHandler.GetRightThumbstickValue().x * 0.5f, 0);
                return;
            }
            
            Vector3 bodyRotation = player.rotation.eulerAngles;
            player.transform.eulerAngles = new Vector3(bodyRotation.x,
                                                     bodyRotation.y + 45.0f * _xrHandler.GetRightThumbstickValue().normalized.y, 
                                                       bodyRotation.z);
#endif            
#if USE_KB
            Vector3 kbInput = new Vector3(0.0f, 0.0f, 0.0f);

            if (Input.GetKey(KeyCode.Keypad8))
                kbInput.x -= 15.0f;
            if (Input.GetKey(KeyCode.Keypad2))
                kbInput.x += 15.0f;
            if (Input.GetKey(KeyCode.Keypad4))
                kbInput.y -= 15.0f;
            if (Input.GetKey(KeyCode.Keypad6))
                kbInput.y += 15.0f;
            if (Input.GetKey(KeyCode.PageUp))
                kbInput.z += 15.0f;
            if (Input.GetKey(KeyCode.PageDown))
                kbInput.z -= 15.0f;
            
            //Debug.Log("kbInput: " + kbInput);
            playerCamera.transform.localRotation = Quaternion.Euler(kbInput);
            
            if (Input.GetKey(KeyCode.LeftControl))
                return;
            
            //Debug.Log("MouseInput: " + kbInput);
            if (PickedUpBlock)
            {
                kbInput = new Vector3(0.0f, PickedUpBlock.transform.localRotation.eulerAngles.y, 0.0f);
                kbInput.y += Input.GetAxis("Mouse X") * 1.5f;
                PickedUpBlock.transform.localRotation = Quaternion.Euler(kbInput);
                return;
            }
            
            kbInput = player.transform.localRotation.eulerAngles;
            kbInput.y += Input.GetAxis("Mouse X") * 1.0f;
            player.transform.localRotation = Quaternion.Euler(kbInput);
#endif
        }
        #endregion

        #region Interactions
        private void Interact()
        {
#if !USE_KB || FORCE_XR           
            if (_xrHandler.GetRightTriggerValue() > 0.0f && !_pickedUpBlock)
                Grab();
            else if (_xrHandler.GetRightTriggerValue() <= 0.0f && _pickedUpBlock)
                Drop();

            if (_xrHandler.GetLeftTriggerValue() > 0.0f && !_pickedUpBlock)
                SpawnBlock();
#endif            
#if USE_KB
            if (Input.GetKey(KeyCode.Mouse0) && !PickedUpBlock)
                Grab();
            else if (!Input.GetKey(KeyCode.Mouse0) && PickedUpBlock)
                Drop();

            if (Input.GetKeyUp(KeyCode.Mouse1) && !PickedUpBlock)
                SpawnBlock();

            if (Input.GetKeyUp(KeyCode.Backspace))
                Game.DestroyGeneratedBlocks();
#endif
        }
        
        private void Grab()
        {
            if (PickedUpBlock || !_rightHand.IsTouchingBlock)
                return;

            PickedUpBlock = _rightHand.block;
            PickedUpBlock.transform.localRotation = Quaternion.Euler(Vector3.zero);
            PickedUpBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log(PickedUpBlock.name);
        }

        private void Drop()
        {
            PickedUpBlock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            PickedUpBlock = null;
        }

        private void SpawnBlock()
        {
            //Debug.Log("Spawn block");
#if BENCHMARK
            for (int i = 0; i < 1000; i++)
            {
#endif
            PickedUpBlock = Game.GetRandomBlock();
            PickedUpBlock.transform.localPosition = _rightHand.GetPosition() + player.transform.localPosition + Vector3.forward * 2.0f;
#if BENCHMARK
            }
#endif
        }

        private void MoveBlock()
        {
            if (!PickedUpBlock)
                return;
            
            PickedUpBlock.transform.localPosition = _rightHand.GetPosition() + player.transform.localPosition;
        }
        
        #endregion
        
        #region Unity
        private void Awake()
        {
            _xrHandler = XRHandler.GetInstance();
            _rightHand = GetComponentInChildren<PlayerHand>();
        }

        private void Update()
        {
            Rotate();
            Interact();
        }

        private void FixedUpdate()
        {
#if !USE_KB || FORCE_XR
            _rightHand.SetPosition(_xrHandler.GetRightControllerPosition());
#endif            
#if USE_KB
            Vector3 handPosition = _rightHand.GetPosition();
            Vector3 mousePosition = Input.mousePosition.normalized;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                _rightHand.SetPosition(new Vector3(mousePosition.x - 0.5f, 1.1f + mousePosition.y / 1.4f,
                    handPosition.z));
            }
#endif
            MoveBlock();
            Move();
        }

        #endregion
    }
}
