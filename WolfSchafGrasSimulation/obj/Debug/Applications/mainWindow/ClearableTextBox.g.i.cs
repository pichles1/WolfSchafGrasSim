﻿#pragma checksum "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AF7BC99457A2E0A30E3E5680BEB179C19E58708C2A7BCE9C7B364251ECA68344"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WolfSchafGrasSimulation.Applications.mainWindow;


namespace WolfSchafGrasSimulation.Applications.mainWindow {
    
    
    /// <summary>
    /// ClearableTextBox
    /// </summary>
    public partial class ClearableTextBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtInput;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbPlaceholder;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClear;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WolfSchafGrasSimulation;component/applications/mainwindow/clearabletextbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 12 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
            this.txtInput.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtInput_TextChanged);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
            this.txtInput.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtInput_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
            this.txtInput.AddHandler(System.Windows.DataObject.PastingEvent, new System.Windows.DataObjectPastingEventHandler(this.txtInput_Pasting));
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbPlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.btnClear = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\..\Applications\mainWindow\ClearableTextBox.xaml"
            this.btnClear.Click += new System.Windows.RoutedEventHandler(this.btnClear_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

