﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityExplorer.UI.Models;

namespace UnityExplorer.UI
{
    public class InputFieldRef : UIBehaviourModel
    {
        public InputFieldRef(InputField InputField) 
        { 
            this.InputField = InputField;
            Rect = InputField.GetComponent<RectTransform>();
            PlaceholderText = InputField.placeholder.TryCast<Text>();
            InputField.onValueChanged.AddListener(OnInputChanged);
        }

        public event Action<string> OnValueChanged;

        public InputField InputField;
        public Text PlaceholderText;
        public RectTransform Rect;

        public string Text
        {
            get => InputField.text;
            set => InputField.text = value;
        }

        public bool ReachedMaxVerts => TextGenerator.vertexCount >= UIManager.MAX_TEXT_VERTS;
        public TextGenerator TextGenerator => InputField.cachedInputTextGenerator;

        private bool updatedWanted;

        private void OnInputChanged(string value)
        {
            updatedWanted = true;
        }

        public override void Update()
        {
            if (updatedWanted)
            {
                //LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
                LayoutRebuilder.MarkLayoutForRebuild(Rect);

                OnValueChanged?.Invoke(InputField.text);
                updatedWanted = false;
            }
        }

        public override GameObject UIRoot => InputField.gameObject;

        public override void ConstructUI(GameObject parent)
        {
            throw new NotImplementedException();
        }
    }
}
