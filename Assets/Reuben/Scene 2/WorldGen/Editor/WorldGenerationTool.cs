
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using WorlGenSetUp;

namespace WorldGenEditor
{
    [CustomEditor(typeof(WorldGen))]
    public class WorldGenerationTool : Editor
    {
        public WorldGen worldGen;
        public VisualTreeAsset inspectorXML;

        private Button generate;


        public override VisualElement CreateInspectorGUI()
        {
            worldGen = (WorldGen)target;


            VisualElement worldGeneration_inspector = new VisualElement();


            inspectorXML.CloneTree(worldGeneration_inspector);
        
            //generate = new Button();
            generate = worldGeneration_inspector.Q<Button>("Generate");
            generate.RegisterCallback<ClickEvent>(Generate);


            return worldGeneration_inspector;

        
        }


        private void Generate(ClickEvent eventClick)
        {

            worldGen.UseAllGenerateMethods();

        }
   
    }

}

