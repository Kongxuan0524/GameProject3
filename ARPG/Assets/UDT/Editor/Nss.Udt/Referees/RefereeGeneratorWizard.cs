using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using Nss.Udt.Editors;

public class RefereeGeneratorWizard : ScriptableWizard {

	public MonoScript targetComponent;
	public string savePath = Application.dataPath;
	
	private string targetError;
	
	[MenuItem("Component/UDT/Create Strong-Typed Referee Component", false, 104)]
	private static void CreateWizard() {
		ScriptableWizard.DisplayWizard<RefereeGeneratorWizard>("UDT: Referee Generator", "Create");
	}
	
	private void OnWizardCreate() {
		GenerateReferee();
	}
	
	private void OnWizardUpdate() {
		helpString = "Select a MonoBehaviour script component in the project window that you want a referee manager component created for";
		
		if(!targetComponent) {
			targetError = "Please pick a script that you have not already made a referee for...";
		}
		else {
			targetError = "";
		}
		
		errorString = !string.IsNullOrEmpty(targetError) ? targetError : "";
		
		isValid = string.IsNullOrEmpty(targetError);
	}
	
	private void GenerateReferee() {
		string message = string.Empty;
		StringBuilder builder = new StringBuilder();
		
		string ns = "";
		
		if(targetComponent.text.Contains("namespace")) {
			int nsIndex = targetComponent.text.IndexOf("namespace", StringComparison.OrdinalIgnoreCase) + 9;
			
			ns = "using " + targetComponent.text.Substring(
				nsIndex,
				targetComponent.text.IndexOf("{", nsIndex) - nsIndex - 1
				) + ";";
		}
		
		builder.AppendLine(ns);
		builder.AppendLine("using Nss.Udt.Referee;");
		builder.AppendFormat("public class {0} : RefereeManager<{1}> ",
			targetComponent.name + "Referee",
			targetComponent.name);
		
		builder.AppendLine("{ }");
		
		try {
			string script = Path.Combine(savePath, targetComponent.name + "Referee.cs");
			
			using(TextWriter writer = File.CreateText(script)) {
				writer.Write(builder.ToString());
				writer.Close();
			}
			
			AssetDatabase.ImportAsset(targetComponent.name + "Referee.cs");
			AssetDatabase.Refresh();
			
			message = "Success!  Created referee for " + targetComponent.name;
			message += "\n\nRemember to configure the OnEnable and OnDisable events in " + targetComponent.name;
			message += "\n\n\nFilename: " + targetComponent.name + "Referee.cs";
			message += "\n\nLocation: " + savePath;
		}
		catch {
			message = "Error!  Failed to referee for " + targetComponent.name;
			message += "\nMake sure the referee does not already exist and your paths are correct and try again.";
		}
		finally {
				
			EditorUtility.DisplayDialog("UDT: Referee Generator",
					message,
					"Ok");
		}
    }
}