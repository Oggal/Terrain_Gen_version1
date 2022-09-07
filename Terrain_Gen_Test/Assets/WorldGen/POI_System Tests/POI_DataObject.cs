using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[CreateAssetMenu()]
public class POI_DataObject : ScriptableObject
{
	public string Name;
	public int Size;
	[SerializeField]
	private List<POI_StructureRule> structureRules;

	public Mesh BuildMesh(int Seed)
	{
		System.Random random = new System.Random(Seed);
		if (structureRules==null) { return null; }
		foreach(POI_StructureRule r in structureRules)
		{
		//	r.BuildPartVerts(random.Next());
		}
		Mesh Example = new Mesh();
		//
		/////////////
		Vector3[] verts = structureRules[0].BuildPartVerts(Seed,ref Example);
		int[] tris = structureRules[0].BuildPartTris(Seed,ref Example);
		//////////
		///

		Example.vertices = verts;
		Example.triangles = tris;
		Example.RecalculateNormals();
		return Example;
	}

	public void addStructureRule(POI_StructureRule rule)
	{
		if(structureRules == null)
		{
			structureRules = new List<POI_StructureRule>();
		}
		structureRules.Add(rule);
	}
	public int RuleCount()
	{
		if (structureRules == null) return 0;
		return structureRules.Count;
	}

	public POI_StructureRule GetRule(int i) {
#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
		return structureRules[Mathf.Clamp(i,0,structureRules.Count)];

	}


	public void RemoveStructureRule(int i)
	{
		
		structureRules.RemoveAt(i);
#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
	}

#if UNITY_EDITOR
	[ContextMenu("DoSomething")]
	void OpenEditor(){
	POI_DataWizard.Init();
	}
#endif
}


#if UNITY_EDITOR

public class POI_DataWizard : EditorWindow
{
	int Counter = 0;

	[SerializeField]
	public POI_DataObject Target;


	[MenuItem("GameObject/New POIData")]
	public static void Init()
	{
		POI_DataWizard window = (POI_DataWizard)EditorWindow.GetWindow(typeof(POI_DataWizard));
		window.Show();
	}



	void SelectTowerNode() {
		Target.addStructureRule(CreateInstance<POI_SR_Tower>());
	}

	void OnGUI()
	{
		bool Ready = true;
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		try
		{
			Target = EditorGUILayout.ObjectField("Target: ", Target, typeof(POI_DataObject), false) as POI_DataObject;
			if(Target == null) { return; }
			Target.Name = EditorGUILayout.TextField("Text Field", Target.Name);
			GUILayout.BeginHorizontal();
			if (EditorGUILayout.DropdownButton(new GUIContent("Add New Rule"), FocusType.Passive))
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Tower"), false, SelectTowerNode);


				// display the menu
				menu.ShowAsContext();
			}

	


			GUILayout.EndHorizontal();
			

			if (Target.RuleCount()!= 0)
			{
				if (Target.GetRule(Counter) == null) { Target.RemoveStructureRule(Counter); }
				if (Counter >= Target.RuleCount()) { Counter = Target.RuleCount() - 1; }
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("<<",GUILayout.ExpandWidth(true)))
					Counter = Mathf.Abs(--Counter) % Target.RuleCount();
				GUILayout.Label(": " + Target.GetRule(Counter).GetType().ToString() +"("+(Counter+1)+"/"+Target.RuleCount()+"):",GUILayout.ExpandWidth(false));
				if (GUILayout.Button(">>",GUILayout.ExpandWidth(true)))
					Counter = Mathf.Abs(++Counter) % Target.RuleCount();
				GUILayout.EndHorizontal();
			
				Counter = Mathf.Abs(Counter) % Target.RuleCount();
				Target.GetRule(Counter).EditorGUI();


				if (GUILayout.Button(new GUIContent("Remove Rule")))
				{
					Target.RemoveStructureRule(Counter--);
					
				}
			}
			else { Counter = 0; }
			
		}
		catch (System.NullReferenceException e)
		{
			Ready = false;
			Counter = 0;
			Debug.LogException(e);
		}

			GUILayout.Label("This whole area is very under development,\n tread with caution");
			if(GUILayout.Button(new GUIContent("Save Rule To File")))
			{
			
			AssetDatabase.CreateAsset(Target.GetRule(Counter), AssetDatabase.GenerateUniqueAssetPath("Assets/Freaky.asset"));
			AssetDatabase.SaveAssets();
		}


	}
}

#endif


[Serializable()]
public abstract class POI_StructureRule :ScriptableObject
{

	public abstract Vector3[] BuildPartVerts(int Seed,ref Mesh mesh);

	public abstract int[] BuildPartTris(int Seed, ref Mesh mesh);


	protected float getValueInRange(Vector2 Range, ref System.Random r)
	{
		return (float)(r.NextDouble() * (Range.y - Range.x) + Range.x);
	}
#if UNITY_EDITOR

	public abstract void EditorGUI();

	protected void EditorGUI_SetMinMax(ref Vector2 Range, GUIContent Label, float Min = 0, float Max = 1)
	{

		GUILayout.Label(Label);
		GUILayout.BeginHorizontal();
		Range.x = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Minimum:"), Range.x, EditorStyles.miniTextField), Min, Range.y);
		Range.y = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Maximum:"), Range.y, EditorStyles.miniTextField), Range.x, Max);
		GUILayout.EndHorizontal();
	}

	protected void EditorGUI_SetMinMax(ref Vector2Int range, GUIContent Label, int Min = 0, int Max = 1,bool useOneValue = false)
	{
		if (useOneValue)
		{
			range.x = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Side Count: ", "Number of sides to the tower"), range.x), Min, Max);
			range.y = range.x;
		}
		else
		{
			GUILayout.Label(Label);
			GUILayout.BeginHorizontal();
			range.x = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Minimum:"), range.x, EditorStyles.miniTextField), Min, range.y);
			range.y = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Maximum:"), range.y, EditorStyles.miniTextField), range.x, Max);
			GUILayout.EndHorizontal();
		}
	}
#endif
	protected int getValueInRange(Vector2Int Range,ref System.Random r)
	{
		return r.Next(Range.x,Range.y+1);
	}

}




