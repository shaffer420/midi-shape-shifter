﻿using System;
using System.Drawing;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Common;
using MidiShapeShifter.UI;
using System.Collections.Generic;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This object manages the custom editor (UI) for your plugin.
    /// </summary>
    /// <remarks>
    /// When you do not implement a custom editor, 
    /// your Parameters will be displayed in an editor provided by the host.
    /// </remarks>
    public class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;
        private WinFormsControlWrapper<PluginEditorView> _view;

        public PluginEditor(Plugin plugin)
        {
            _plugin = plugin;
            _view = new WinFormsControlWrapper<PluginEditorView>();
            _view.SafeInstance.Init(plugin);
        }

        public Rectangle Bounds
        {
            get { return _view.Bounds; }
        }

        public void Close()
        {
            _view.Close();
        }

        public void KeyDown(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // empty by design
        }

        public void KeyUp(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // empty by design
        }

        public VstKnobMode KnobMode { get; set; }

        public void Open(IntPtr hWnd)
        {
            // make a list of parameters to pass to the dlg.
            var paramList = new List<VstParameterManager>()
                {
                    _plugin.MidiHandler.processor.VariableAMgr,
                    _plugin.MidiHandler.processor.VariableBMgr,
                    _plugin.MidiHandler.processor.VariableCMgr,
                    _plugin.MidiHandler.processor.VariableDMgr
                };

            _view.SafeInstance.InitializeVariableParameters(paramList);

            _view.Open(hWnd);
        }

        public void ProcessIdle()
        {
            // keep your processing short!
        }
    }
}