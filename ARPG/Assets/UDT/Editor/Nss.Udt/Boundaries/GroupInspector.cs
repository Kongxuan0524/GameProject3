using System;
using Nss.Udt.Common;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Boundaries.Editors {
    [CustomEditor(typeof(Group))]
    public class GroupInspector : Editor {

        #region Serialized Items
        private Group group;
        private SerializedProperty forceUpdate;
        #endregion

        #region Private Members
        private bool showHeader = true;
        private bool showSegments = false;
        private bool showOps = true;

        private int massApplyPart = 0;
        private float massApplyValue = 0f;
        private Vector3 buffer = Vector3.zero;
        private Tool lastTool = Tool.None;
        #endregion

        #region Unity3D Events
        public override void OnInspectorGUI() {
            Tools.current = Tool.None;
			GUI.changed = false;
			
			EditorToolkit.DrawTitle("Boundaries Group");
            
            DrawToolbar();
			EditorGUILayout.Space();
			
            DrawSettings();
            DrawOperations();
            DrawSegments();

            group.Connect();
			
			EditorToolkit.DrawFooter();
			
			if(GUI.changed) {
            	serializedObject.ApplyModifiedProperties();
            	EditorUtility.SetDirty(target);
			}
        }

        private void OnEnable() {
            group = (Group)target;
            forceUpdate = serializedObject.FindProperty("forceUpdate");

            lastTool = Tools.current;
            Tools.current = Tool.None;
        }

        private void OnDisable() {
            Tools.current = lastTool;
        }

        private void OnSceneGUI() {
            Undo.SetSnapshotTarget(group, "Boundary group modified");
            DrawHandles();

            Event e = Event.current;
            if (e.keyCode == KeyCode.B && e.type == EventType.KeyUp) {
                // Create segment
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit, 1000.0f)) {
                    if (group.segments.Count > 0) {
                        AddSegment(hit.point);
                    }
                    else {
                        // First segment, require two entries
                        if (buffer == Vector3.zero) {
                            Debug.Log("First segment start point registered at " + hit.point + ". Select a second point to complete the segment.");
                            buffer = hit.point;
                        }
                        else {
                            AddSegment(buffer, hit.point);
                            buffer = Vector3.zero;
                        }
                    }
                }

                e.Use();
            }

            if (e.keyCode == KeyCode.C && e.type == EventType.KeyUp) {
                // Create segment
                if (group.segments.Count < 3) {
                    Debug.Log("You must have three or more segments in order to close a group.  Operation aborted.");
                    e.Use();

                    return;
                }

                //group.isClosed = !group.isClosed;

                Debug.Log(group.isClosed ?
                    "Group has been closed." :
                    "Group has been opened."
                );

                e.Use();
            }
			
			if (e.keyCode == KeyCode.F && e.type == EventType.KeyUp) {
				var temp = new GameObject();
				temp.transform.position = group.Centroid;
				
				var prev = Selection.activeTransform;
				Selection.activeTransform = temp.transform;
				SceneView.lastActiveSceneView.FrameSelected();
				Selection.activeTransform = prev;
				
				GameObject.DestroyImmediate(temp);
				
                e.Use();
            }

            if (Input.GetMouseButtonDown(0)) {
                Undo.CreateSnapshot();
                Undo.RegisterSnapshot();
            }
        }
        #endregion

        #region SceneView UI
        private void DrawHandles() {
            Color color = group.color;
            color.a = 1f;

            // Centroid label and mover
            HandlesHelpers.VisibleLabel(
                group.Centroid + Vector3.up * group.height / 2f,
                100f,
                group.name,
                new GUIStyle() {
                    normal = new GUIStyleState() {
                        textColor = color
                    },
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                }
            );

            Vector3 delta = HandlesHelpers.VisiblePositionHandle(group.Centroid, Config.HANDLES_DISTANCE * 2f);
            if ((delta - group.Centroid) != Vector3.zero) {
                group.Translate(delta - group.Centroid);
            }

            // Segments
            GUIStyle segmentLabelStyle = new GUIStyle() {
                normal = new GUIStyleState() {
                    textColor = color
                },
                alignment = TextAnchor.MiddleCenter
            };

            for (int i = 0; i < group.segments.Count; i++) {
                if (group.segments.Count > 1) {
                    HandlesHelpers.VisibleLabel(
                        group.segments[i].Midpoint + Vector3.up * group.height / 2f,
                        Config.HANDLES_DISTANCE,
                        group.segments[i].name,
                        segmentLabelStyle
                    );
                }

                group.segments[i].start = HandlesHelpers.VisiblePositionHandle(group.segments[i].start, Config.HANDLES_DISTANCE);

                if (i == group.segments.Count - 1 && !group.isClosed) {
                    group.segments[i].end = HandlesHelpers.VisiblePositionHandle(group.segments[i].end, Config.HANDLES_DISTANCE);
                }
            }
        } 
        #endregion

        #region Inspector UI
        private void DrawToolbar() {
            EditorGUILayout.BeginHorizontal();
            
			if (GUILayout.Button("< Back", EditorToolkit.LargeToolbarButtonLayoutOption())) {
                Selection.activeGameObject = group.gameObject.transform.parent.gameObject;
            }
			
			EditorGUILayout.Space();
			
			if (GUILayout.Button("Add Segment", EditorToolkit.LargeToolbarButtonLayoutOption())) {
                AddSegment();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawOperations() {
			showOps = EditorToolkit.DrawTitleFoldOut(showOps, "Operations");

            if (showOps) {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Mass Apply", GUILayout.MinWidth(80f));
                massApplyPart = EditorGUILayout.Popup(massApplyPart, new[] { "None", "X", "Y", "Z" }, GUILayout.MinWidth(60f));
                massApplyValue = EditorGUILayout.FloatField(massApplyValue, GUILayout.MinWidth(50f));

                if (GUILayout.Button("Go", EditorToolkit.GoButtonLayoutOption())) {
                    MassApply(massApplyPart, massApplyValue);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }
        }

        private void DrawSettings() {
			showHeader = EditorToolkit.DrawTitleFoldOut(showHeader, "Settings");

            if (showHeader) {
                EditorGUI.indentLevel++;
                group.gameObject.name = EditorGUILayout.TextField("Name", group.name);
                group.color = EditorGUILayout.ColorField("Color", group.color);
                group.layer = EditorGUILayout.LayerField("Layers", group.layer);
                group.isClosed = EditorGUILayout.Toggle("Is Closed", group.isClosed);
                forceUpdate.boolValue = EditorGUILayout.Toggle("Force Editor Update", forceUpdate.boolValue);
                group.height = EditorGUILayout.FloatField("Height", group.height);
                group.depthAnchor = (DepthAnchorTypes)EditorGUILayout.EnumPopup("Depth Anchor", (Enum)group.depthAnchor);
                group.depth = EditorGUILayout.FloatField("Depth", group.depth);
                EditorGUI.indentLevel--;
            }
        }

        private void DrawSegments() {
			showSegments = EditorToolkit.DrawTitleFoldOut(showSegments, "Segments");

            if (showSegments) {
                EditorGUI.indentLevel++;

                if (group.segments.Count > 0) {
                    for (int i = 0; i < group.segments.Count; i++) {
                        EditorGUI.indentLevel++;

                        EditorGUILayout.BeginHorizontal();

                        group.segments[i].name = EditorGUILayout.TextField(group.segments[i].name);
                        if (GUILayout.Button("X", EditorToolkit.CloseButtonLayoutOption())) {
                            group.segments.RemoveAt(i);
                            return;
                        }

                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel++;

                        group.segments[i].start = EditorGUILayout.Vector3Field("Start", group.segments[i].start);

                        if (group.segments.Count - 1 == i && !group.isClosed) {
                            group.segments[i].end = EditorGUILayout.Vector3Field("End", group.segments[i].end);
                        }
                        EditorGUI.indentLevel--;

                        EditorGUILayout.Separator();

                        EditorGUI.indentLevel--;
                    }
                }
                else {
                    EditorGUILayout.HelpBox("No segments.  Click Add Segment in the toolbar or use the SceneView click-to-create method.", MessageType.Info);
                }

                EditorGUI.indentLevel--;
				EditorToolkit.DrawSeparator();
            }
        }
        #endregion

        #region Add Segment to Group
        private void AddSegment() {
            Vector3 s = Vector3.zero;
            Vector3 e = Vector3.zero;

            if (group.segments.Count > 0) {
                s = e = group.segments[group.segments.Count - 1].end;
            }

            AddSegment(s, e);
        }

        private void AddSegment(Vector3 start, Vector3 end) {
            Undo.RegisterSceneUndo("Add segment to group");

            group.segments.Add(new Segment {
                name = "seg-" + group.segments.Count,
                start = start,
                end = end
            });
        }

        private void AddSegment(Vector3 end) {
            Vector3 s = group.segments[group.segments.Count - 1].end;

            AddSegment(s, end);
        }
        #endregion

        #region Operations
        private void MassApply(int part, float value) {
            switch (part) {
                case 1:
                    group.segments.ForEach(s => {
                        s.start.x = value;
                        s.end.x = value;
                    });

                    break;

                case 2:
                    group.segments.ForEach(s => {
                        s.start.y = value;
                        s.end.y = value;
                    });

                    break;

                case 3:
                    group.segments.ForEach(s => {
                        s.start.z = value;
                        s.end.z = value;
                    });

                    break;

                default:
                    return;
            }

            massApplyPart = 0;
            massApplyValue = 0f;
        }
        #endregion
    }
}