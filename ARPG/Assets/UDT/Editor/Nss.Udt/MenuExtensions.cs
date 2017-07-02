using Nss.Udt.Boundaries;
using Nss.Udt.Pooling;
using Nss.Udt.Zones;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Editors {
	public class MenuExtensions : Editor {
	
		[MenuItem("Component/UDT/Create Pool Controller", false, 100)]
		private static void CreatePoolController() {
			var root = GetPoolControllerRoot();
			Selection.activeGameObject = root;
		}
		
		[MenuItem("Component/UDT/Create Boundaries Controller", false, 101)]
        private static void CreateBoundariesController() {
            var root = GetBoundariesControllerRoot();
			Selection.activeGameObject = root;
        }
        
        [MenuItem("Component/UDT/Create Zones Controller", false, 102)]
        private static void CreateZonesController() {
            var root = GetZonesControllerRoot();
			Selection.activeGameObject = root;
        }
		
		[MenuItem("Component/UDT/Create Referee Controller", false, 103)]
        private static void CreateRefereeController() {
            var root = GetRefereeControllerRoot();
			Selection.activeGameObject = root;
        }
		
		[MenuItem("Component/UDT/Add Pool", false, 120)]
		private static void AddPool() {
			var root = GetPoolControllerRoot();
			var pool = CreateNewPool(root);
			
			Selection.activeGameObject = pool;
		}

        [MenuItem("Component/UDT/Add Box Zone", false, 121)]
        private static void AddBoxZone() {
			for(int i=0; i < Selection.gameObjects.Length; i++) {
				var bz = Selection.gameObjects[i].GetComponent<BoxZone>();
				
				if(bz == null) {
					bz = Selection.gameObjects[i].AddComponent<BoxZone>();
				}
				
				bz.GetComponent<Collider>().isTrigger = true;
			}
        }
		
		
        
		public static GameObject GetUdtRoot() {
			var root = GameObject.Find(Config.ROOT_NAME);
			
			if(root == null) {
				root = new GameObject(Config.ROOT_NAME);
			}
			
			EnsureUdtControlPanelComponent(root);
			
			return root;
		}
		
		public static GameObject GetPoolControllerRoot() {
			var udt = GetUdtRoot();
			var root = GameObject.Find(Config.ROOT_NAME_POOLS);
			
			if(root == null) {
				root = new GameObject(Config.ROOT_NAME_POOLS);
				root.transform.parent = udt.transform;
			}
			
			EnsurePoolComponent(root);
			
			return root;
		}
		
		public static GameObject GetBoundariesControllerRoot() {
			var udt = GetUdtRoot();
            var root = GameObject.Find(Config.ROOT_NAME_BOUNDARIES);
            
            if(root == null) {
                root = new GameObject(Config.ROOT_NAME_BOUNDARIES);
                root.transform.parent = udt.transform;
            }
            
            EnsureBoundariesComponent(root);
            
            return root;
        }
		
		public static GameObject GetZonesControllerRoot() {
			var udt = GetUdtRoot();
            var root = GameObject.Find(Config.ROOT_NAME_ZONES);
            
            if(root == null) {
                root = new GameObject(Config.ROOT_NAME_ZONES);
                root.transform.parent = udt.transform;
            }
            
            EnsureZonesComponent(root);
            
            return root;
        }
		
		public static GameObject GetRefereeControllerRoot() {
			var udt = GetUdtRoot();
			var root = GameObject.Find(Config.ROOT_NAME_REFEREES);
			
			if(root == null) {
				root = new GameObject(Config.ROOT_NAME_REFEREES);
				root.transform.parent = udt.transform;
			}
			else {
				root.transform.parent = udt.transform;
			}
			
			EnsureRefereeComponent(root);
			
			return root;
		}
		
		private static GameObject CreateNewPool(GameObject root) {
			var pool = new GameObject("new-pool");
			pool.transform.parent = root.transform;
			pool.AddComponent<Pool>();
			
			return pool;
		}
		
		private static void EnsureUdtControlPanelComponent(GameObject root) {
            var c = root.GetComponent<UdtController>();
            
            if(c == null) {
                root.AddComponent<UdtController>();
				Debug.Log("UDT: Created UDT Control Panel");
            }
        }
		
		private static void EnsurePoolComponent(GameObject root) {
			var controller = root.GetComponent<PoolController>();
			
			if(controller == null) {
				root.AddComponent<PoolController>();
				Debug.Log("UDT: Created Pool Controller");
			}
		}
		
		private static void EnsureBoundariesComponent(GameObject root) {
            var c = root.GetComponent<BoundaryController>();
            
            if(c == null) {
                root.AddComponent<BoundaryController>();
				Debug.Log("UDT: Created Boundaries Controller");
            }
        }
		
		private static void EnsureZonesComponent(GameObject root) {
            var c = root.GetComponent<ZoneController>();
            
            if(c == null) {
                root.AddComponent<ZoneController>();
				Debug.Log("UDT: Created Zones Controller");
            }
        }
		
		private static void EnsureRefereeComponent(GameObject root) {
            var c = root.GetComponent<RefereeController>();
            
            if(c == null) {
                root.AddComponent<RefereeController>();
				Debug.Log("UDT: Created Referee Controller");
            }
        }
	}
}