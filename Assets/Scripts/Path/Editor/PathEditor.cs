using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path Path
    {
        get
        {
            return creator.path;
        }
    }

    const float segmentSelectDistanceThreshold = .1f;
    int selectedSegmentIndex = -1;
    int selectedWidthIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
        }

        bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
        if (isClosed != Path.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            Path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != Path.AutoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle auto set controls");
            Path.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin - creator.transform.position;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split segment");
                Path.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if (!Path.IsClosed)
            {
                Undo.RecordObject(creator, "Add segment");
                Path.AddSegment(mousePos);
            }
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = creator.anchorDiameter * .5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < Path.NumPoints; i += 3)
            {
                float dst = Vector2.Distance(mousePos, Path[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete segment");
                Path.DeleteSegment(closestAnchorIndex);
            }
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;

            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }

        // Check for width handle selection
        if (guiEvent.type == EventType.MouseMove || guiEvent.type == EventType.MouseDown)
        {
            float minDstToWidthHandle = creator.controlDiameter * .5f;
            int newSelectedWidthIndex = -1;

            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                float dst = Vector2.Distance(mousePos, points[1]);
                if (dst < minDstToWidthHandle)
                {
                    minDstToWidthHandle = dst;
                    newSelectedWidthIndex = i;
                }
            }

            if (newSelectedWidthIndex != selectedWidthIndex)
            {
                selectedWidthIndex = newSelectedWidthIndex;
                HandleUtility.Repaint();
            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    void Draw()
    {
        Vector2 pos = creator.transform.position;

        for (int i = 0; i < Path.NumSegments; i++)
        {
            Vector2[] points = Path.GetPointsInSegment(i);
            if (creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(pos + points[1], pos + points[0]);
                Handles.DrawLine(pos + points[2], pos + points[3]);
            }
            Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentCol : creator.segmentCol;
            Handles.color = segmentCol;
            Handles.DrawBezier(pos + points[0], pos + points[3], pos + points[1], pos + points[2], segmentCol, null, 2);

            // Draw width handle
            if (selectedWidthIndex == i)
            {
                Handles.color = Color.blue;
                Handles.DrawSolidDisc(pos + points[1], Vector3.forward, creator.controlDiameter * .5f);
            }
        }

        // Draw width handles
        if (creator.displayControlPoints)
        {
            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                Handles.color = Color.blue;
                EditorGUI.BeginChangeCheck();
                Vector2 newWidthPos = Handles.FreeMoveHandle(pos + points[0], creator.controlDiameter * 3f, Vector3.zero, Handles.CircleHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(creator, "Move width point");
                    Path.MoveWidthHandle(i, (newWidthPos - (pos + points[0])).magnitude);
                }
                EditorGUI.BeginChangeCheck();
                newWidthPos = Handles.FreeMoveHandle(pos + points[3], creator.controlDiameter * 3f, Vector3.zero, Handles.CircleHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(creator, "Move width point");
                    Path.MoveWidthHandle(i+1, (newWidthPos - (pos + points[3])).magnitude);
                }
            }
        }


        // Draw points
        for (int i = 0; i < Path.NumPoints; i++)
        {
            if (i % 3 == 0 || creator.displayControlPoints)
            {
                Handles.color = (i % 3 == 0) ? creator.anchorCol : creator.controlCol;
                float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;
                Vector2 newPos = Handles.FreeMoveHandle(pos + Path[i], handleSize, Vector3.zero, Handles.CircleHandleCap);
                if (Path[i] + pos != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    Path.MovePoint(i, newPos - pos);
                }
            }
        }
    }

    void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath();

        }
    }
}

