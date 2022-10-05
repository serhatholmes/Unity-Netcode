using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedObjectTransform;


    // value types can be assign and usable with this, and only owner can change it
    // all clients can see these changes
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int =56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn(){
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " +newValue._int + "; "+ newValue._bool+ "; " + newValue.message);
        };
    }

    private void Update() {

        // if its not owner, code doesnt work in Update method
        if(!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.T)){

            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            // every user can see
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            //TestServerRpc(new ServerRpcParams());
            // server's id =0, first client's id = 1, works only client
            //TestClientRpc(new ClientRpcParams{Send = new ClientRpcSendParams {TargetClientIds = new List<ulong> { 1 }}});
            /*
            randomNumber.Value = new MyCustomData{
                _int = 10,
                _bool = false,
                message = " hello there! "
            }; */
        }

        if(Input.GetKeyDown(KeyCode.Y)){
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnedObjectTransform.gameObject);
        }
        
        Vector3 moveDir = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if(Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if(Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if(Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams){
        Debug.Log("TestServerRpc " + OwnerClientId+ "; " + serverRpcParams.Receive.SenderClientId);
    }

    //server call this and all clients perform
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams) {
        Debug.Log("testclientRpc");
    }
}
